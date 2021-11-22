using UnityEngine;

namespace Actions
{
	public class KeyAction : Action
	{
		public override float captureDistance => 0.3f;

		[SerializeField] private Transform _point;
		public override Transform point => _point;

		public override void Do()
		{
			Player.instance.keyCount++;
			print(Player.instance.keyCount);
			Destroy(gameObject);
		}
	}
}