using UnityEngine;

namespace LunraGames 
{
	public static class GameObjectExtensions 
	{
		public static GameObject InstantiateChild(
			this GameObject gameObject,
			GameObject prefab,
			Vector3? localPosition = null,
			Vector3? localScale = null,
			Quaternion? localRotation = null,
			bool? setActive = null
		)
		{
			localPosition = localPosition.HasValue ? localPosition : Vector3.zero;
			localScale = localScale.HasValue ? localScale : Vector3.one;
			localRotation = localRotation.HasValue ? localRotation : Quaternion.identity;

			var child = Object.Instantiate(prefab).transform;
			child.SetParent(gameObject.transform);
			child.localScale = localScale.Value;
			child.localPosition = localPosition.Value;
			child.localRotation = localRotation.Value;

			if (setActive.HasValue) child.gameObject.SetActive(setActive.Value);

			return child.gameObject;
		}

		public static T InstantiateChild<T>(
			this GameObject gameObject, 
			T prefab, 
			Vector3? localPosition = null, 
			Vector3? localScale = null,
			Quaternion? localRotation = null,
			bool? setActive = null
		) 
			where T : Component
		{
			return gameObject.InstantiateChild(prefab.gameObject, localPosition, localScale, localRotation, setActive).GetComponent<T>();
		}
	}
}