using UnityEngine;
using UnityEngine.AI;

using System;

using EnemyStates;
using JetBrains.Annotations;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour
{
    [Header("View")]
    [Range(0f, 100f)] public float viewDistance = 10f;
    [Range(0f, 360f)] public float viewAngle = 30f;
    public LayerMask viewMask;
    
    [NonSerialized] public NavMeshAgent Agent;
    [NonSerialized] public Transform Transform;
    
    [Header("Debugging")]
    public bool debugStateMachine = false;

    [Header("Waypoints")]
    public Transform[] waypoints;
    public float waypointsCaptureDistance = 0.5f;
    [NonSerialized] public int? currentWaypointIndex;
    [CanBeNull] [NonSerialized] public Transform currentWaypoint;

    private EnemyState _state;
    
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Transform = GetComponent<Transform>();
    }

    private void Start()
    {
        _state = new EnemyState_Patrolling(this);
    }

    private void Update()
    {
        _state = _state.Update(this);
    }
    
    [Button]
    public void SwitchToNextWaypoint()
    {
        if (currentWaypointIndex.HasValue)
        {
            currentWaypointIndex = (currentWaypointIndex.Value + 1) % waypoints.Length;
            currentWaypoint = waypoints[currentWaypointIndex.Value];
        }
    }

    [Button]
    public void SwitchToNearestWaypoint()
    {
        float distance = Single.PositiveInfinity;
        int nearestWaypointIndex = 0;

        for (var index = 0; index < waypoints.Length; index++)
        {
            Transform waypoint = waypoints[index];
            
            float waypointDistance = Vector3.Distance(Transform.position, waypoint.position);

            if (waypointDistance >= distance) continue;

            distance = waypointDistance;
            nearestWaypointIndex = index;
        }

        if (!Single.IsPositiveInfinity(distance))
            SwitchToWaypoint(nearestWaypointIndex);
    }

    public void SwitchToWaypoint(int index)
    {
        if (index < 0 || index >= waypoints.Length)
            return;

        currentWaypointIndex = index;
        currentWaypoint = waypoints[index];
    }
    
    [Button]
    public void GoToWaypoint()
    {
        if (currentWaypoint == null)
            return;
        
        Agent.SetDestination(currentWaypoint.position);
    }

    public bool CanSeePlayer()
    {
        return CanSeePlayer(out bool _);
    }
    
    public bool CanSeePlayer(out bool canGiveAnswer)
    {
        canGiveAnswer = true;
        
        if (ReferenceEquals(Player.instance, null) || ReferenceEquals(Transform, null))
        {
            canGiveAnswer = false;
            return false;
        }

        if (Vector3.Distance(Transform.position, Player.instance.transform.position) > viewDistance)
            return false;

        float angle = Vector3.Angle(
            Player.instance.transform.position - Transform.position,
            Transform.forward
        );

        if (angle > viewAngle / 2f)
            return false;

        return !Physics.Raycast(
            Transform.position + Vector3.up * 0.5f, 
            Player.instance.transform.position - Transform.position,
            out RaycastHit _, Vector3.Distance(Transform.position, Player.instance.transform.position),
            viewMask
        );
    }

    private void OnDrawGizmos()
    {
        NavMeshAgent a = GetComponent<NavMeshAgent>();

        if (ReferenceEquals(a, null)) return;

        Gizmos.color = CanSeePlayer(out bool canGiveAnswer) ? Color.green : Color.red;
        
        if (!canGiveAnswer) return;

        Quaternion rotation = Transform.rotation;
        Vector3 position = Transform.position;
        
        Quaternion rotation1 = Quaternion.Euler(0, rotation.eulerAngles.y - viewAngle / 2, 0);
        Quaternion rotation2 = Quaternion.Euler(0, rotation.eulerAngles.y + viewAngle / 2, 0);
            
        Gizmos.DrawRay(position, rotation1 * Vector3.forward * viewDistance);
        Gizmos.DrawRay(position, rotation2 * Vector3.forward * viewDistance);

        int steps = 80;

        for (float angle = -(viewAngle / 2); angle < viewAngle / 2; angle += viewAngle / steps)
        {
            Quaternion rotation3 = Quaternion.Euler(0, rotation.eulerAngles.y + angle, 0);
            Quaternion rotation4 = Quaternion.Euler(0, rotation.eulerAngles.y + angle + viewAngle / steps, 0);
            
            Gizmos.DrawLine(
                position + rotation3 * Vector3.forward * viewDistance,
                position + rotation4 * Vector3.forward * viewDistance
            );
        }
        
        Gizmos.DrawRay(position, Player.instance.transform.position - Transform.position);
        Gizmos.DrawRay(position, Transform.rotation * Vector3.forward);
    }
}
