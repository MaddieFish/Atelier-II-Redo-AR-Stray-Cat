using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class KittenControllerTest : MonoBehaviour
{
    private Rigidbody rb;

    //Navigate towards object
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    Transform food; //Reference to food position
    Transform toy; //reference to toy position
    //Transform bed; //reference to bed position

    //Wander
    public float wanderRadius;
    public float wanderTimer;

    public float timer;

    //Affection counter
    public Text affectionText;
    public int affection;

    //Enable colliders 
    private bool enableColFood;
    private bool enableColToy;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        affection = 0;
        SetAffectionText();

        //InvokeRepeating("CalculateTarget", 0, 2);
       
    }

    private void OnEnable()
    {
        nav = GetComponent<NavMeshAgent>();
        timer += wanderTimer;

    }

    /*
    private void CalculateTarget()
    {
        //if (foodPresent && Random.Range(0, 1f) < 0.1f)
        if (foodPresent && Random.Range(0, 1f) < 0.1f)
        {
            targetPosition = food.transform.position;
        }
        else
        {
            targetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        //enableColFood = false;
        //enableColToy = false;

        GameObject[] gameObjectsFood;
        gameObjectsFood = GameObject.FindGameObjectsWithTag("Food");

        //Set Affection from object destruction scripts
        //SetAffectionText();

        if (gameObjectsFood.Length == 0)
        {
            Wander();
            //Debug.Log("No game objects are tagged with 'Food'");
        } else
        {
            food = GameObject.FindWithTag("Food").transform;
            //wanderTimer = 2;

        }

        GameObject[] gameObjectsToy;
        gameObjectsToy = GameObject.FindGameObjectsWithTag("Toy");

        if (gameObjectsToy.Length == 0)
        {

            Wander();
            //Debug.Log("No game objects are tagged with 'Toy'");
        }
        else
        {
            toy = GameObject.FindWithTag("Toy").transform;
            ActionProbability();
            //wanderTimer = 2;


        }


        //bed = GameObject.FindGameObjectWithTag("Bed").transform;

        /*
        OnDestruction onDestruction_scriptFood;
        onDestruction_scriptFood = food.GetComponent<OnDestruction>();

        affection = affection + onDestruction_scriptFood.affection;
        */
        //timer += wanderTimer;
        timer += Time.deltaTime;

    }

    void ActionProbability()
    {
        int prob = Random.Range(0, 100);

        if (food != null && prob < 10)
        {
            Debug.Log("food" + prob + enableColFood + enableColToy);

            //nav.SetDestination(target2.position);
            enableColFood = true;

            nav.SetDestination(food.position);

            //Wander();

            //  WaitAfterInteraction();

          
        }
        else if (toy != null && prob > 10 && prob < 25)
        {
            Debug.Log("toy" + prob + enableColFood + enableColToy);

            //nav.SetDestination(target2.position);
            enableColToy = true;

            nav.SetDestination(toy.position);

            //enableColFood = false;

            //Wander();
            // WaitAfterInteraction();

           
        }
        /*
        else if (bed != null && prob > 35 && prob < 40)
        {
            //nav.SetDestination(target2.position);
            nav.SetDestination(bed.position);

        }
        */
        else
        {
           // enableColFood = false;
            //enableColToy = false;

            Debug.Log("Wander" + prob + enableColFood + enableColToy);
            Wander();
 
        }

    }

    void Wander()
    {
        if (timer >= wanderTimer)
        {

            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            nav.SetDestination(newPos);
            timer = 0;
            Debug.Log("wander" + enableColFood + enableColToy);

            
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
        if (other.gameObject.CompareTag("Food") && enableColFood == true)
        {
                //Fetch the Renderer from the GameObject
                Renderer rend = other.gameObject.GetComponent<Renderer>();

                //Set the main Color of the Material
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.green);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);

                //Destroy Object
                //other.gameObject.SetActive(false);

                Destroy(other.gameObject, 1);

                if (affection < 100)
                {
                    affection = affection + 5;
                    SetAffectionText();
                }

        }

        if (other.gameObject.CompareTag("Toy") && enableColToy == true)
        {
            
                //Fetch the Renderer from the GameObject
                Renderer rend = other.gameObject.GetComponent<Renderer>();

                //Set the main Color of the Material
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.green);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);

                //other.gameObject.SetActive(false);
                Destroy(other.gameObject, 1);

                if (affection < 100)
                {
                    affection = affection + 10;
                    SetAffectionText();
                }
            

        }

    }

    IEnumerator WaitAfterInteraction()
    {
        print(Time.time);
        yield return new WaitForSecondsRealtime(5);
        print(Time.time);
    }

    void SetAffectionText()
    {
        affectionText.text = "Affection: " + affection.ToString();

    }

}
