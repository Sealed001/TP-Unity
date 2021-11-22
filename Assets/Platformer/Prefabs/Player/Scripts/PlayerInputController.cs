using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
	private Vector2 _deltaViewDirection;
	private Vector2 _deltaMovementDirection;

	private PlayerMovementController _playerMovementController;

	private void Start()
	{
		_playerMovementController = GetComponent<PlayerMovementController>();
	}

	private void Update()
	{
		if (_deltaMovementDirection.magnitude != 0)
			_playerMovementController.Move(_deltaMovementDirection);
		
		if (_deltaViewDirection.magnitude != 0)
			_playerMovementController.Look(Vector2.Scale(_deltaViewDirection, Vector2.down + Vector2.right));
	}

	public void Jump(InputAction.CallbackContext callbackContext)
	{
		if (!callbackContext.ReadValueAsButton()) return;
		
		_playerMovementController.Jump();
	}
	
	public void Look(InputAction.CallbackContext callbackContext)
	{
		_deltaViewDirection = callbackContext.ReadValue<Vector2>();
	}
	
	public void Move(InputAction.CallbackContext callbackContext)
	{
		_deltaMovementDirection = callbackContext.ReadValue<Vector2>();
	}

	public void CursorVisibility(InputAction.CallbackContext callbackContext)
	{
		int cursorVisibility = (int)callbackContext.ReadValue<float>();
		
		if (cursorVisibility == 0) return;

		Cursor.visible = cursorVisibility != 1;
		Cursor.lockState = cursorVisibility == -1 ? CursorLockMode.None : CursorLockMode.Locked;
	}
}
