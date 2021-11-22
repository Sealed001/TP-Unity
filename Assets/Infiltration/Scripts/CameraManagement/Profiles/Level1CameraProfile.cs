using UnityEngine;

namespace CameraManagement.Profiles
{
	public class Level1CameraProfile : CameraProfile
	{
		public override Vector3 GetTargetCameraPosition(Vector3 playerPosition)
		{
			return Vector3.Scale(playerPosition, new Vector3(1, 0, 1)) + new Vector3(0, 7, 0);
		}
	}
}