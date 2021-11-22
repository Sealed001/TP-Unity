using UnityEngine;

namespace EnemyStates
{
	public class EnemyState_Patrolling : EnemyState
	{
		public EnemyState_Patrolling(Enemy enemy)
        {
            enemy.SwitchToNearestWaypoint();
            enemy.GoToWaypoint();
            
            if (enemy.debugStateMachine)
	            Debug.Log("Patrolling");
        }
		
		public override EnemyState Update(Enemy enemy)
		{
			if (enemy.CanSeePlayer())
				return new EnemyState_Chasing(enemy);

			if (!ReferenceEquals(enemy.currentWaypoint, null))
			{
				if (Vector3.Distance(enemy.currentWaypoint.position, enemy.Transform.position) <
				    enemy.waypointsCaptureDistance)
				{
					enemy.SwitchToNextWaypoint();
					enemy.GoToWaypoint();
				}
			}

			return this;
		}
	}
}