using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//process the input for the tier3
//this scripts only detects the horizontal touch delta position
public class FourthTierInputController : MonoBehaviour
{

    private ObjectDisplayScript ods;
	void Start () {
        ods = GetComponent<ObjectDisplayScript>();
	}
	

	void Update () {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.touches[0];
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    ods.impulse = touch.deltaPosition.x * -1;
                    break;
                case TouchPhase.Stationary:
                    ods.impulse = 0.0f;
                    break;
            }
        }	
	}
}
