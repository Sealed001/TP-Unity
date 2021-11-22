using UnityEngine;

public class DrowningZone : MonoBehaviour
{
    private Transform _transform;

    private void Awake()
    { 
        _transform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        
        Vector3 bubblesSpawnPoint = otherGameObject.transform.position;
        bubblesSpawnPoint.y = _transform.position.y;

        switch (otherGameObject.tag)
        {
            case "Player":
                otherGameObject.GetComponent<PlayerController>().Die(
                    new DrowningContext(
                        new DrowningData
                        {
                            BubblesSpawnPoint = bubblesSpawnPoint
                        }
                    )
                );
                break;
        }
    }
}
