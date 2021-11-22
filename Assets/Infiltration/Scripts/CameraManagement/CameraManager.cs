using UnityEngine.SceneManagement;
using UnityEngine;

using CameraManagement.Profiles;

namespace CameraManagement
{
	public class CameraManager : MonoBehaviour
	{
		public static CameraManager instance;
		
		[SerializeField] private float _cameraSmoothSpeed;
		[SerializeField] private Transform _playerTransform;
		[SerializeField] private Transform _cameraTransform;
		[SerializeField] private CameraProfile _cameraProfile;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			_cameraProfile = SceneManager.GetActiveScene().name switch
            {
                "Level 1" => new Level1CameraProfile(),
				_ => new Level1CameraProfile()
            };
		}

		private void Update()
		{
			Vector3 targetCameraPosition = _cameraProfile.GetTargetCameraPosition(_playerTransform.position);

			float distance = Vector3.Distance(
				Vector3.Scale(targetCameraPosition, Vector3.right + Vector3.forward),
				Vector3.Scale(_cameraTransform.position, Vector3.right + Vector3.forward)
			);

			if (distance > 2.5f)
			{
				_cameraTransform.position = Vector3.Lerp(
					_cameraTransform.position,
					targetCameraPosition,
					_cameraSmoothSpeed * Time.deltaTime
				);
			}

			_cameraTransform.position = new Vector3(_cameraTransform.position.x, 8, _cameraTransform.position.z);
			
			_cameraTransform.LookAt(_playerTransform);
		}

		private void OnDrawGizmosSelected()
		{
			_cameraProfile?.DrawGizmos();
		}
	}
}