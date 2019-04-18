using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastInteraction : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    //public GameObject Toy;
    
    //GameObject Kitten;
    private readonly GameObject polarbear_coloured;
    GameObject Food;

    // Use this for initialization
    void Start()
    {




    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
                Debug.DrawLine(ray.origin, hit.point);
         

            if (hit.collider.gameObject.tag == "Kitten")
            {
                print("Hit something!");
                hit.transform.SendMessage("HitByRay");

            } else
            {

            }

            if (hit.collider.gameObject.tag == "Food")
            {
                print("Destroy food!");
                //hit.transform.SendMessage("HitByRay");

                Destroy(hit.collider.gameObject);

            }

        }

        /*
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetKey(KeyCode.Mouse0))
            {
                //create game object
                GameObject obj = Instantiate(Toy, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;

            }

        }
        */
    }
}