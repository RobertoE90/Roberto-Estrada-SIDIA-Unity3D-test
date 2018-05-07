using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootController : MonoBehaviour {
    public int horizontalRotationRange = 20;
    private float horizontalRotationAmount = 0.0f;

    public int verticalRotationRange = 20;
    private float verticalRotationAmount = 0.0f;

    public GameObject bullet;
    public Slider forceSlider;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

	//shoots bullets at the screen touched point width a given force
	void Update () {

        if (Input.touchCount >= 1)
        {
            
            Touch touch = Input.touches[0];
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
            if (touch.phase == TouchPhase.Moved) 
            {
                float horizontalRotationDelta = touch.deltaPosition.x * Time.deltaTime * 10.0f;
                if (Mathf.Abs(horizontalRotationAmount + horizontalRotationDelta) < horizontalRotationRange)
                {
                    horizontalRotationAmount += horizontalRotationDelta;
                    transform.Rotate(Vector3.up, horizontalRotationDelta);
                }

                float verticalRotationDelta = touch.deltaPosition.y * Time.deltaTime * -10.0f;
                if (Mathf.Abs(verticalRotationAmount + verticalRotationDelta) < verticalRotationRange)
                {
                    verticalRotationAmount += verticalRotationDelta;
                    transform.GetChild(0).Rotate(Vector3.right, verticalRotationDelta);
                }
                
            }

        }		
	}

    //on shoot button click
    public void shoot() 
    {
        GameObject tempBullet = GameObject.Instantiate(bullet,
                                                       transform.position + transform.TransformDirection(Vector3.up * -0.7f),
                                                       Quaternion.identity);
        Vector3 shootDirection = transform.GetChild(0).TransformDirection(Vector3.forward);
        //the force range is 50-150
        tempBullet.GetComponent<Rigidbody>().AddForce(shootDirection * (forceSlider.value * 100 + 50));
    }
}
