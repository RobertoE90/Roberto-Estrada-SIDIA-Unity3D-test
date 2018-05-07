using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//handles all the states of the enemy
//Handles all the movement logic of the enemy
public class EnemyMoveScript : MonoBehaviour {

    
    public float moveSpeed = 0.0f;
    private bool isGrounded = false;
    private float gravitySpeed = 0.0f;

    private Animator animator;
    //used for fliping the animations
    public SpriteRenderer spriteRenderer;

    public GameObject splashEffect;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {

        //ascelerates the gravity speed
        if (!isGrounded)
        {
            gravitySpeed -= 98.0f * Time.deltaTime;
        }

        //used for character move
        Vector3 nextPosition = transform.position;

        nextPosition.x += moveSpeed * Time.deltaTime;

        if (moveSpeed == 0.0f)
        {
            animator.SetInteger("movingParameter", 0);
        }
        else{
            animator.SetInteger("movingParameter", 1);
            spriteRenderer.flipX = (int)(Mathf.Sign(moveSpeed)) > 0;
        }

        Vector3 origin;
        Vector3 end;
        RaycastHit hit;

        //the character will fall to the ground
        origin = nextPosition;
        end = nextPosition + Vector3.up * gravitySpeed * Time.deltaTime;
        Debug.DrawLine(origin, end, Color.red);

        if (Physics.Linecast(origin, end, out hit))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                nextPosition = hit.point;
                gravitySpeed = 0.01f;
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
            nextPosition += Vector3.up * gravitySpeed * Time.deltaTime;
        }
        /*
        if (nextPosition.y < 0)
        {
            nextPosition.y = 0;
        }*/
        transform.position = nextPosition;
   
    }

    //kills the enemy on a hight speed collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            return;
        }
        if (collision.relativeVelocity.magnitude > 2){
            GameObject.Destroy(gameObject);
            GameObject tempSplash = GameObject.Instantiate(splashEffect, transform.position, Quaternion.identity);
            GameObject.Destroy(tempSplash, 1.0f);
        }
    }
}
