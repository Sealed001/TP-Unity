using UnityEngine;

using System.Linq;
using Infiltration.Scripts.InputManagement.Behaviors;
using InputManagement.Behaviors;
using UnityEngine.InputSystem;

namespace InputManagement
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField] private Camera _cameraComponent;

		private readonly InputManagerBehavior[] _inputBehaviors =
		{
			new GoToBehavior(),
			new GoToAndDoBehavior()
		};

		private void Update()
		{
			if (!Physics.Raycast(_cameraComponent.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 100))
				return;
			
			string s = hit.transform.tag;

			foreach (InputManagerBehavior inputBehavior in _inputBehaviors)
			{
				if (!inputBehavior.Tags.Contains(s)) continue;
				
				inputBehavior.Hover(hit);
			
				if (Mouse.current.leftButton.wasPressedThisFrame)
					inputBehavior.LeftClick(hit);
	                
				if (Mouse.current.rightButton.wasPressedThisFrame)
					inputBehavior.RightClick(hit);
	                
				break;
			}
		}
	}
}