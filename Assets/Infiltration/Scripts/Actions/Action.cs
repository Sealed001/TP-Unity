using System;
using Sirenix.OdinInspector;

using UnityEngine;

namespace Actions
{
	public class Action : MonoBehaviour
	{
		public virtual float captureDistance => 0f;
		public virtual Transform point => transform;

		[Button]
		public virtual void Do()
		{
			
		}

		protected void OnDrawGizmos()
		{
			Vector3 position = point.position;
			
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(position, 0.15f);
			Gizmos.DrawWireSphere(position, captureDistance);
		}
	}
}