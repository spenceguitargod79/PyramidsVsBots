using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject cloud;
    [SerializeField]
    private float topLimitY;//highest point a cloud can spawn
    [SerializeField]
    private float bottomLimitY;//lowest point a cloud can spawn
    private float spawnPointX;//This is where the cloud will spawn
    [SerializeField]
    private float spawnInterval;//time between spawing a cloud
    private GameObject newCloud;
    // Start is called before the first frame update
    
    void Start()
    {
        newCloud = Instantiate(cloud, transform.position, Quaternion.identity) as GameObject;//cast as a gameobject
    }

    // Update is called once per frame
    void Update()
    {
    
        
        //StartCoroutine("spawn");
        if(newCloud!=null){
            //newCloud.transform.position = new Vector2(spawnPointX, newCloud.transform.position.y);
            newCloud.transform.position.Set(newCloud.transform.position.x, newCloud.transform.position.y + 2, newCloud.transform.position.z);
        }
        

    }

    	//IEnumerator spawn(){

            //GameObject newCloud = Instantiate(cloud, transform.position, Quaternion.identity) as GameObject;//cast as a gameobject
			
			//yield return new WaitForSeconds(spawnInterval);
			
		//}
}

