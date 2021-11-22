using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	[Header("Camera")]
	public Transform cameraTransform;
	public float viewSpeed;
	
	[Header("Movement")]
	public float movementSpeed;
	
	[Header("Jump")]
	public Transform jumpRaycastPointTransform;
	public LayerMask jumpRaycastLayerMask;
	[Range(0f, 5f)] public float jumpRaycastInset = .1f;
	[Range(0f, 5f)] public float jumpRaycastLength = 0.5f;
	public float jumpSpeed = 100f;
	public float lateJumpCountdownDuration = 0.1f;
	

	private Rigidbody _rigidbody;
	private Transform _transform;
	private PlayerPlatformHooker _playerPlatformHooker;
	
	private Vector3 _viewDirection = new Vector3(90, 0, 0);
	private float _lateJumpCountdown;

	private bool _canJump;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_transform = GetComponent<Transform>();
		_playerPlatformHooker = GetComponent<PlayerPlatformHooker>();
	}

	private void Update()
	{
		_canJump = Physics.Raycast(
			new Ray(
				jumpRaycastPointTransform.position + Vector3.up * jumpRaycastInset,
				Vector3.down
			),
			out RaycastHit raycastHit,
			jumpRaycastLength,
			jumpRaycastLayerMask
		);

		if (_canJump)
			_playerPlatformHooker.Hook(raycastHit.transform);
		else
			_playerPlatformHooker.UnHook();

		if (_lateJumpCountdown == 0f) return;

		_lateJumpCountdown = Mathf.Max(_lateJumpCountdown - Time.deltaTime, 0f);

		if (!_canJump) return;

		Vector3 velocity = _rigidbody.velocity;
		
		_rigidbody.velocity = new Vector3(
			velocity.x,
			0,
			velocity.z
		);
		
		_rigidbody.AddForce(Vector3.up * jumpSpeed);
		_lateJumpCountdown = 0f;
	}

	public void Move(Vector2 direction)
	{
		Vector3 movementForce = new Vector3(
			direction.x,
			0,
			direction.y
		);
		
		_rigidbody.AddForce(_transform.localRotation * movementForce * (movementSpeed * Time.deltaTime));
	}
	
	public void Jump()
	{
		if (_canJump)
		{
			_rigidbody.AddForce(Vector3.up * jumpSpeed);
			return;
		}

		_lateJumpCountdown = lateJumpCountdownDuration;
	}

	public void Look(Vector2 direction)
	{
		_viewDirection.x += direction.y * Time.deltaTime * viewSpeed;
		_viewDirection.x = Mathf.Clamp(_viewDirection.x, 10, 170);
		
		_viewDirection.y += direction.x * Time.deltaTime * viewSpeed;
		
		cameraTransform.localRotation = Quaternion.Euler(Vector3.Scale(_viewDirection, Vector3.right));
		
		_transform.localRotation = Quaternion.Euler(Vector3.Scale(_viewDirection, Vector3.up));
	}
}
