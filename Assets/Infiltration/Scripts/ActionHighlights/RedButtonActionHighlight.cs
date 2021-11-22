using UnityEngine;

using Actions;

namespace ActionHighlights
{
	public class RedButtonActionHighlight : ActionHighlight
	{
		public GameObject[] highlightLines;
		
		private RedButtonAction _buttonAction;

		private void Awake()
		{
			_buttonAction = GetComponent<RedButtonAction>();
		}

		protected new void Update()
		{
			base.Update();
			
			bool showHighlight = canShowHighlight && _buttonAction.mode switch
			{
				RedButtonMode.Toggle => true,
				RedButtonMode.FlipToOn => !_buttonAction.IsOn,
				RedButtonMode.FlipToOff => _buttonAction.IsOn,
				RedButtonMode.Countdown => true,
				_ => false
			};
			
			foreach (GameObject highlightLine in highlightLines)
				highlightLine.SetActive(showHighlight);
		}
	}
}