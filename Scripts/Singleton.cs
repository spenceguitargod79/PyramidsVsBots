using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour{

public static T instance;//singleton
//a getter for the instance
public static T Instance {
	get{
		//check if instance of gamemanager is null
		if(instance == null){
			instance = FindObjectOfType<T>();
		}
		else if (instance != FindObjectOfType<T>()){
			Destroy(FindObjectOfType<T>());
		}

		DontDestroyOnLoad(FindObjectOfType<T>());

		return instance;
	}
}

}
