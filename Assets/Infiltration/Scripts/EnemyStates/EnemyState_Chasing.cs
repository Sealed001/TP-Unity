using UnityEngine;

namespace EnemyStates
{
	public class EnemyState_Chasing : EnemyState
	{
		private float _cannotSeePlayerCountdown;
		private const float CannotSeePlayerCountdownDuration = 2.5f;
		
		public EnemyState_Chasing(Enemy enemy)
		{
			if (enemy.debugStateMachine)
				Debug.Log("Chasing");
		}
		
		public override EnemyState Update(Enemy enemy)
		{
			if (enemy.CanSeePlayer())
			{
				_cannotSeePlayerCountdown = CannotSeePlayerCountdownDuration;
				
				if (Vector3.Distance(enemy.Transform.position, Player.instance.Transform.position) < 0.85f)
				{
					Player.instance.StartReSpawnCoroutine();
				}
			}
				
			else
				_cannotSeePlayerCountdown -= Time.deltaTime;
			
			if (_cannotSeePlayerCountdown <= 0)
                return new EnemyState_Patrolling(enemy);

			enemy.Agent.SetDestination(Player.instance.Transform.position);

			return this;
		}
	}
}