using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

	//need to keep track of time between attacks
	[SerializeField]
	private float timeBetweenAttacks;
	[SerializeField]
	private float attackRadius;

	[SerializeField]
	//need gameobject that our projectile can be set to
	private Projectile projectile;
	private Robot targetEnemy = null;
	private float attackCounter; 
	private bool isAttacking = false;
	private bool isDead = false;
	[SerializeField]
	private int healthPoints;
	private Collider2D enemyCollider;

	public int HealthPoints{
		get{
			return healthPoints;
		}
	}

	public bool IsDead{
		get{
			return isDead;
		}
	}

	// Use this for initialization
	void Start () {
		enemyCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		attackCounter -= Time.deltaTime;
		if(targetEnemy == null || targetEnemy.IsDead){
			Robot nearestEnemy = getNearestEnemyInRange();
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

	//Use fixedupdate to move gameobjects around
	void FixedUpdate(){
		if(isAttacking){
			attack();
		}
	}

	public void attack(){
		//reset isAttacking to false
		isAttacking = false;
		//need a projectile
		Projectile newProjectile = Instantiate(projectile) as Projectile;
		newProjectile.transform.localPosition = transform.localPosition;

		//playing sounds
		if(newProjectile.ProjectileType == proType.arrow){
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
		}
		else if(newProjectile.ProjectileType == proType.fireball){
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.FireBall);
		}
		else if(newProjectile.ProjectileType == proType.block){
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Block);
		}

		//check if the target enemy exists
		if(targetEnemy == null){
			Destroy(newProjectile);
		}
		else{
			//move the projectile to enemy
			StartCoroutine(MoveProjectile(newProjectile));
		}
	}

	IEnumerator MoveProjectile(Projectile projectile){
		//loop until projectile hits enemy
		while(getTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null){
			//we need to know direction, angle, and which way to make the projectile look
			 var dir = targetEnemy.transform.localPosition - transform.localPosition;
			 var angleDirection = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
			 //transform the position of our projectile
			 projectile.transform.rotation = Quaternion.AngleAxis(angleDirection,Vector3.forward);
			 projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
			 yield return null;
		}
		if(projectile != null || targetEnemy == null){
			Destroy(projectile);
		}
	}

	//find a new enemy if the original target doesnt exist anymore
	private float getTargetDistance(Robot thisEnemy){
		if(thisEnemy == null){
			thisEnemy = getNearestEnemyInRange();
			if(thisEnemy == null){
				return 0f;
			}
		}
		return Mathf.Abs(Vector2.Distance(transform.localPosition,thisEnemy.transform.localPosition));
	}

	private List<Robot> GetEnemiesInRange(){
		List<Robot> enemiesInRange = new List<Robot>();
		//Cycle thrugh registered enemies to see if they are in our attack radius
		if(GameManager.Instance.EnemyList.Count > 0){
			foreach(Robot enemy in GameManager.Instance.EnemyList){
				if(enemy != null){
					if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius){
						enemiesInRange.Add(enemy);
					}
				}
			}
		}
		return enemiesInRange;
	}

	private Robot getNearestEnemyInRange(){
		Robot nearestEnemy = null;
		float smallestDistance = float.PositiveInfinity;// so anything compared to it will be smaller
		foreach(Robot enemy in GetEnemiesInRange()){
			if(Vector2.Distance(transform.localPosition,enemy.transform.localPosition) < smallestDistance){
				smallestDistance = Vector2.Distance(transform.localPosition,enemy.transform.localPosition);
				nearestEnemy = enemy;
			}
		}
		return nearestEnemy;
	}


	public void enemyHit(int hitpoints){
		if(healthPoints - hitpoints > 0){
			healthPoints-=hitpoints;
			//hit sound
			GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
			//hurt animation
			
		}
		else{
			//death animation
			//animator.SetTrigger("didDie");//we did at trigger so this anim can be called from any state
			//kill enemy
			die();
		}
		
	}

	public void die(){
		isDead = true;
		//enemyCollider.enabled = false;//so they dont get targeted after death
		//GameManager.Instance.TotalKilled+=1;
		GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
		//GameManager.Instance.addMoney(rewardAmount);
		//GameManager.Instance.isWaveOver();//check status of the game
	}

	void OnTriggerEnter2D(Collider2D other){
/* 		if(other.gameObject.tag == "rocket"){
			Projectile newP = other.gameObject.GetComponent<Projectile>();//now we can use newP to access all the Projectile scripts variables
			enemyHit(newP.AttackStrength);//accesses the projectiles getter, AttackStrength.
			GameManager.Instance.animator.SetTrigger("didDie");
			Destroy(other.gameObject);//will destroy any tagged projectiles
			Debug.Log("a rocket hit the pyramid!");
			
		} */
	}
}
