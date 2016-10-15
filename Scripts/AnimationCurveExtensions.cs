using System.Collections.Generic;
using UnityEngine;

namespace LunraGames
{
	public static class AnimationCurveExtensions
	{
		/// <summary>
		/// Used to see if two curves, possibly null, are equal to each other.
		/// </summary>
		/// <remarks>
		/// The default equality for curves is not very good, probably suffering from floating point issues, so we use 
		/// this more robust method instead.
		/// </remarks>
		/// <returns><c>true</c>, if equal was curvesed, <c>false</c> otherwise.</returns>
		/// <param name="curve0">Curve0.</param>
		/// <param name="curve1">Curve1.</param>
		public static bool CurvesEqual(AnimationCurve curve0, AnimationCurve curve1)
		{
			var changed = (curve0 == null && curve1 != null) || (curve0 != null && curve1 == null);

			if (changed) return changed;

			if (curve0.keys.Length == curve1.keys.Length)
			{
				var pairedKeys = new List<Keyframe>(curve0.keys);
				foreach (var key in curve1.keys)
				{
					int? index = null;
					for (var i = 0; i < pairedKeys.Count; i++)
					{
						var k = pairedKeys[i];
						if (Mathf.Approximately(key.inTangent, k.inTangent) && Mathf.Approximately(key.outTangent, k.outTangent) && Mathf.Approximately(key.time, k.time) && Mathf.Approximately(key.value, k.value))
						{
							index = i;
						}
						if (index.HasValue) break;
					}
					if (index.HasValue)
					{
						pairedKeys.RemoveAt(index.Value);
					}
					else
					{
						changed = true;
						break;
					}
				}
			}
			else changed = true;

			return changed;
		}
	}
}