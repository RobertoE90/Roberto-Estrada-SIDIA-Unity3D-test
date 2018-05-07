using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirtsTierInputController : MonoBehaviour
{

    public GameController gameController;
    private RunnerController runner;

    private Vector2 startTounchPos;

    //the minGestureDistance is on inches
    public float minGestureDistanceInches = 5.0f;
    private float minGestureDistance = 100.0f;
    void Start() 
    {
        minGestureDistance = minGestureDistanceInches * Screen.dpi;
        runner = GetComponent<RunnerController>();     
    }

	void Update () {
        //only process the input if the gameState is running
        if (gameController.gameState != GameStates.Running) 
        {
            return;
        }
        if (Input.touchCount >= 1) {
            Touch touch = Input.touches[0];
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
            switch (touch.phase) 
            {
                case TouchPhase.Began:
                    startTounchPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    //It can be used delta position but is less reliable for this case
                    Vector2 delta = touch.position - startTounchPos;
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        if (Mathf.Abs(delta.x) >= minGestureDistance) {
                            if (delta.x > 0)
                            {
                                runner.ProcessInput(MoveDirections.Right);
                            }
                            else {
                                runner.ProcessInput(MoveDirections.Left);
                            }
                        } 
                    }
                    else 
                    {
                        if (Mathf.Abs(delta.y) >= minGestureDistance)
                        {
                            if (delta.y > 0)
                            {
                                runner.ProcessInput(MoveDirections.Up);
                            }
                            else
                            {
                                runner.ProcessInput(MoveDirections.Down);
                            }
                        }
                    }
                    break;
            }
        }
	}
}
