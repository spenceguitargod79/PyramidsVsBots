using UnityEngine;
using System.Collections;

public class smokeController : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.Play("smokeanim");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
