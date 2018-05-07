using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{

    public int moveDirection = 1;
    private float timer = 0.0f;
    //the wait time of the enemy in secounds
    private int waitTime = -1;
    private EnemyMoveScript enemyMoveScript;
	// Use this for initialization
	void Start () {
        enemyMoveScript = GetComponent<EnemyMoveScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
            enemyMoveScript.moveSpeed = 0.0f;
            return;
        }
        else 
        {
            waitTime = -1;
            timer = 0;
        }
        enemyMoveScript.moveSpeed = moveDirection * 3;

        Vector3 origin = transform.position;
        origin += Vector3.up * 0.25f * transform.localScale.y;
        origin += Vector3.right * moveDirection * 0.21f * transform.localScale.x;
        Debug.DrawRay(origin, Vector3.right * moveDirection * (Mathf.Abs(enemyMoveScript.moveSpeed) + 0.1f), Color.red);
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.right * moveDirection, out hit, Mathf.Abs(enemyMoveScript.moveSpeed) + 0.1f)) 
        {
            moveDirection *= -1;   
            waitTime = Random.Range(1, 5);
        }
	}
}
