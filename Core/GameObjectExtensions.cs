using UnityEngine;

namespace LunraGames 
{
	public static class GameObjectExtensions 
	{
		public static T InstantiateChild<T>(
			this GameObject gameObject, 
			T prefab, 
			Vector3? localPosition = null, 
			Vector3? localScale = null,
			Quaternion? localRotation = null,
			bool? setActive = null
		) 
			where T : MonoBehaviour
		{
			localPosition = localPosition.HasValue ? localPosition : Vector3.zero;
			localScale = localScale.HasValue ? localScale : Vector3.one;
			localRotation = localRotation.HasValue ? localRotation : Quaternion.identity;

			T child = Object.Instantiate(prefab).GetComponent<T>();
			child.transform.SetParent(gameObject.transform);
			child.transform.localScale = localScale.Value;
			child.transform.localPosition = localPosition.Value;
			child.transform.localRotation = localRotation.Value;

			if (setActive.HasValue) child.gameObject.SetActive(setActive.Value);
			
			return child;
		}
	}
}