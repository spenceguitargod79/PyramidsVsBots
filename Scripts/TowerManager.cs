using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class TowerManager : Singleton<TowerManager> {

	public TowerButton towerBtnPressed {get;set;}
	private SpriteRenderer spriteRenderer;
	public List<Tower> TowerList = new List<Tower>();
	private List<Collider2D> BuildList = new List<Collider2D>();
	private Collider2D buildTile;
	private int selectedTowerCost;

	//Add-on build site variables
	[SerializeField]
	private int addOnBuildSiteCost = 22;
	[SerializeField]
	private int maxBuildSites = 1;
	private int totalBuildSites;
	[SerializeField]
	private Button addOnSiteBtn;
	[SerializeField]
	private List<GameObject> grassPatches = new List<GameObject>();
	[SerializeField]
	private GameObject buildsiteSprite;
	public List<GameObject> addOnSitesList = new List<GameObject>();
	private SpriteRenderer addOnSiteRend;//need this to change sorting order after instantiated
	[SerializeField]
	private GameObject boomParticles;
	private int maxAddOnBuildSites = 2;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();//grabs a reference
		buildTile = GetComponent<Collider2D>();
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//get user input - if user clicks left mouse btn
		//if(Input.GetMouseButtonDown(0)){
		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Stationary) {
				Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				//create a raycast
				RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);
				if (hit.collider.tag == "buildsite") {
					if (GameManager.Instance.TotalMoney >= selectedTowerCost) {//only place tower if user has money
						Debug.Log ("Last selected tower cost is: " + getSelectedTowerCost());
						buildTile = hit.collider;
						placeTower (hit);
						//buildTile.tag = "buildsitefull";//rename the tag so another tower cant be placed on top of the other
						RegisterBuildSite (buildTile);

					}
					else{
						Debug.Log ("You dont have enough cash for that!");
						//GameManager.Instance.TotalMoney = 0;
					}
					Debug.Log ("Select tower cost = " + getSelectedTowerCost ());
				}
		    }
		}
		if(spriteRenderer.enabled){
			followMouse();
		}

		if ((TowerList.Count >= maxBuildSites) && (GameManager.Instance.TotalMoney >= addOnBuildSiteCost) && (addOnSitesList.Count < maxAddOnBuildSites) && (GameManager.Instance.getCurrentGameState () != gameStatus.win) && (GameManager.Instance.getCurrentGameState () != gameStatus.gameover)){
			showAddOnSiteButton ();
		}
		else{
			hideAddOnSiteButton ();
		}
		//Debug.Log ("Total cash = " + GameManager.Instance.TotalMoney); 
	}

	public void RegisterBuildSite(Collider2D buildtag){
		BuildList.Add(buildtag);
	}

	public void RegisterTower(Tower tower){
		TowerList.Add(tower);
	}
	public void RenameTagsBuildSites(){
		foreach(Collider2D buildtag in BuildList){
			if(buildtag != null){
				buildtag.tag = "buildsite";
			}
		}
		BuildList.Clear();
	}

	public void DestroyAllTowers(){
		foreach(Tower tower in TowerList){
			Destroy(tower.gameObject);
		}
		TowerList.Clear();
	}


	public void placeTower(RaycastHit2D hit){
		if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null){
				buildTile.tag = "buildsitefull";//rename the tag so another tower cant be placed on top of the other
				Tower newTower = Instantiate(towerBtnPressed.TowerObject);
				newTower.transform.position = hit.transform.position;
				buyTower(towerBtnPressed.TowerPrice);
				GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
				RegisterTower(newTower);
				disableDragSprite();

			if ((TowerList.Count >= maxBuildSites) && (GameManager.Instance.TotalMoney >= addOnBuildSiteCost) && (addOnSitesList.Count < maxAddOnBuildSites) && (GameManager.Instance.getCurrentGameState () != gameStatus.win) && (GameManager.Instance.getCurrentGameState () != gameStatus.gameover)) {
					showAddOnSiteButton ();
				}
				else{
					hideAddOnSiteButton ();
				}
		}
	}

	public void showAddOnSiteButton(){
		//show add-on build site button if requirements are met
		//Debug.Log ("towerlist count = " + TowerList.Count);
		//enable button
		addOnSiteBtn.gameObject.SetActive(true);
		Debug.Log ("Enabling add on build site button");
		//TODO:play smoke animation at button location
	}

	public void hideAddOnSiteButton(){
		//hide button
		Debug.Log ("Disabling add on build site button");
		addOnSiteBtn.gameObject.SetActive(false);
		//TODO:play smoke animation at button location
	}

	public void buyTower(int price){
		GameManager.Instance.subtractMoney(price);
	}

	public void followMouse(){
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);	}

	public void enableDragSprite(Sprite sprite){
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void disableDragSprite(){
		spriteRenderer.enabled = false;	
	}

	public void selectedTower(TowerButton towerSelected){
		Debug.Log ("totalMoney = " + GameManager.Instance.TotalMoney);
		//if(towerSelected.TowerPrice <= GameManager.Instance.TotalMoney){
		if(GameManager.Instance.TotalMoney >= towerSelected.TowerPrice){
			towerBtnPressed = towerSelected;
			//Debug.Log("Pressed " + towerBtnPressed );
			enableDragSprite(towerBtnPressed.DragSprite);
		}
	}

	public void setSelectedTowerCost(TowerButton towerSelected){
		selectedTowerCost = towerSelected.TowerPrice;
	}

	public int getSelectedTowerCost(){
		return selectedTowerCost;
	}

	public void addBuildSiteButtonPressed(){
		Debug.Log ("Add-On Build Site Button was pressed!");
		//choose a random grass patch from 'grassPatches' list
		if(grassPatches.Count != 0){
			int index = Random.Range (0, grassPatches.Count);
			Debug.Log ("Random grasspatch index chosen = " + index);
			//get position of chosen grass patch
			Vector2 pos = grassPatches[index].transform.position;
			Debug.Log ("Grasspatch position: " + pos);
			//destroy the grasspatch
			Debug.Log ("Destroying grass patch at index = " + index);
			//Destroy (grassPatches[index]);
			//remove patch from list
			Debug.Log ("Deleting grass patch from List = " + index);
			grassPatches.RemoveAt (index);

			//Instantiate build site tile at the grasspatches position
			GameObject newSite = Instantiate(buildsiteSprite, new Vector2(pos.x, pos.y), Quaternion.identity) as GameObject;
			//Get the gameobject's sprite renderer and change its sorting order so it's above the grass patch.
			addOnSiteRend = newSite.GetComponent <SpriteRenderer>();
			addOnSiteRend.sortingLayerName = "buildground";
			addOnSiteRend.sortingOrder = 2;

			//set the tag
			newSite.tag = "buildsite";

			//Need a new list to hold newly created build sites, so we can delete them when game is restarted
			addOnSitesList.Add (newSite);
			Debug.Log ("Adding to addonsitesList, total list size = " + addOnSitesList.Count);

			instantiateBoomParticles (pos.x,pos.y);

			//TODO somehwere else: when new game is started, put grass tiles back in place of the add-on build sites.
			//might be able to comment out the deletion of these, and they will just lie underneath the buildsites.
		}
		else{
			Debug.Log ("Grass patch List is empty!");
		}
	}

	public void destroyAddOnBuildSites(){
		//TODO:destroy the gameobjects in the list, which should remove them from teh screen
		foreach(GameObject bs in addOnSitesList){
			Destroy(bs);
		}

		//clear the list
		addOnSitesList.Clear();
	}

	public void instantiateBoomParticles(float x, float y){
		GameObject particles = Instantiate(boomParticles, new Vector2(x, y), Quaternion.identity) as GameObject;
		GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
		//subtract $$ from players bank
		buyTower (addOnBuildSiteCost);//not actually buying a tower in this case, just an add-on build site


	}

	//These get destroyed, so re-add them when the game is over, either a win or loss state.
	public void resetAddOnSiteGrassPatches(){
		//make sure its empty
		if (grassPatches.Count == 0) {
			grassPatches.Add (GameObject.Find("Map1/ground/GrassGround (73)"));
			grassPatches.Add (GameObject.Find("Map1/ground/GrassGround (241)"));
			//grassPatches.Add (GameObject.Find("Map1/ground/GrassGround (227)"));
		}
	}
}
