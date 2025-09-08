using RDG;
using System;
using UnityEngine;

public class ParabolaMove : MonoBehaviour
{
    public bool playOnAwake;
    public AnimationCurve heightCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 90f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, 90f, 0f));
    //public AnimationCurve xMovingCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 90f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, 90f, 0f));
    //public AnimationCurve zMovingCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 90f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, 90f, 0f));
    public float timeToReach = 1, height = 3;
    public bool lookForwardOnMove, destroyOnComplete;
    public Transform endPoint;

    Vector3 startPosition, endPosition, lastPostion;
    Vector3 offset = Vector3.zero;
    float time, gap = 0.25f;
    float curveActualLengthInGraph;
    bool initialized;
    public Action action;
    public Vector3 lastDirection;
    public Vector3 lastVelocity;

    // Awake is called before Start
    public void Awake()
    {
        lastPostion = transform.position;
        startPosition = transform.position;

        if (playOnAwake)
            Initialized(endPoint, offset, timeToReach, heightCurve, height, lookForwardOnMove, destroyOnComplete);

    }

    public void Initialized()
    {
        initialized = true;
    }

    public void Initialized(Transform endPoint, Vector3 offset, float timeToReach, AnimationCurve curve, float height, bool lookForwardOnMove, bool destroyOnComplete, Action action = null)
    {
        this.endPoint = endPoint;
        this.offset = offset;
        Execute(timeToReach, curve, height, lookForwardOnMove, destroyOnComplete, action);
    }

    public void Initialized(Vector3 endPositoin, float timeToReach, AnimationCurve curve, float height, bool lookForwardOnMove, bool destroyOnComplete, Action action = null)
    {
        this.endPosition = endPositoin;
        Execute(timeToReach, curve, height, lookForwardOnMove, destroyOnComplete, action);
    }

    public void Initialized(Vector3 endPositoin, Action action = null)
    {
        this.endPosition = endPositoin;
        Execute(timeToReach, heightCurve, height, lookForwardOnMove, destroyOnComplete, action);
    }

    void Execute(float timeToReach, AnimationCurve curve, float height, bool lookForwardOnMove, bool destroyOnComplete, Action action = null)
    {
        this.lookForwardOnMove = lookForwardOnMove;
        this.destroyOnComplete = destroyOnComplete;
        this.action = action;
        this.heightCurve = curve;
        this.height = height;
        this.timeToReach = timeToReach;

        lastPostion = transform.position;
        startPosition = transform.position;
        time = 0;
        curveActualLengthInGraph = curve.length * 2 / 10f;
        initialized = true;
    }

    private void LateUpdate()
    {
        if (!initialized)
        {
            return;
        }

        if (endPoint != null)
            endPosition = endPoint.position + offset;

        time += Time.deltaTime;
        float lerpValue = Mathf.InverseLerp(0, timeToReach, time);
        Vector3 pos = Vector3.Lerp(startPosition, endPosition, lerpValue);
        pos.y += height * heightCurve.Evaluate(Mathf.Lerp(0, heightCurve.keys[heightCurve.keys.Length - 1].time, lerpValue));
        //pos.x *= xMovingCurve.Evaluate(Mathf.Lerp(0, heightCurve.keys[heightCurve.keys.Length - 1].time, lerpValue));
        //pos.z *= zMovingCurve.Evaluate(Mathf.Lerp(0, heightCurve.keys[heightCurve.keys.Length - 1].time, lerpValue));
        transform.position = pos;
        if (lookForwardOnMove)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.position - lastPostion, Vector3.up);
            rotation.x = transform.rotation.x;
            rotation.z = transform.rotation.z;
            transform.rotation = rotation;
        }

        if (lerpValue == 1)
        {
            action?.Invoke();
            initialized = false;
        }
        if (destroyOnComplete && time >= timeToReach)
        {
            Destroy(this);
        }

    }

    private void Update()
    {
        lastDirection = (transform.position - lastPostion).normalized;
        lastVelocity = transform.position - lastPostion;
        lastPostion = transform.position;
    }
}
