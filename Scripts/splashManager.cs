using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class splashManager : MonoBehaviour {
	[SerializeField]
	int delay;
	[SerializeField]
	int delay2;

	[SerializeField]
	private Text companyName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(fadeOutAndNextScene());
	}

	//coroutine for spawning enemies
	IEnumerator fadeOutAndNextScene(){
		
		yield return new WaitForSeconds(delay);

		//fade out the text - alpha, 255 is highest 0 is non visable
		companyName.CrossFadeAlpha(0.0f,2.0f,false);


		yield return new WaitForSeconds(delay2);


		//move to next scene
		loadLevel ();

	}

	public void loadLevel(){
		//load level 1
		SceneManager.LoadScene ("StartScreen");
	}
}
