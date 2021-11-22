using UnityEngine;

using System;

namespace CameraManagement.Profiles
{
	[Serializable]
	public abstract class CameraProfile
	{
		public abstract Vector3 GetTargetCameraPosition(Vector3 playerPosition);
		
		public virtual void DrawGizmos() { }
	}
}