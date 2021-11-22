using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundAndLight.Scripts
{
    public class SoundAndLight : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(NextScene());
        }

        private IEnumerator NextScene()
        {
            yield return new WaitForSeconds(15f);
            SceneManager.LoadScene(1);
        }
    }
}
