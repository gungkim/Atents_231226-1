using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WaypointUser
{
    public float spinSpeed = 720.0f;
    Transform bladeMesh;

    protected override Transform Target
    {
        set
        {
            base.Target = value;
            transform.right = Target.position - transform.position;
        }
    }

    private void Awake()
    {
        bladeMesh = transform.GetChild(0);
        
    }

    private void Update()
    {
        bladeMesh.Rotate(0,0,Time.deltaTime * spinSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IAlive live = collision.gameObject.GetComponent<IAlive>();
        if(live != null)
        {
            live.Die();
        }
    }
}
