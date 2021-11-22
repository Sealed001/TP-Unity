using System;
using System.Collections;
using UnityEngine;

public class VirusMovementController : MonoBehaviour
{
	public float acceleration;
	
	private Transform _transform;
	private Rigidbody _rigidbody;
	private bool _isTraveling;
	
	private Vector3? _fromGizmos;
	private Vector3? _toGizmos;

	private IEnumerator _travelCoroutine;

	private void Start()
	{
		_transform = GetComponent<Transform>();
		_rigidbody = GetComponent<Rigidbody>();
		
		StartCoroutine(CheckPlayersCoroutine());
	}

	private IEnumerator CheckPlayersCoroutine()
	{
		while (true)
		{
			foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
			{
				if (Vector3.Distance(player.transform.position, _transform.position) < 40)
					CheckPlayers(player);
			}

			yield return new WaitForSeconds(1f);
		}
	}

	private void CheckPlayers(GameObject other)
	{
		if (!_isTraveling)
		{
			_travelCoroutine = Travel(_transform.position,
				other.transform.position + (other.transform.position - _transform.position));
			
			StartCoroutine(_travelCoroutine);
		}
	}

	private IEnumerator Travel(Vector3 from, Vector3 to)
	{
		to = new Vector3(to.x, from.y, to.z);
		
		_toGizmos = to;
		_fromGizmos = from;
		
		_isTraveling = true;
		Vector3 direction = (to - from).normalized;

		while (Vector3.Distance(_transform.position, from) < Vector3.Distance(_transform.position, to) || _rigidbody.velocity.magnitude > 1.5f)
		{
			if (Vector3.Distance(_transform.position, from) < Vector3.Distance(_transform.position, to))
			{
				_rigidbody.AddForce(direction * acceleration * .05f);
			}
			else
			{
				_rigidbody.AddForce(-direction * acceleration * .05f);
			}

			yield return new WaitForSeconds(.05f);
		}

		_isTraveling = false;
		
		_toGizmos = null;
		_fromGizmos = null;

		_travelCoroutine = null;
	}

	private void OnDrawGizmos()
	{
		if (!_fromGizmos.HasValue || !_toGizmos.HasValue) return;
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(_fromGizmos.Value, _toGizmos.Value);
	}
}
