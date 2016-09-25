using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace LunraGames
{
	public static class AssetDatabaseExtensions
	{
		public static string GetAbsoluteAssetPath(Object assetObject)
		{
			return Path.Combine(Project.Root.FullName, AssetDatabase.GetAssetPath(assetObject));
		}
	}
}