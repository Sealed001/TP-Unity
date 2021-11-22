using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.AI;

namespace Actions
{
	public enum DoorLockingMode
	{
		None,
		Key,
		Button
	}
	
	public class DoorAction : Action
	{
		public DoorLockingMode lockingMode;
		
		[SerializeField]
		private Transform _door;
		[SerializeField]
		private float _doorOpeningSpeed = 0.5f; 
    
		[ShowIf("lockingMode", DoorLockingMode.Button)]
		public RedButtonAction button;
    
		[ShowIf("lockingMode", DoorLockingMode.Key)]
		public bool asBeenOpenedByAKey;
		
		[ShowIf("lockingMode", DoorLockingMode.None)]
		public bool asBeenOpened;

		private OffMeshLink _offMeshLinkComponent;
		private float _doorRotationY = 0f;

		public override float captureDistance => 1.3f;

		private void Awake()
		{
			_offMeshLinkComponent = GetComponent<OffMeshLink>();
		}

		private void Update()
		{
			_offMeshLinkComponent.activated = CanEntitiesPassThrough();
        
			float targetRotationY = CanEntitiesPassThrough() ? 90 : 0;
			_doorRotationY = Mathf.Lerp(_doorRotationY, targetRotationY, Time.deltaTime * _doorOpeningSpeed);
        
			_door.rotation = Quaternion.Euler(0, _doorRotationY, 0);
		}

		private bool CanEntitiesPassThrough()
		{
			switch (lockingMode)
			{
				case DoorLockingMode.Button:
					if (ReferenceEquals(button, null)) return false;
					return button.IsOn;
				case DoorLockingMode.Key:
					return asBeenOpenedByAKey;
				case DoorLockingMode.None:
					return true;
				default:
					return false;
			}
		}

		public override void Do()
		{
			switch (lockingMode)
			{
				case DoorLockingMode.Key:
					if (!asBeenOpenedByAKey && Player.instance.keyCount > 0)
					{
						asBeenOpenedByAKey = true;
						Player.instance.keyCount--;
					}
					break;
				case DoorLockingMode.None:
					asBeenOpened = !asBeenOpened;
					break;
			}
		}

		protected new void OnDrawGizmos()
		{
			if (!ReferenceEquals(button, null))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, button.transform.position + Vector3.up * 0.5f);
			}
			
			base.OnDrawGizmos();
		}
	}
}