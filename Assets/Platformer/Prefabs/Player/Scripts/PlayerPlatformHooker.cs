using UnityEngine;

public class PlayerPlatformHooker : MonoBehaviour
{
    public Transform defaultPlayerHolder;

    private Transform _transform;
    private Rigidbody _rigidbody;
    
    private Transform _currentPlatform;
    private Vector2 _platformAngularSpeed = Vector2.zero;

    private void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        
        UnHook();
    }

    public void Hook(Transform platform)
    {
        if (_currentPlatform == platform) return;
        
        _currentPlatform = platform;
        _transform.parent = platform.GetComponent<Platform>().playersHolder;
    }
    
    public void UnHook()
    {
        if (_currentPlatform == defaultPlayerHolder) return;
        
        _currentPlatform = defaultPlayerHolder;
        _transform.parent = defaultPlayerHolder;
    }
}
