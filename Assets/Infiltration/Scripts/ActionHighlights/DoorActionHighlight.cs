using UnityEngine;

using Actions;

namespace ActionHighlights
{
	public class DoorActionHighlight : ActionHighlight
	{
		public GameObject[] highlightLines;
		
		public GameObject Key;
		public GameObject Lock;
		
		private DoorAction _doorAction;

		private void Awake()
		{
			_doorAction = GetComponent<DoorAction>();
		}

		protected new void Update()
		{
			base.Update();
			
			bool showHighlight = canShowHighlight && _doorAction.lockingMode switch
			{
				DoorLockingMode.None => true,
				DoorLockingMode.Key => Player.instance.keyCount > 0,
				DoorLockingMode.Button => false,
                _ => false
            };

			Lock.SetActive(_doorAction.lockingMode == DoorLockingMode.Key);
			
			Key.SetActive(_doorAction.lockingMode == DoorLockingMode.Key && (showHighlight || _doorAction.asBeenOpenedByAKey));
			
			foreach (GameObject highlightLine in highlightLines)
				highlightLine.SetActive(showHighlight);
		}
	}
}