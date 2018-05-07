using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public Transform target;
    public Vector3 distanceToTarget;
    public Vector3 rotationToTarget;
    public GameController gameController;
    private Transform cameraChild;
    private float timer = 0.0f;


    void Start()
    {
        cameraChild = transform.GetChild(0);
        Screen.orientation = ScreenOrientation.Portrait;        
    }
	// Update is called once per frame
	void LateUpdate () {
        switch (gameController.gameState) 
        { 
        
            case GameStates.Started:
                timer += Time.deltaTime;
                transform.Rotate(Vector3.up, Mathf.Sin(timer * 0.5f) * 0.3f);
                break;
            case GameStates.Running:
                transform.position = target.position;
               cameraChild.localPosition = Vector3.Lerp(cameraChild.localPosition,
                                                  distanceToTarget,
                                                  Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 
                                                      Mathf.LerpAngle(transform.eulerAngles.y, rotationToTarget.y, Time.deltaTime), 
                                                      0);


                cameraChild.localRotation = Quaternion.Euler(Mathf.LerpAngle(cameraChild.localEulerAngles.x, rotationToTarget.x, Time.deltaTime),
                                                        0,
                                                        0);
               //cameraChild.rotation = Quaternion.Euler(rotationToTarget);
                break;
            case GameStates.Dead:
               //transform.position = target.position - Vector3.forward * 0.5f;

               transform.localPosition = Vector3.Lerp(transform.position,
                                                      target.position - Vector3.forward * 0.5f,
                                                      Time.deltaTime * 10);
               cameraChild.localPosition = Vector3.Lerp(cameraChild.localPosition,
                                                  new Vector3(0.0f, 5.0f, -1.0f),
                                                  Time.deltaTime);
               transform.Rotate(Vector3.up, Time.deltaTime * 10.0f);
               cameraChild.localRotation = Quaternion.Euler(Mathf.LerpAngle(cameraChild.localEulerAngles.x, 60.0f, Time.deltaTime),
                                                            0,
                                                            0);
                break;
        }
	}
}
