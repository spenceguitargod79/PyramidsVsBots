using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartScreenManager : Singleton<StartScreenManager> {

	[SerializeField]
	private Transform arrivalPoint;
	private float navigationTime = 0;
	[SerializeField]
	private float navigationUpdate;
	private Camera camera;
	//private Transform cameraLocation;
	[SerializeField]
	private GameObject startBtn;
	[SerializeField]
	private float speedMultiplier;
	private AudioSource audioSource;

	public AudioSource AudioSource{
		get{
			return audioSource;
		}
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		camera = Camera.main;
		//disable the start button
		startBtn.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		//move camera to destination
		navigationTime += Time.deltaTime;

		//camera.transform.position = Vector3.MoveTowards(camera.transform.position,arrivalPoint.position,navigationTime*speedMultiplier);
	
	}

	void OnCollisionEnter(Collision other){
		//if we hit something, increment the checkpoint array
		if (other.gameObject.tag == "Finish") {
			//target += 1;// meaning we hit aa checkpoint, so set the next one.
			Debug.Log ("Camera has reached the target");
			//Enable the start button
			startBtn.SetActive(true);
			AudioSource.PlayOneShot(SoundManager.Instance.Death);
		}
		else{
			Debug.Log ("Camera hasNOT reached the target");
		}
	}

	//This gets called when the start button is clicked
	public void loadLevel(){
		//load level 1
		SceneManager.LoadScene ("level1");
	}


}
