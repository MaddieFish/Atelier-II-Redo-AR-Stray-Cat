using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class ImageTargetDetection : MonoBehaviour,
                                            ITrackableEventHandler
{
    //public GameObject Food;
    public GameObject Spawner;

    Transform FS;
    //Transform FT;
    private TrackableBehaviour mTrackableBehaviour;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Spawn Food

            FS = GameObject.FindWithTag("FoodSpawner").transform;
            //FT = GameObject.FindWithTag("ToySpawner").transform;

                FS.SendMessage("TargetTracked");
                Debug.Log("Target Tracked");

                //FT.SendMessage("TargetTracked");
                //Debug.Log("Target Tracked"); 

        }
        else
        {

        }
    }
}