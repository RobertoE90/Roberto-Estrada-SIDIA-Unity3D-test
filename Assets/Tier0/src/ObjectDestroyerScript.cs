using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectDestroyerScript : MonoBehaviour
{
    //Updates the ground when the runner has enter the trigger area
    void OnTriggerEnter(Collider other)
    {
        GameObject.Destroy(other.gameObject);
    }
}
