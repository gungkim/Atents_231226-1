using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어를 천천히 따라가는 카메라
public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public float speed = 3.0f;

    Vector3 offset;

    float length;

    private void Start()
    {
        if(target == null)
        {
            target = GameManager.Instance.Player.transform.GetChild(7);
        }

        offset = transform.position - target.position;
        length = offset.magnitude;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp( transform.position,
            target.position + Quaternion.LookRotation(target.forward) * offset,
            Time.fixedDeltaTime * speed);  
        transform.LookAt(target);          

        Ray ray = new Ray(target.position, transform.position - target.position);
        if( Physics.Raycast(ray, out RaycastHit hitInfo, length) )
        {
            transform.position = hitInfo.point;
        }    

    }
}
