using UnityEngine;

namespace ActionHighlights
{
	public class CoinActionHighlight : ActionHighlight
	{
		public GameObject[] highlightLines;

		protected new void Update()
		{
			base.Update();

			foreach (GameObject highlightLine in highlightLines)
				highlightLine.SetActive(canShowHighlight);
		}
	}
}