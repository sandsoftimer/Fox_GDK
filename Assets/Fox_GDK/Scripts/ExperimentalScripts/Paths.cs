#if DOTWEEN

using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class Paths : MonoBehaviour
{
	public bool runOnStart;
	public Transform target;
	public int loopCount = -10;
	public LoopType loopType;
	public PathMode pathMode = PathMode.Full3D;
	public PathType pathType = PathType.CatmullRom;
	public bool closePath = true;
	public float timeToTravel = 5;
	public Transform wayPointsHolder;

	Vector3[] _waypoints;

    private void Awake()
    {
		_waypoints = new Vector3[wayPointsHolder.childCount];
        for (int i = 0; i < wayPointsHolder.childCount; i++)
        {
			_waypoints[i] = wayPointsHolder.GetChild(i).position;
        }
    }

    void Start()
	{
		if (runOnStart)
			TravelPath();		
	}

	public void TravelPath(Action action = null)
	{
		// Create a path tween using the given pathType, Linear or CatmullRom (curved).
		// Use SetOptions to close the path
		// and SetLookAt to make the targetHole orient to the path itself
		Tween t = target.DOPath(_waypoints, timeToTravel, pathType, pathMode, 10, Color.red)
			.SetOptions(closePath)
			.SetLookAt(0.001f);
		// Then set the ease to Linear and use infinite loops
		t.SetEase(Ease.Linear).SetLoops(loopCount, loopType).OnComplete(()=> {

			action?.Invoke();
		});
	}
}

#endif