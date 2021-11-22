using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public static Player instance;
	
	public int keyCount = 0;
	public bool asWined = false;
	public bool asLoosed = false;
	public Transform Transform;

	public GameObject winText;
	public GameObject loseText;

	private void Awake()
	{ 
		instance = this;
		Transform = GetComponent<Transform>();
	}
	
	private void Update()
    {
	    winText.SetActive(asWined);
	    loseText.SetActive(asLoosed);
    }

	public void StartReSpawnCoroutine()
	{
		if (!asLoosed)
		{
			asLoosed = true;
			StartCoroutine(ReSpawnCoroutine());
		}
	}

	private IEnumerator ReSpawnCoroutine()
	{
		yield return new WaitForSeconds(3f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}