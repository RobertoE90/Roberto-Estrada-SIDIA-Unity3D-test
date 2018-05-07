using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisplayScript: MonoBehaviour
{
    public float impulse = 0.0f;

	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(impulse) > 0.0f) 
        {
            impulse += Mathf.Sign(impulse) * Time.deltaTime * -2.0f;
            transform.Rotate(Vector3.up, impulse);
        }

	}
}
