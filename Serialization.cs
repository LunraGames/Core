using UnityEngine;
using System;
using Newtonsoft.Json;
using LunraGames.Converters;
using Newtonsoft.Json.Converters;

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
	}
}