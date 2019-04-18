using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public float force = 20;
    NavMeshAgent nav;               // Reference to the nav mesh agent.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "kitten")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = col.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.left * 1000.0f);
    }
    */
}
