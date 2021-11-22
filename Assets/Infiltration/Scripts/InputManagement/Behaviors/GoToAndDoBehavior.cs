using ActionHighlights;
using Actions;
using InputManagement.Behaviors;
using UnityEngine;

namespace Infiltration.Scripts.InputManagement.Behaviors
{
	public class GoToAndDoBehavior : InputManagerBehavior
	{
		public override string[] Tags => new [] { "RedButton", "Door", "Key" };

		public override void RightClick(RaycastHit hit)
		{
			if (ReferenceEquals(PlayerMovementController.instance, null))
				return;

			Action action = hit.transform.GetComponent<Action>();
			
			PlayerMovementController.instance.GoToAndDo(action.point.position, action);
		}

		public override void Hover(RaycastHit hit)
		{
			ActionHighlight actionHighlight = hit.transform.GetComponent<ActionHighlight>();
			
			actionHighlight.ShowHighlight();
		}
	}
}