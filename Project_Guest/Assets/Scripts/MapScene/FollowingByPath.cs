using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Paths;

public class FollowingByPath : MonoBehaviour
{
	public enum MovementType
	{
		Moveing,
		Learping
	}

	Paths paths = new Paths();

	public MovementType Type = MovementType.Moveing;
	public List<Tuple<double, double>> path = new List<Tuple<double, double>>(); 
	public float speed = 1;
	public float maxDistance = .1f;
	Vector3 pointInPath = new Vector3();
	public bool onTheWay = false;
	private int i = 1;

	void Start()
	{
		if (path.Count == 0)
		{
			return;
		}
		pointInPath = new Vector3((float)path[0].Item1, (float)path[0].Item2, 0);

		transform.position = pointInPath; //встать на стартовую точку пути

	}

	void Update()
	{
		if (path.Count == 0)
		{
			return;
		}
		onTheWay = true;
		pointInPath = new Vector3((float)path[i].Item1, (float)path[i].Item2, -5);

		if (Type == MovementType.Moveing)
		{ 
			transform.position = Vector3.MoveTowards(transform.position, pointInPath, Time.deltaTime * speed);
		}

		else if (Type == MovementType.Learping)
		{
			transform.position = Vector3.Lerp(transform.position, pointInPath, Time.deltaTime * speed);
		}

		var distanceSqure = (transform.position - pointInPath).sqrMagnitude;

		if (distanceSqure < maxDistance * maxDistance)
		{
			i+=1;
			if (i > path.Count - 1) {
				onTheWay = false;
				i = 1;
				path.Clear();
				return;
			}
			pointInPath = new Vector3((float)path[i].Item1, (float)path[i].Item2, 0);
		}
	}

}