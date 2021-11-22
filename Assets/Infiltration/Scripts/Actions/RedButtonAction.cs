using Sirenix.OdinInspector;

using UnityEngine;

namespace Actions
{
	public enum RedButtonMode
	{
		Toggle,
		FlipToOn,
		FlipToOff,
		Countdown
	}
	
	public class RedButtonAction : Action
	{
		public bool IsOn { private set; get; }
		
		public RedButtonMode mode;
		
		[ShowIf("mode", RedButtonMode.Countdown)]
		public float countdownDuration = 1f;
		
		private float _countdownTimer;
		
		private void Start()
		{
			switch (mode)
			{
				case RedButtonMode.Toggle:
					IsOn = false;
					break;
				case RedButtonMode.FlipToOn:
					IsOn = false;
					break;
				case RedButtonMode.FlipToOff:
					IsOn = true;
					break;
				case RedButtonMode.Countdown:
					IsOn = false;
					break;
			}
		}
		
		private void Update()
		{
			if (mode == RedButtonMode.Countdown)
			{
				_countdownTimer = Mathf.Max(_countdownTimer - Time.deltaTime, 0f);
				IsOn = _countdownTimer != 0f;
			}
		}
		
		public override float captureDistance => 0.2f;

		[SerializeField] private Transform _point;
		public override Transform point => _point;

		public override void Do()
		{
			switch (mode)
			{
				case RedButtonMode.Countdown:
					_countdownTimer = countdownDuration;
					break;
				case RedButtonMode.Toggle:
					IsOn = !IsOn;
					break;
				case RedButtonMode.FlipToOff:
					IsOn = false;
					break;
				case RedButtonMode.FlipToOn:
					IsOn = true;
					break;
			}
		}
	}
}