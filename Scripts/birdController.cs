using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class birdController : MonoBehaviour {
	[SerializeField] float objectSpeed = 4;
	[SerializeField] private float resetPositionx = -2.55f;
	[SerializeField] private float startPositionx = 60.0f;
	[SerializeField] private float startPositiony = 60.0f;
	private Rigidbody2D myScriptsRigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
		myScriptsRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    	void Update () {

		if(GameManager.Instance.SpawnBirds == true){
			//keep the bird moving
			transform.Translate(Vector3.right * (objectSpeed * Time.deltaTime), Space.World);

			//move each bird to the beginning when it is off screen
			if (transform.localPosition.x >= resetPositionx){
				Vector3 newPosition = new Vector3(startPositionx, transform.position.y,transform.position.z);
				transform.position = newPosition;
			}

			if(transform.localPosition.y >= 7.0f){
				Debug.Log("Bird went out of Y bounds (top of screen)");
				//reset to starting position
				Vector3 newPosition = new Vector3(startPositionx, startPositiony,transform.position.z);
				transform.position = newPosition;
				//reset the rigid body velocity so it flies straight again
				myScriptsRigidbody2D.velocity = Vector3.zero;
				
			}
			if(transform.localPosition.y <= -7.0f){
				Debug.Log("Bird went out of Y bounds (bottom of screen)");
				Vector3 newPosition = new Vector3(startPositionx, startPositiony,transform.position.z);
				transform.position = newPosition;
				//reset the rigid body velocity so it flies straight again
				myScriptsRigidbody2D.velocity = Vector3.zero;
			}	
		}
	}
}

    
