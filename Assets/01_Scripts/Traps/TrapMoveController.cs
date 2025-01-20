using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMoveController : MonoBehaviour
{
	public Vector3[] localWaypoints;

	public float startTime;

	Vector3[] globalWaypoints;

	public float speed;
	public bool cyclic;
	public float restartTime;
	float waitTime;
	[Range(0, 2)]
	public float easeAmount; // Ease

	int fromWaypointIndex;
	float currentTime;

	float percentBetweenWaypoints;

	void Start()
	{
		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
		waitTime = startTime;
	}

	public virtual void Update()
	{
		Vector3 velocity = CalculatePlatformMovement();
		transform.Translate(velocity, Space.World);

		currentTime += Time.deltaTime;
	}

	float Ease(float x) // 닷트윈의 Ease 그래프 참조, 그래프
	{
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}
	Vector3 CalculatePlatformMovement()
	{
		if (currentTime < waitTime)
			return Vector3.zero;

		fromWaypointIndex %= globalWaypoints.Length;

		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		//float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]); // 이전장소에서 다음장소 이동 거리 변수
		percentBetweenWaypoints += Time.deltaTime * speed;
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		float easePercentBetweenWaypoints = Ease(percentBetweenWaypoints);

		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easePercentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1)
		{
			percentBetweenWaypoints = 0;
			fromWaypointIndex++;
			if (!cyclic)
			{
				if (fromWaypointIndex >= globalWaypoints.Length - 1)
				{
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
					waitTime = restartTime;
				}
				else
					waitTime = 0;
			}
			else
            {
				if (toWaypointIndex == 0)
					waitTime = restartTime;
				else
					waitTime = 0;
			}
			

			currentTime = 0f;
		}

		return newPos - transform.position;
	}

	struct PassengerMovement
	{
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
		{
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}

	private void OnDrawGizmos()
	{
		if (localWaypoints != null)
		{
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i = 0; i < localWaypoints.Length; i++)
			{
				Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
}
