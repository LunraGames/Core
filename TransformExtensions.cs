using UnityEngine;

namespace LunraGames
{
	public static class TransformExtensions
	{
		public static void ClearChildren(this Transform transform)
		{
			for (var i = 0; i < transform.childCount; i++) Object.Destroy(transform.GetChild(i).gameObject);
		}

		public static void SetChildrenActive(this Transform transform, bool active) {
			for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(active);
		}
	}
}