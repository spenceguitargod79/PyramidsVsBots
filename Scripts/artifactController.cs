using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class artifactController : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private GameObject explodingPrefab;

    	//getters/setters
	public int Health{
		get{
			return health;
		}
		set{
			health = value;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //if health is 0 then hide sprite and play explode prefab, wait and then destroy this gameobject
        Debug.Log("Artifact health = " + health);

        if(health <= 0){
            //disable sprite renderer
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //instantiate explode prefab

            //play sound

            //destroy artifact after the prefab is done, somehow

            //(will need to get game manager to instantiate the artifact when the game starts
            //because it will get destroyed here)
        }
    }

    //if robot collides with artifact, decrease health
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            health-=1;
            Debug.Log("Robot hit the artifact!!!");
        }
    }

    
}
