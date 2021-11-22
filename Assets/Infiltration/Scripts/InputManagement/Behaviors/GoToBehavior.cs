using InputManagement.Behaviors;

using UnityEngine;

namespace Infiltration.Scripts.InputManagement.Behaviors
{
	public class GoToBehavior : InputManagerBehavior
	{
		public override string[] Tags => new [] { "Ground" };

		public override void RightClick(RaycastHit hit)
		{
			if (ReferenceEquals(PlayerMovementController.instance, null))
                return;
			
			PlayerMovementController.instance.GoTo(hit.point);
		}
	}
}