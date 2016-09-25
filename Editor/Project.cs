using UnityEngine;
using System.IO;

namespace LunraGames
{
	public static class Project
	{
		public static DirectoryInfo Root { get { return new DirectoryInfo(Application.dataPath).Parent; } }
	}
}