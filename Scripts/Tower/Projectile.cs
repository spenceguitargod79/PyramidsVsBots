using UnityEngine;

//declare enum outside of class so it can be referenced from anywhere in the project
public enum proType{
	block,arrow,fireball,rocket
};
public class Projectile : MonoBehaviour {

[SerializeField]
private int attackStrength;
[SerializeField]
private proType projectileType;

//getters (setters not needed because we will set via the inspector)
public int AttackStrength{
	get{
		return attackStrength;
	}
}

public proType ProjectileType{
	get{
		return projectileType;
	}
}

}
