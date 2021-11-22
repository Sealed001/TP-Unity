using UnityEngine;

public class BubblesPadParticles : MonoBehaviour
{
	private void Start()
	{
		Destroy(gameObject, 60f);
	}
}
