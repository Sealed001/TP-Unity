using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string scene;
    
    private void OnTriggerEnter(Collider other)
    {
	    Cursor.visible = true;
	    Cursor.lockState = CursorLockMode.None;
	    SceneManager.LoadScene(scene);
    }
}
