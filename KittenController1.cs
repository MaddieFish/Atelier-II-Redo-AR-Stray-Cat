using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class KittenController1 : MonoBehaviour
{
    private Rigidbody rb;

    //Navigate towards object
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    Transform food; //Reference to food position
    Transform toy; //reference to toy position
    //Transform bed; //reference to bed position

    private bool foodPresent;
    private bool toyPresent;
    //private bool bedPresent;

    //public Transform target1;
    //public Transform target2;
    //public Transform target3;

    //Wander
    public float wanderRadius;
    public float wanderTimer;

    public float timer;

    //Affection counter
    public Text affectionText;
    public int affection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        affection = 0;
        SetAffectionText();

        //food = GameObject.FindWithTag("Food").transform;
        //toy = GameObject.FindWithTag("Toy").transform;
        //bed = GameObject.FindGameObjectWithTag("Bed").transform;

        //food = GameObject.FindWithTag("Food").transform;
        //toy = GameObject.FindWithTag("Toy").transform;

        nav = GetComponent<NavMeshAgent>();
        timer += wanderTimer;

    }

    // Update is called once per frame
    void Update()
    {
        food = GameObject.FindWithTag("Food").transform;
        if (food)
        {
            Debug.Log(food.name);
        }
        else
        {
            Debug.Log("No game object called wibble found");
        }
        /*
        if (food == true){
            foodPresent = true;
        } else
        {
            foodPresent = false;
        }
        */
        toy = GameObject.FindWithTag("Toy").transform;
        if (toy)
        {
            Debug.Log(toy.name);
        }
        else
        {
            Debug.Log("No game object called wibble found");
        }
        /*
        if (toy == true)
        {
            toyPresent = true;
        }
        else
        {
            toyPresent = false;
        }
        */

        //bed = GameObject.FindGameObjectWithTag("Bed").transform;

        timer += Time.deltaTime;
        ActionProbability();
    }

    void ActionProbability()
    {
        int prob = Random.Range(0, 100);

        if (food!=null && prob < 25)
        {
            //timer = 1;
            //nav.SetDestination(target1.position);
            nav.SetDestination(food.position);
        }
        else if (toy!= null && prob > 25 && prob < 40)
        {
            //timer = 1;
            //nav.SetDestination(target2.position);
            nav.SetDestination(toy.position);

        }
        /*
        else if (bedPresent == true && prob > 35 && prob < 40)
        {
            //nav.SetDestination(target2.position);
            nav.SetDestination(bed.position);

        }
        */
        else if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            nav.SetDestination(newPos);
            timer = 0;

        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            //other.gameObject.SetActive(false);

            //Fetch the Renderer from the GameObject
            Renderer rend = other.gameObject.GetComponent<Renderer>() ;

            //Set the main Color of the Material
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);

            //Destroy Object
            other.gameObject.SetActive(false);

            //Destroy(other.gameObject, 1);

            if (affection < 100)
            {
                affection = affection + 5;
                SetAffectionText();
            }
        
        }

        if (other.gameObject.CompareTag("Toy"))
        {
            //Fetch the Renderer from the GameObject
            Renderer rend = other.gameObject.GetComponent<Renderer>();

            //Set the main Color of the Material
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
           
            other.gameObject.SetActive(false);
            //Destroy(other.gameObject, 1);

           if (affection < 100)
            {
                affection = affection + 20;
                SetAffectionText();
            }
         
        }
    }


        void SetAffectionText()
    {
        affectionText.text = "Affection: " + affection.ToString();

    }

}
