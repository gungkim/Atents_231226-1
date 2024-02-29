using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointUser : MonoBehaviour
{
    public Waypoints targetWaypoints;

    public float moveSpeed = 5.0f;

    protected Vector3 moveDelta = Vector3.zero;

    Vector3 moveDirection;

    Transform target;

    protected virtual Transform Target
    {
        get => target;
        set
        {
            target = value;
            moveDirection = (target.position - transform.position).normalized;
        }
    }

    bool IsArrived
    {
        get
        {
            return (target.position - transform.position).sqrMagnitude < 0.01f;
        }
    }

    private void Start()
    {
        Target = targetWaypoints.CurrentWaypoint;
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    protected virtual void OnMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.fixedDeltaTime * moveSpeed);

        if (IsArrived)
        {
            OnArrived();
        }
    }

    protected virtual void OnArrived()
    {
        Target = targetWaypoints.GetNextWaypoint();
    }

}
