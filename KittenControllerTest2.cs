using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KittenControllerTest2 : MonoBehaviour
{
    private Rigidbody rb;

    //Navigate towards object
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    Transform food; //Reference to food position
    Transform toy; //reference to toy position
    Transform bed; //reference to bed position

    //Limit to wander field
    public float wanderRadius;

    //Affection counter
    public Text affectionText;
    public int affection;

    //Enable colliders 
    private bool enableColFood;
    private bool enableColToy;
    private bool enableColBed;

    //Random position 
    private Vector3 targetPosition;
    private float repeatRate;

    //Stores the icrease in prob of an action based on increasing affection
    private int probIncrFood;
    private int probIncrToy;
    private int probIncrBed;
    private int probIncrPet;

    //Testing prob output
    public Text foodProbText;
    public Text toyProbText;
    public Text bedProbText;
    public Text petProbText;

    public int foodProb;
    public int toyProb;
    public int bedProb;
    public int petProb;

    //public Text petReactionText;
     


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        affection = 0;
        SetAffectionText();

        foodProb = 25;
        toyProb = 10;
        bedProb = 5;
        SetProbText();

        repeatRate = Random.Range(3, 5);
        InvokeRepeating("CalculateTarget", 0, repeatRate);

    }

    private void OnEnable()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void CalculateTarget()
    {
        SearchForObject();

        int prob = Random.Range(0, 100);

        if (food != null && prob < 25 + probIncrFood)
        {
            enableColFood = true;
            Debug.Log("food" + prob + enableColFood);

            nav.SetDestination(food.position);

            //enableColFood = false;

        }
        else if (toy != null && prob > 25 + probIncrFood && prob < 35 + probIncrToy)
        {
            enableColToy = true;
            Debug.Log("toy" + prob + enableColToy);

            nav.SetDestination(toy.position);

            //enableColToy = false;

        }
        else if (bed != null && prob > 35 + probIncrToy && prob < 40 + probIncrBed)
        {
            enableColBed = true;
            Debug.Log("bed" + prob + enableColBed);

            nav.SetDestination(bed.position);

            //enableColBed = false;

        }
        else
        {
            enableColFood = false;
            enableColToy = false;
            enableColBed = false;

            int chanceIdle = Random.Range(0, 3);

            if (chanceIdle > 0)
            {

                targetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
                nav.SetDestination(targetPosition);

                Debug.Log("Wander" + prob + enableColFood + enableColToy + enableColBed);

            } else
            {
                //StartCoroutine(WaitAfterInteraction());

                Debug.Log("Sitting" + prob + enableColFood + enableColToy + enableColBed);
            }

        }
    }

    private void SearchForObject()
    {
        //Search for Food
        GameObject[] gameObjectsFood;
        gameObjectsFood = GameObject.FindGameObjectsWithTag("Food");

        if (gameObjectsFood.Length == 0)
        {
            //Debug.Log("No game objects are tagged with 'Food'");
        } else if (gameObjectsFood.Length == 1)
        {
            food = GameObject.FindWithTag("Food").transform;
        }
        else
        {     
            food = gameObjectsFood[Random.Range(1, gameObjectsFood.Length)].transform;
        }

        //Search for Toys
        GameObject[] gameObjectsToy;
        gameObjectsToy = GameObject.FindGameObjectsWithTag("Toy");

        if (gameObjectsToy.Length == 0)
        {
            //Debug.Log("No game objects are tagged with 'Toy'");
        }
        else if (gameObjectsFood.Length == 1)
        {
            toy = GameObject.FindWithTag("Toy").transform;
        }
        else
        {
            toy = gameObjectsToy[Random.Range(1, gameObjectsToy.Length)].transform;
        }

        //Search for Bed
        GameObject[] gameObjectsBed;
        gameObjectsBed = GameObject.FindGameObjectsWithTag("Bed");
        if (gameObjectsBed.Length == 0)
        {
            //Debug.Log("No game objects are tagged with 'Bed'");
        }
       /* else if (gameObjectsFood.Length == 1)
        {
        }*/
        else
        {
            //bed = gameObjectsBed[Random.Range(1, gameObjectsBed.Length)].transform; 
            bed = GameObject.FindWithTag("Bed").transform;


        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

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

            Destroy(other.gameObject, repeatRate/2);
            StartCoroutine(WaitAfterInteraction());


            if (affection < 100)
            {
                affection = affection + 5;
                SetAffectionText();

                probIncrFood = probIncrFood + 1;
                probIncrToy = probIncrToy + 2;
                probIncrBed = probIncrBed + 3;
                probIncrPet = probIncrPet + 1;

                CalculateObjectProb();
                SetProbText();
            }

        }

        if (other.gameObject.CompareTag("Toy") && enableColToy == true)
        {
            GetComponent<Rigidbody>().isKinematic = true;

            //Fetch the Renderer from the GameObject
            Renderer rend = other.gameObject.GetComponent<Renderer>();

            //Set the main Color of the Material
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);

            //other.gameObject.SetActive(false);
            //Destroy(other.gameObject, 2);
            StartCoroutine(WaitAfterInteraction());

            GetComponent<Rigidbody>().isKinematic = false;



            if (affection < 100)
            {
                affection = affection + 10;
                SetAffectionText();

                probIncrFood = probIncrFood + 2;
                probIncrToy = probIncrToy + 4;
                probIncrBed = probIncrBed + 6;
                probIncrPet = probIncrPet + 2;


                CalculateObjectProb();
                SetProbText();
            }


        }

        if (other.gameObject.CompareTag("Bed") && enableColBed == true)
        {
            GetComponent<Rigidbody>().isKinematic = true;

            //Fetch the Renderer from the GameObject
            Renderer rend = other.gameObject.GetComponent<Renderer>();

            //Set the main Color of the Material
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);

            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);

            //other.gameObject.SetActive(false);
            //Destroy(other.gameObject, 2);

            StartCoroutine(WaitAfterInteraction());

            GetComponent<Rigidbody>().isKinematic = false;




            if (affection < 100)
            {
                affection = affection + 15;
                SetAffectionText();

                probIncrFood = probIncrFood + 3;
                probIncrToy = probIncrToy + 6;
                probIncrBed = probIncrBed + 9;
                probIncrPet = probIncrPet + 3;


                CalculateObjectProb();
                SetProbText();
            }


        }
    }

    IEnumerator WaitAfterInteraction()
    {
        GetComponent<NavMeshAgent>().speed = 0;

        print(Time.time);

        yield return new WaitForSecondsRealtime(repeatRate/2);

        GetComponent<NavMeshAgent>().speed = 2;

        print(Time.time);
    }

    void SetAffectionText()
    {
        affectionText.text = "Affection: " + affection.ToString();

    }

    void CalculateObjectProb()
    {
        foodProb = 25 + probIncrFood;
        toyProb = (35 + probIncrToy) - (foodProb);
        bedProb = (40 + probIncrBed) - (foodProb + toyProb);
        petProb = (1 + probIncrPet);
    }

    void SetProbText()
    {
        foodProbText.text = "Food Prob: " + foodProb.ToString() + "/100";
        toyProbText.text = "Toy Prob: " + toyProb.ToString() + "/100";
        bedProbText.text = "Bed Prob: " + bedProb.ToString() + "/100";
        petProbText.text = "Pet Prob: " + petProb.ToString() + "/50";

    }

    void HitByRay()
    {

        int pProb = Random.Range(1, 50);

        if (affection < 100)
        {
            if (pProb < 2 + probIncrPet)
            {
                Debug.Log("Purrrr");

                StartCoroutine(WaitAfterInteraction());

                affection = affection + 5;
                SetAffectionText();

                probIncrFood = probIncrFood + 1;
                probIncrToy = probIncrToy + 2;
                probIncrBed = probIncrBed + 3;
                probIncrPet = probIncrPet + 1;

                CalculateObjectProb();
                SetProbText();
            } else
            {
                Debug.Log("Petting Failed");
            }
        }
            
    }
}
