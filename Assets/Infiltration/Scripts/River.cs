using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Actions;
using Random = UnityEngine.Random;

public class LifeLimitedPlatform
{
    public GameObject platform;
    public float lifeTime;
}

public class River : MonoBehaviour
{
    public GameObject duckHolder;
    public Transform platformHolder;
    public Transform noPlatformHolder;
    public Transform platformStartPoint;
    public RedButtonAction button;

    public float platformSizeX;
    public float platformSizeZ;
    
    public GameObject platformPrefab;
    public GameObject _noPlatformPrefab;
    public float platformMinSpawnDuration = 5f;
    public float platformMaxSpawnDuration = 14f;
    public float platformLifetime = 7f;

    public float startZ;
    public float endZ;
    
    private bool _triggered;
    private bool _isButtonOn;

    private List<LifeLimitedPlatform> _platforms;
    private List<GameObject> _noPlatforms;

    private List<float[]> _forbiddenZones;
    
    private void Awake()
    {
        duckHolder.SetActive(false);
        _platforms = new List<LifeLimitedPlatform>();
        _noPlatforms = new List<GameObject>();
    }

    private void Update()
    {
        foreach (LifeLimitedPlatform platform in _platforms)
            platform.lifeTime -= Time.deltaTime;

        for (int i = 0; i < _platforms.Count; i++)
        {
            if (_platforms[i].lifeTime <= 0)
            {
                Destroy(_platforms[i].platform);
                _platforms.RemoveAt(i);
                i--;
            }
        }

        if (!_triggered)
        {
            if (button.IsOn != _isButtonOn)
            {
                _isButtonOn = button.IsOn;

                if (_isButtonOn)
                {
                    _triggered = true;
                    duckHolder.SetActive(true);
                    StartCoroutine(SpawnPlatformsCoroutine());
                }
            }
        }
        
        List<float[]> forbiddenZones = new List<float[]>();
        
        if (_platforms.Count == 0)
        {
            forbiddenZones.Add(new [] {
                startZ,
                endZ
            });
        }
        else
        {
            for (int i = 0; i < _platforms.Count - 1; i++)
            {
                GameObject platform1 = _platforms[i].platform;
                GameObject platform2 = _platforms[i + 1].platform;
            
                float[] forbiddenZone = new float[2];
                forbiddenZone[0] = platform1.transform.localPosition.z - platformSizeZ / 2;
                forbiddenZone[1] = platform2.transform.localPosition.z + platformSizeZ / 2;
            
                forbiddenZones.Add(forbiddenZone);
            }

            forbiddenZones = forbiddenZones.Where(
                forbiddenZone =>
                {
                    return !((forbiddenZone[0] < startZ && forbiddenZone[1] < startZ)
                             || (forbiddenZone[0] > endZ && forbiddenZone[1] > endZ));
                }).ToList();

            forbiddenZones.Add(new[]
            {
                _platforms[0].platform.transform.localPosition.z + platformSizeZ / 2,
                endZ
            });
            
            forbiddenZones.Add(new[]
            {
                startZ,
                _platforms[_platforms.Count - 1].platform.transform.localPosition.z - platformSizeZ / 2,
            });

            forbiddenZones = forbiddenZones.Select(
                forbiddenZone => {
                    return new [] {
                        Mathf.Max(startZ, Mathf.Min(endZ, forbiddenZone[0])),
                        Mathf.Max(startZ, Mathf.Min(endZ, forbiddenZone[1]))
                    };
                }).ToList();
        }

        while (forbiddenZones.Count > _noPlatforms.Count)
        {
            GameObject noPlatform = Instantiate(_noPlatformPrefab, noPlatformHolder);
            noPlatform.transform.localPosition = new Vector3(0, 0, 0);
            noPlatform.transform.localScale = new Vector3(0, 0, 0);
            _noPlatforms.Add(noPlatform);
        }

        while (forbiddenZones.Count < _noPlatforms.Count)
        {
            GameObject noPlatform = _noPlatforms[_noPlatforms.Count - 1];
            _noPlatforms.RemoveAt(_noPlatforms.Count - 1);
            Destroy(noPlatform);
        }

        for (int i = 0; i < forbiddenZones.Count; i++)
        {
            _noPlatforms[i].transform.localPosition = new Vector3(
                0, 
                0,
                (forbiddenZones[i][0] + forbiddenZones[i][1]) / 2
            );
            
            _noPlatforms[i].transform.localScale = new Vector3(
                3,
                1,
                 Mathf.Abs(forbiddenZones[i][1] - forbiddenZones[i][0])
            );
        }

        _forbiddenZones = forbiddenZones;
    }

    private IEnumerator SpawnPlatformsCoroutine()
    {
        while (true)
        {
            _platforms.Add(new LifeLimitedPlatform
            {
                lifeTime = platformLifetime,
                platform = Instantiate(platformPrefab, platformStartPoint.position, Quaternion.identity, platformHolder)
            });
            
            yield return new WaitForSeconds(Random.Range(platformMinSpawnDuration, platformMaxSpawnDuration));
        }
    }

    private void OnDrawGizmos()
    {
        if (_forbiddenZones != null)
        {
            foreach (float[] forbiddenZone in _forbiddenZones)
            {
                Gizmos.color = Color.red;
                
                Gizmos.DrawLine(
                    transform.position + new Vector3(-platformSizeX / 2, 1, forbiddenZone[0]),
                    transform.position + new Vector3(platformSizeX / 2, 1, forbiddenZone[0])
                );
                Gizmos.DrawLine(
                    transform.position + new Vector3(-platformSizeX / 2, 1, forbiddenZone[1]),
                    transform.position + new Vector3(platformSizeX / 2, 1, forbiddenZone[1])
                );
                Gizmos.DrawLine(
                    transform.position + new Vector3(-platformSizeX / 2, 1, forbiddenZone[0]),
                    transform.position + new Vector3(platformSizeX / 2, 1, forbiddenZone[1])
                );
                Gizmos.DrawLine(
                    transform.position + new Vector3(platformSizeX / 2, 1, forbiddenZone[0]),
                    transform.position + new Vector3(-platformSizeX / 2, 1, forbiddenZone[1])
                );
            }
        }
        
        Gizmos.color = Color.magenta;
        
        Gizmos.DrawLine(
            transform.position + new Vector3(-platformSizeX / 2, 3, startZ),
            transform.position + new Vector3(platformSizeX / 2, 3, startZ)
        );
        Gizmos.DrawLine(
            transform.position + new Vector3(-platformSizeX / 2, 3, endZ),
            transform.position + new Vector3(platformSizeX / 2, 3, endZ)
        );
    }
}
