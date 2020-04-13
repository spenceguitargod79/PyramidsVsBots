using UnityEngine;
using System.Collections;

public class Rocket : Projectile {

	public Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.tag == "tower"){
			Debug.Log("A rocket hit a tower: detected in Rocket class");
			animator.SetTrigger("didDie");
			Destroy(gameObject);
		}
	}
}
