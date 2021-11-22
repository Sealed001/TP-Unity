using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.AI;

using Actions;

namespace Infiltration
{
	public class PlayerMovementController : MonoBehaviour
	{
		public static PlayerMovementController instance;
	
		private NavMeshAgent _agent;
		[CanBeNull] private Action _actionToComplete;

		private void Awake()
		{
			instance = this;
			_agent = GetComponent<NavMeshAgent>();
		}

		private void Update()
		{
			if (ReferenceEquals(_actionToComplete, null)) return;

			if (_agent.remainingDistance >= _actionToComplete.captureDistance) return;
		
			_actionToComplete.Do();
			_actionToComplete = null;
		}
	
		public void GoTo(Vector3 position)
		{
			_agent.destination = position;
			_actionToComplete = null;
		}
	
		public void GoToAndDo(Vector3 position, Action action)
		{
			_agent.destination = position;
			_actionToComplete = action;
		}
	}
}