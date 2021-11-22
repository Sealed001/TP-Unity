using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
