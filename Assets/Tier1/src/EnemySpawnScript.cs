using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour {

    public GameObject enemyPrefab;
    
    //timer used for spawning enemies at random time
    private float timer = 0.0f;
    private float nextSpownTimeTrigger = 0.0f;

    void Start() 
    {
        //spawns a enemy betweeen 5 and 15 secounds
        nextSpownTimeTrigger = Random.Range(5, 15);
    }

	// Update is called once per frame
	void Update () {
        if (timer < nextSpownTimeTrigger)
        {
            timer += Time.deltaTime;
        }
        else 
        {
            timer = 0.0f;
            nextSpownTimeTrigger = Random.Range(5, 15);
            GameObject.Instantiate(enemyPrefab, 
                                   transform.position + Vector3.forward * Random.Range(-2.0f, 2.0f) + Vector3.up * 0.2f, 
                                   Quaternion.identity);
        }
	}
}
