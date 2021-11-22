using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackSegment
{
    private Vector3 _point1, _point2;
    public float Length { get; private set; }

    public Vector3 Point1
    {
        get => _point1;
        set
        {
            _point1 = value;
            ComputeLength();
        }
    }
    
    public Vector3 Point2
    {
        get => _point2;
        set
        {
            _point2 = value;
            ComputeLength();
        }
    }

    public TrackSegment(Vector3 point1, Vector3 point2)
    {
        _point1 = point1;
        _point2 = point2;
        
        ComputeLength();
    }

    void ComputeLength()
    {
        Length = Vector3.Distance(_point1, _point2);
    }

    public Vector3 Lerp(float t)
    {
        return Vector3.Lerp(_point1, _point2, t);
    }
}

public class Platform : MonoBehaviour
{
    [Header("Players Holder")]
    public Transform playersHolder;

    [Header("Translation properties")]
    public Vector3[] points;
    public float duration;
    private Transform _transform;

    [Header("Gizmos properties")]
    public Color gizmosColor;
    public Transform transformParent;
    
    private float _timer;
    private List<TrackSegment> _trackSegments;
    private float _trackLength;

    private void Start()
    {
        _transform = transform;

        _trackSegments = new List<TrackSegment>();
        
        foreach (Tuple<Vector3, Vector3> line in Lines())
        {
            _trackSegments.Add(new TrackSegment(line.Item1, line.Item2));
        }

        _trackLength = _trackSegments.Select(trackSegment => trackSegment.Length).Sum();
    }

    private void FixedUpdate()
    {
        if (_trackSegments.Count == 0) return;
        
        _timer += Time.fixedDeltaTime;
        _timer %= duration;

        float t = _timer / duration;
        float currentTrackAdvancement = t * _trackLength;

        int currentTrackSegmentIndex = 0;
        float trackLengthLeft = currentTrackAdvancement;
        
        for (int i = 0; i < _trackSegments.Count; i++)
        {
            if (trackLengthLeft < _trackSegments[i].Length)
            {
                currentTrackSegmentIndex = i;
                break;
            }

            trackLengthLeft -= _trackSegments[i].Length;
        }
        
        UpdatePlatformPosition(_trackSegments[currentTrackSegmentIndex]
            .Lerp(trackLengthLeft / _trackSegments[currentTrackSegmentIndex].Length));
    }

    private void UpdatePlatformPosition(Vector3 position)
    {
        _transform.SetPositionAndRotation(position, Quaternion.identity);
    }

    private IEnumerable<Tuple<Vector3, Vector3>> Lines()
    {
        if (points.Length > 1 && !ReferenceEquals(transformParent, null))
        {
            Vector3 gizmosParentPosition = transformParent.position;
            Vector3 gizmosParentScale = transformParent.lossyScale;
            Quaternion gizmosParentRotation = transformParent.rotation;
        
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 point1 = gizmosParentPosition + 
                                 gizmosParentRotation * Vector3.Scale(points[i], gizmosParentScale);
            
                Vector3 point2 = gizmosParentPosition +
                                 gizmosParentRotation * Vector3.Scale(points[(i + 1) % points.Length], gizmosParentScale);

                yield return new Tuple<Vector3, Vector3>(point1, point2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (points.Length < 2) return;

        Gizmos.color = gizmosColor;

        foreach (Tuple<Vector3, Vector3> line in Lines())
        {
            Gizmos.DrawLine(line.Item1, line.Item2);
        }
    }
}
