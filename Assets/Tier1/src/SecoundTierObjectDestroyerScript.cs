using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SecoundTierObjectDestroyerScript : MonoBehaviour
{
    //Updates the ground when the runner has enter the trigger area
    void OnTriggerExit(Collider other)
    {
        GameObject.Destroy(other.gameObject);
    }
}
