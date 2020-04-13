using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : MonoBehaviour {
	[SerializeField]
	private float speedMultiplier;
	
	[SerializeField]
	private int healthPoints;
	[SerializeField]
	private int rewardAmount;
	[SerializeField]
	private Transform exitPoint;
	[SerializeField]
	private Transform[] waypoints;
	[SerializeField]
	private float navigationUpdate;
	private int target = 0;
	private Transform enemy;
	private float navigationTime = 0;
	private bool isDead = false;
	private bool isAttacking = false;
	private float attackCounter;
	private Animator animator;
	[SerializeField]
	//need gameobject that our projectile can be set to
	private Rocket rocket;
	private Tower targetEnemy = null;//towers will be the robot's target
	private int currentWave;

	//need to keep track of time between attacks
	[SerializeField]
	private float timeBetweenAttacks;
	[SerializeField]
	private float attackRadius;

	public bool IsDead{
		get{
			return isDead;
		}
	}
	private Collider2D enemyCollider;

	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform>();
		//register the enemy, which adds him to a list
		GameManager.Instance.RegisterEnemy(this);
		//get the collider component
		enemyCollider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();

		//speed multiplier should increase with each wave. Get the current wave and set new speed to that number * some amount.
		currentWave = GameManager.Instance.WaveNumber;
		if(currentWave >= 5 && currentWave < 9){
			speedMultiplier = 3f;
		}
		else if (currentWave == 9){//9 is actually wave 10
			speedMultiplier = 3.5f;	
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(waypoints != null && !isDead){
			navigationTime += Time.deltaTime;
			if(navigationTime > navigationUpdate){
				if(target < waypoints.Length){
					enemy.position = Vector2.MoveTowards(enemy.position,waypoints[target].position,navigationTime*speedMultiplier);
				}
				else{
					enemy.position = Vector2.MoveTowards(enemy.position,exitPoint.position,navigationTime*speedMultiplier);
				}
				navigationTime = 0;//reset the timer
			}

		//Attacking logic
		attackCounter -= Time.deltaTime;
		if(targetEnemy == null || targetEnemy.IsDead){
			Tower nearestEnemy = getNearestEnemyInRange();
			//it found an enemy
			if(nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius){
				targetEnemy = nearestEnemy;
			}
		}
		else{
			if(attackCounter <= 0){
				//time between attacks is now zero
				isAttacking = true;
				//reset attack counter
				attackCounter = timeBetweenAttacks;
			}
			else{
				isAttacking = false;
			}
			//call off the attack if the enemy leaves the attack radius
			if(Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius){
				targetEnemy = null;
			}
		}
		}
	}

		//Use fixedupdate to move gameobjects around
/* 	void FixedUpdate(){
		if(isAttacking){
			attack();
			Debug.Log("isAttacking = TRUE");
		}
		else{
			Debug.Log("isAttacking = false");
		}
	} */

	void OnTriggerEnter2D(Collider2D other){
		//if we hit something, increment the checkpoint array
		if(other.tag == "checkpoint"){
			target += 1;// meaning we hit aa checkpoint, so set the next one.
		}
		else if(other.tag == "Finish"){
			GameManager.Instance.UnregisterEnemy(this);
			//keep trak of escaped enemies so we know if the wave is over
			GameManager.Instance.RoundEscaped+=1;
			GameManager.Instance.TotalEscaped+=1;
			GameManager.Instance.isWaveOver();//check status of the game
		}
		else if(other.tag == "projectile"){
			if(other != null){
				Projectile newP = other.gameObject.GetComponent<Projectile>();//now we can use newP to access all the Projectile scripts variables
				if(newP != null){
					enemyHit(newP.AttackStrength);//accesses the projectiles getter, AttackStrength.
				}
				Destroy(other.gameObject);//will destroy any tagged projectiles
			}
		}
		else if(other.tag == "artifact"){
			GameManager.Instance.UnregisterEnemy(this);
			//keep trak of escaped enemies so we know if the wave is over
			GameManager.Instance.RoundEscaped+=1;
			GameManager.Instance.TotalEscaped+=1;
			GameManager.Instance.isWaveOver();//check status of the game
			
			Destroy(gameObject);
			//play explode animation, then destroy
		}
	}

	public void enemyHit(int hitpoints){
		if(healthPoints - hitpoints > 0){
			healthPoints-=hitpoints;
			//hit sound
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
			//hurt animation
			animator.Play("hurt");
			animator.Play("hurt2");
		}
		else{
			//death animation
			animator.SetTrigger("didDie");//we did at trigger so this anim can be called from any state
			//kill enemy
			die();
		}
		
	}

	public void die(){
		isDead = true;
		enemyCollider.enabled = false;//so they dont get targeted after death
		GameManager.Instance.TotalKilled+=1;
		GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
		GameManager.Instance.addMoney(rewardAmount);
		GameManager.Instance.isWaveOver();//check status of the game
	}

	/* public void attack(){
		//reset isAttacking to false
		isAttacking = false;
		//need a rocket projectile
		Rocket newRocket = Instantiate(rocket) as Rocket;
		newRocket.transform.localPosition = transform.localPosition;

		//check if the target enemy exists
		if(targetEnemy == null){
			Destroy(newRocket);
		}
		else{
			//move the projectile to enemy
			StartCoroutine(MoveProjectile(newRocket));
		}
	} */

	//Work in progress----
	IEnumerator MoveProjectile(Rocket rocket){
		if(targetEnemy == null){
			Debug.Log("Robot class:TARGET ENEMY IS NULL!");
		}
		//loop until projectile hits enemy
		while(getTargetDistance(targetEnemy) > 0.20f && rocket != null && targetEnemy != null){
			Debug.Log("MOVING ROCKET!");
			
			//we need to know direction, angle, and which way to make the projectile look
			 var dir = targetEnemy.transform.localPosition - transform.localPosition;
			 var angleDirection = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
			 //transform the position of our projectile
			 rocket.transform.localPosition = Vector2.MoveTowards(rocket.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
			 rocket.transform.rotation = Quaternion.AngleAxis(angleDirection,Vector3.forward);
			 
			 yield return null;
		}
		if(rocket != null || targetEnemy == null){
			//rocket.animator.SetTrigger("didDie");
			Destroy(rocket);
			
		}
	}

		//find a new enemy if the original target doesnt exist anymore
	private float getTargetDistance(Tower thisEnemy){
		if(thisEnemy == null){
			thisEnemy = getNearestEnemyInRange();
			if(thisEnemy == null){
				return 0f;
			}
		}
		return Mathf.Abs(Vector2.Distance(transform.localPosition,thisEnemy.transform.localPosition));
	}

	private List<Tower> GetEnemiesInRange(){
		List<Tower> enemiesInRange = new List<Tower>();
		//Cycle through registered enemies to see if they are in our attack radius
		foreach(Tower enemy in TowerManager.Instance.TowerList){
			if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius){
				enemiesInRange.Add(enemy);
			}
		}
		return enemiesInRange;
	}

	private Tower getNearestEnemyInRange(){
		Tower nearestEnemy = null;
		float smallestDistance = float.PositiveInfinity;// so anything compared to it will be smaller
		foreach(Tower enemy in GetEnemiesInRange()){
			if(Vector2.Distance(transform.localPosition,enemy.transform.localPosition) < smallestDistance){
				smallestDistance = Vector2.Distance(transform.localPosition,enemy.transform.localPosition);
				nearestEnemy = enemy;
			}
		}
		return nearestEnemy;
	}

	public float getSpeedMultiplier(){
		return speedMultiplier;
	}

	public void setSpeedMultiplier(float s){
		speedMultiplier = s;
	}
}
