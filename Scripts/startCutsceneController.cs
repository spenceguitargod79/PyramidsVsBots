using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class startCutsceneController : MonoBehaviour {
	[SerializeField] float objectSpeed = 4;
	[SerializeField] private float resetPosition = -2.55f;
	[SerializeField] private float startPosition = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    	void Update () {

        if(gameObject.transform.position.x >= resetPosition){
            Destroy(gameObject);
        }
		
		//Move the platform
		transform.Translate(Vector3.right * (objectSpeed * Time.deltaTime), Space.World);

		//move each cloud to the beginning when it is off screen
		//if (transform.localPosition.x >= resetPosition){
			//Vector3 newPosition = new Vector3(startPosition, transform.position.y,transform.position.z);
			//transform.position = newPosition;
		//}
		
	}
}

    
