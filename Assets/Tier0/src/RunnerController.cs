using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour {
    //reference to the game controller script
    public GameController gameController;

    private float gravitySpeed = -0.001f;
    //flag for enabling jumping
    private bool isGrounded = false;

    //runner states
    private RunnerJumpStates runnerState;
    //private RunnerSideStates runnerSideState;

    //moving side state will be treated different, since moving sides and vertical can be done at the same time
    private bool movingSides = false;

    //used for moving between rails
    private int xPositionTarget = 0;

    //used for time dependent actions trigger
    private float actionTimer = 0.0f;

    public Animator animator;

    void Start()
    {
        runnerState = RunnerJumpStates.None;
        animator = GetComponent<Animator>();
    }

    public void Run(float speed) 
    {
        transform.Translate(Vector3.forward * speed);
    }

    //used for handling the runnerStates
    public void Update() 
    {

        if (gameController.gameState == GameStates.Started) {
            return;
        }
        //ascelerates the gravity speed
        if (!isGrounded)
        {
            gravitySpeed -= 98.0f * Time.deltaTime;
            if (gravitySpeed < 0.1f) {
                animator.SetInteger("verticalMoveParameter", -1);
            }
        }

        //used for character move
        Vector3 nextPosition = transform.position;

        Vector3 origin;
        Vector3 end;
        RaycastHit hit;

        
        if (gameController.gameState == GameStates.Dead)
        {
            
            //the character will fall to the ground
            origin = nextPosition;
            end = nextPosition + Vector3.up * gravitySpeed * Time.deltaTime;
            Debug.DrawLine(origin, end, Color.red);

            if (Physics.Linecast(origin, end, out hit))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    nextPosition = hit.point;
                    gravitySpeed = 0.0f;
                }
            }
            else
            {
                nextPosition += Vector3.up * gravitySpeed * Time.deltaTime;
            }
            if (nextPosition.y < 0) {
                nextPosition.y = 0;
            }
            transform.position = nextPosition;
            return;
        }

        
        nextPosition.z += (gameController.gameCurrentSpeed * Time.deltaTime);
        

        if (movingSides) 
        {
            float sidesMoveDisplacement = Time.deltaTime * 10 * Mathf.Sign(xPositionTarget - transform.position.x);
            if (Mathf.Abs(transform.position.x - xPositionTarget) <= sidesMoveDisplacement)
            {
                animator.SetFloat("horizontalMoveParameter", 0.0f);
                nextPosition.x = xPositionTarget;
                movingSides = false;
            }
            else 
            {
                animator.SetFloat("horizontalMoveParameter", Mathf.Sign(sidesMoveDisplacement));
                nextPosition.x += sidesMoveDisplacement;
            }
        }

        //search if ground is in from of the character
        origin = nextPosition + Vector3.up * (0.25f + gameController.gameCurrentSpeed * Time.deltaTime * 0.75f);
        end = nextPosition + Vector3.up * gravitySpeed * Time.deltaTime;



        if (Physics.Linecast(origin, end, out hit))
        {
            //fix the next position of the character to the frint ground surface 
            if (hit.collider.gameObject.tag == "Ground")
            {
                nextPosition = hit.point;
                gravitySpeed = -0.001f;
                if (runnerState == RunnerJumpStates.Ducking)
                {
                    animator.SetInteger("verticalMoveParameter", -3);
                }
                else 
                {
                    animator.SetInteger("verticalMoveParameter", -2);
                }
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
            nextPosition += Vector3.up * gravitySpeed * Time.deltaTime;
        }

        //handles the ducking state
        if (runnerState == RunnerJumpStates.Ducking)
        {
            //ducks only if is grounded
            if (isGrounded)
            {
                if (actionTimer >= 1.0f)
                {
                    GetComponent<CapsuleCollider>().center = Vector3.up * 0.5f;
                    GetComponent<CapsuleCollider>().height = 1.0f;
                    actionTimer = 0.0f;
                    runnerState = RunnerJumpStates.None;
                    animator.SetInteger("verticalMoveParameter", 0);
                }
                else
                {
                    actionTimer += Time.deltaTime;
                }
            }
        }
        transform.position = nextPosition;
    }
    

    public void ProcessInput(MoveDirections direction)
    {
        switch (direction) {
            //moves right
            case MoveDirections.Right:
                movingSides = true;
                xPositionTarget += 2;
                break;
            //moves left
            case MoveDirections.Left:
                movingSides = true;
                xPositionTarget -= 2;
                break;
            //jumps
            case MoveDirections.Up:
                if (isGrounded)
                {
                    if (runnerState == RunnerJumpStates.Ducking) 
                    {
                        GetComponent<CapsuleCollider>().center = Vector3.up * 0.5f;
                        GetComponent<CapsuleCollider>().height = 1.0f;
                        actionTimer = 0.0f;
                    }
                    runnerState = RunnerJumpStates.Jumping;
                    animator.SetInteger("verticalMoveParameter", 1);
                    gravitySpeed = 25;
                    isGrounded = false;
                }
                break;

            //ducks
            case MoveDirections.Down:
                //disables multiple speed ascelerations when is jumping
                if ((runnerState != RunnerJumpStates.Ducking) && (!isGrounded)) 
                {
                    //ascelerates the falling 
                    gravitySpeed -= 40;
                }
                if (isGrounded)
                {
                    animator.SetInteger("verticalMoveParameter", -3);
                }
                runnerState = RunnerJumpStates.Ducking;
                GetComponent<CapsuleCollider>().center = Vector3.up * 0.25f;
                GetComponent<CapsuleCollider>().height = 0.5f;
                actionTimer = 0.0f;
                break;
        }
    }



    //reacts to collitions given the collision object tag
    //stops the game if crashes against a wall
    //enables jumping if collides agains a ground object
    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Ground") && (!isGrounded)) 
        {
            if (runnerState != RunnerJumpStates.Ducking) 
            {
                runnerState = RunnerJumpStates.None;
            }
            isGrounded = true;
        }
        else if (collision.gameObject.tag == "Wall") 
        {
            gameController.gameState = GameStates.Dead;
            animator.SetTrigger("deadTrigger");
            GameObject.Find("Tier0Canvas").GetComponent<Animator>().SetInteger("MenuChangeParameter", 3);
        }
    }


    //used for debuging propouses
    /*void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, 400));
        GUILayout.BeginVertical();
        GUILayout.Label("state V " + runnerState);
        GUILayout.Label("running sides " + movingSides);
        GUILayout.Label("timer " + actionTimer);
        GUILayout.Label("is Grounded " + isGrounded);
        GUILayout.Label("gSpeed " + gravitySpeed);

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/

}
