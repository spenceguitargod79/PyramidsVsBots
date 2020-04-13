using UnityEngine;
using System.Collections;
using System.Collections.Generic;//allows use of lists
using UnityEngine.UI;

//public enumerator for when to update the UI
public enum gameStatus{
	next,play,gameover,win
};


public class GameManager : Singleton<GameManager> {
	//variables fr all the UI text labels
	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text totalEscapedLbl;
	[SerializeField]
	private Text totalMoneyLbl;
	[SerializeField]
	private Text currentWaveLbl;
	[SerializeField]
	private Text playButtonLbl;
	[SerializeField]
	private Button playBtn;
	[SerializeField]
	private GameObject winBanner;
	[SerializeField]
	private Text winBannerText;
	[SerializeField]
	private GameObject loseBanner;
	[SerializeField]
	private Text loseBannertext;

	//variables for tracking UI data values
	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalKilled = 0;
	private int whichEnemyToSpawn = 0;
	private gameStatus currentState = gameStatus.play;
	private AudioSource audioSource;
	private int enemiesToSpawn = 0;//the index of what enemy to spawn

	public Animator animator;

	private bool spawnBirds = false;
	//getters/setters
	public bool SpawnBirds{
		get{
			return spawnBirds;
		}
		set{
			spawnBirds = value;
			//update Ui label
			//totalMoneyLbl.text = totalMoney.ToString();
		}
	}

	
	public int WaveNumber{
		get{
			return waveNumber;
		}
	}
		
	public int TotalMoney{
		get{
			return totalMoney;
		}
		set{
			totalMoney = value;
			//update Ui label
			totalMoneyLbl.text = totalMoney.ToString();
		}
	}

	public int TotalEscaped{
		get{
			return totalEscaped;
		}
		set{
			totalEscaped = value;
		}
	}

	public int RoundEscaped{
		get{
			return roundEscaped;
		}
		set{
			roundEscaped = value;
		}
	}

	public int TotalKilled{
		get{
			return totalKilled;
		}
		set{
			totalKilled = value;
		}
	}

	public AudioSource AudioSource{
		get{
			return audioSource;
		}
	}

	//-------------------------------

	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private Robot[] enemies;
	[SerializeField]
	private int totalEnemies = 3;
	[SerializeField]
	private int enemiesPerSpawn;
	
	public List<Robot> EnemyList = new List<Robot>();
	const float spawnDelay = 1.0f;

	// Use this for initialization
	void Start () {
		//StartCoroutine(spawn());
		animator = GetComponent<Animator>();
		playBtn.gameObject.SetActive(false);//hide the next wave button
		audioSource = GetComponent<AudioSource>();
		showMenu();
	}

	void Update(){
		handleEscape();
		//Debug.Log ("Total enemies up in here: " + EnemyList.Count);
	}
	

	//coroutine for spawning enemies
	IEnumerator spawn(){
		if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies){
			for(int i = 0; i < enemiesPerSpawn; i++){
				if(EnemyList.Count < totalEnemies){
					Debug.Log("Spawning enemy");
					//create an enemy
					Robot newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Robot;//cast as a gameobject
					newEnemy.transform.position = spawnPoint.transform.position;
					//start moving the birds
					SpawnBirds = true;
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine(spawn());
		}
	}

	public void RegisterEnemy(Robot enemy){
			EnemyList.Add(enemy);
	}
	public void UnregisterEnemy(Robot enemy){
			EnemyList.Remove(enemy);
			AudioSource.PlayOneShot(SoundManager.Instance.EnemyEscapedSound);
			Debug.Log ("playing sound...");
			Destroy(enemy.gameObject);
	}
	public void addMoney(int amt){
		TotalMoney+=amt;
	}

	public void subtractMoney(int amt){
		TotalMoney-=amt;
	}

	public void isWaveOver(){
		totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
		if((roundEscaped+totalKilled) == totalEnemies){
			//wave is over
			if(waveNumber <= enemies.Length){
				enemiesToSpawn = waveNumber;
				SpawnBirds = false;
			}
			setCurrentGameState();
			showMenu();
		}
	}

	public void setCurrentGameState(){
		waveNumber += 1;
		Debug.Log("Current Wave number = " + waveNumber);
		if(TotalEscaped >= 10){
				currentState = gameStatus.gameover;
		}
		else if(waveNumber == 0 && (totalKilled + roundEscaped) == 0){
			currentState = gameStatus.play;
			
		}
		else if(waveNumber >= totalWaves){
			currentState = gameStatus.win;

			//TODO:stop the rest of the robots from moving and then destroy them one by one.
				//foreach(Robot enemy in EnemyList){
				//	DestroyAllEnemies();
				//}

				for(int i = 0; i < EnemyList.Count; i++){
					DestroyAllEnemies();
				}
		}
		else{
			currentState = gameStatus.next;
		}
	}

	public gameStatus getCurrentGameState(){
		return currentState;
	}

	void showMenu(){
		switch(currentState){
			case gameStatus.gameover:
				TowerManager.Instance.hideAddOnSiteButton ();
				TowerManager.Instance.destroyAddOnBuildSites ();
				TowerManager.Instance.resetAddOnSiteGrassPatches ();
				playButtonLbl.text = "Play Again?";
				int wn = waveNumber;//temp variable to hold value before its reset
					//reset waves and update gui
				waveNumber = 0;
				currentWaveLbl.text = "Wave " + (waveNumber + 1);
					//game over sound
				AudioSource.PlayOneShot (SoundManager.Instance.GameOver);
				loseBanner.gameObject.SetActive (true);

				if (wn < 5) {
					loseBannertext.text = "GAME OVER";
				} else if (wn < 5 && wn < 7) {
					loseBannertext.text = "Not Bad!";
				} else {
					loseBannertext.text = "So Close!";
				}
				wn = 0;
				destroyAllProjectiles();
				
				break;
			case gameStatus.next:
				playButtonLbl.text = "Next Wave";
				break;
			case gameStatus.play:
				playButtonLbl.text = "Play Game";
				break;
			case gameStatus.win:
				TowerManager.Instance.hideAddOnSiteButton ();
				TowerManager.Instance.destroyAddOnBuildSites ();
				TowerManager.Instance.resetAddOnSiteGrassPatches ();
				playButtonLbl.text = "Play Again?";
				//reset waves and update gui
				waveNumber = 0;
				currentWaveLbl.text = "Wave " + (waveNumber + 1);
				//enable win banner with text
				winBanner.gameObject.SetActive(true);
				winBannerText.text = "WINNER!";
				AudioSource.PlayOneShot(SoundManager.Instance.Winner);
				destroyAllProjectiles();
				break;
		}
		playBtn.gameObject.SetActive(true);
	}

	public void playButtonPressed(){
		//Debug.Log("You pushed play");
		switch(currentState){
			case gameStatus.next:
				//waveNumber+=1;
				totalEnemies+=waveNumber;
				//reset wave number if starting over and update gui
				if(waveNumber >= totalWaves){
					waveNumber = 0;
					currentWaveLbl.text = "Wave " + (waveNumber + 1);
				}
				AudioSource.PlayOneShot(SoundManager.Instance.Death);
				destroyAllProjectiles();
				break;
			default:
				totalEnemies = 3;
				TotalEscaped = 0;
				TotalMoney = 20;
				enemiesToSpawn = 0;//reset this 
				TowerManager.Instance.DestroyAllTowers();
				TowerManager.Instance.RenameTagsBuildSites();
				totalMoneyLbl.text = TotalMoney.ToString();
				totalEscapedLbl.text = "Escaped " + TotalEscaped.ToString() + "/10";
				//destroy all game objects on screen
				audioSource.PlayOneShot(SoundManager.Instance.NewGame);
				break;
		}
		DestroyAllEnemies();
		TotalKilled = 0;
		RoundEscaped = 0;
		currentWaveLbl.text = "Wave " + (waveNumber + 1);
		StartCoroutine(spawn());
		playBtn.gameObject.SetActive(false);
		winBanner.gameObject.SetActive(false);
		loseBanner.gameObject.SetActive(false);
	}

	//change this to work with touch screen
	private void handleEscape(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			TowerManager.Instance.disableDragSprite();
			TowerManager.Instance.towerBtnPressed = null;
		}
	}

	public void DestroyAllEnemies(){
		foreach(Robot enemy in EnemyList){
			if(enemy != null){
				Destroy(enemy.gameObject);
			}
		}
		EnemyList.Clear();
	}
	public void destroyAllProjectiles(){
		//go through projectiles by tag and delete
		GameObject[] proj = GameObject.FindGameObjectsWithTag("projectile");
		foreach(GameObject p in proj){
			GameObject.Destroy(p);
		}
	}
}
