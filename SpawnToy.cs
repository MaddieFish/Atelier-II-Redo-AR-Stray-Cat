using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnToy : MonoBehaviour
{
    public Transform spawnPosition;
    Vector3 randomLocation;
    public GameObject spawnToy;

    // Start is called before the first frame update
    void Start()
    {
        randomLocation = Random.insideUnitSphere * 5; //5 is radius
        randomLocation.y = 0.0f;
    }

    void TargetTracked()
    {
        //Spawn itself
        //Instantiate(spawnFood, spawnPosition.position + randomLocation, spawnFood.transform.rotation);
        Instantiate(spawnToy, spawnPosition.position, spawnToy.transform.rotation);
    }
}
