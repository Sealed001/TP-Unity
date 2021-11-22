using System.Collections;
using UnityEngine;

public class DuckSoundManager : MonoBehaviour
{
    public float minTime = 1f;
    public float maxTime = 3f;
    public float minPitch = .9f;
    public float maxPitch = 1.1f;
    public AudioClip[] clips = { };

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        StartCoroutine(PlayDuckSounds());
    }

    private IEnumerator PlayDuckSounds()
    {
        while (true)
        {
            _audioSource.pitch = Random.Range(minPitch, maxPitch);
            
            if (clips.Length > 0)
                _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
