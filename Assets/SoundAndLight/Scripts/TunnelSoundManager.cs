using System;
using UnityEngine;
using UnityEngine.Audio;


[Serializable]
public class TunnelTrackedObject
{
	public Transform transform;
	public string mixerParameter = "Undefined Tracked Param";
	public float minVolume;
	public float maxVolume;
}

public class TunnelSoundManager : MonoBehaviour
{
	[Range(0f, 30f)]
	public float size;
	public AudioMixer mixer;
	public TunnelTrackedObject[] tunnelTrackedObjects = { };
	
	private Vector3 _position;

	private void Awake()
	{
		_position = transform.position;
	}

	private void Update()
	{
		foreach (TunnelTrackedObject tunnelTrackedObject in tunnelTrackedObjects)
		{
			float distance = Vector3.Distance(_position, tunnelTrackedObject.transform.position);

			if (distance > size) continue;

			float volume = tunnelTrackedObject.minVolume +
			               (tunnelTrackedObject.maxVolume - tunnelTrackedObject.minVolume) * (1 - distance / size);

			mixer.SetFloat(tunnelTrackedObject.mixerParameter, volume);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, size);
	}
}
