using UnityEngine;

namespace ActionHighlights
{
	public class ActionHighlight : MonoBehaviour
	{
		private float _showHighlightTimer;

		protected bool canShowHighlight => _showHighlightTimer != 0f;

		public void ShowHighlight()
		{
			_showHighlightTimer = 0.1f;
		}

		protected void Update()
		{
			_showHighlightTimer = Mathf.Max(0f, _showHighlightTimer - Time.deltaTime);
		}
	}
}