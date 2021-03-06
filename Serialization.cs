﻿using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using LunraGames.Converters;

namespace LunraGames
{
	public static class Serialization 
	{
		static JsonConverter[] Converters =
		{
			new Vector2Converter(),
			new Vector3Converter(),
			new Vector4Converter(),
			new QuaternionConverter(),
			new ColorConverter(),
			new StringEnumConverter()
		};

		static JsonSerializerSettings _SerializerSettings;

		static JsonSerializerSettings SerializerSettings 
		{
			get
			{
				if (_SerializerSettings == null)
				{
					_SerializerSettings = new JsonSerializerSettings();
					_SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
					foreach (var converter in Converters) _SerializerSettings.Converters.Add(converter);
				}
				return _SerializerSettings;
			}
		}

		static JsonSerializerSettings _VerboseSerializerSettings;

		/// <summary>
		/// Gets the verbose serializer settings, perfect for use of complex generics.
		/// </summary>
		/// <value>The verbose serializer settings.</value>
		static JsonSerializerSettings VerboseSerializerSettings {
			get {
				if (_VerboseSerializerSettings == null) {
					_VerboseSerializerSettings = new JsonSerializerSettings();
					_VerboseSerializerSettings.TypeNameHandling = TypeNameHandling.All;
					_VerboseSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
					foreach (var converter in Converters) _VerboseSerializerSettings.Converters.Add(converter);
				}
				return _VerboseSerializerSettings;
			}
		}

		public static object DeserializeJson(Type type, string json, object defaultValue = null, bool verbose = false)
		{
			if (StringExtensions.IsNullOrWhiteSpace(json)) return defaultValue;

			try 
			{
				return JsonConvert.DeserializeObject(json, type, verbose ? VerboseSerializerSettings : SerializerSettings);
			}
			catch (Exception e)
			{
				Debug.LogError("Problem parsing value: \n\t"+json+"\nReturning default value\n Exception:\n"+e.Message);
				return defaultValue;
			}
		}

		public static T DeserializeJson<T>(string json, T defaultValue = default(T), bool verbose = false)
		{
			if (StringExtensions.IsNullOrWhiteSpace(json)) return defaultValue;

			try 
			{
				return JsonConvert.DeserializeObject<T>(json, verbose ? VerboseSerializerSettings : SerializerSettings);
			}
			catch (Exception e)
			{
				Debug.LogError("Problem parsing value: \n\t"+json+"\nReturning default value\n Exception:\n"+e.Message);
				return defaultValue;
			}
		}

		public static string SerializeJson(object value, bool verbose = false)
		{
			return value == null ? string.Empty : JsonConvert.SerializeObject(value, Formatting.None, verbose ? VerboseSerializerSettings : SerializerSettings);
		}

		/// <summary>
		/// Deserializes the json into a raw dictionary of strings and objects.
		/// </summary>
		/// <returns>The dictionary of key value pairs.</returns>
		/// <param name="json">Json.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="verbose">Verbose.</param>
		public static object DeserializeJsonRaw(string json, object defaultValue = null, bool verbose = false)
		{
			if (StringExtensions.IsNullOrWhiteSpace(json)) return defaultValue;

			try { return DeserializeRaw(JToken.Parse(json)); }
			catch (Exception e)
			{
				Debug.LogError("Problem parsing value: \n\t" + json + "\nReturning default value\n Exception:\n" + e.Message);
				return defaultValue;
			}
		}

		static object DeserializeRaw(JToken token)
		{
			switch (token.Type)
			{
				case JTokenType.Object: return token.Children<JProperty>().ToDictionary(p => p.Name, p => DeserializeRaw(p.Value));
				case JTokenType.Array: return token.Select(t => DeserializeRaw(t)).ToList();
				default: return ((JValue)token).Value;
			}
		}

		public static string Serialize(this object target, bool verbose = false)
		{
			return SerializeJson(target, verbose);
		}
	}
}