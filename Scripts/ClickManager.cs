using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    public float thrust = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void FixedUpdate()
    {

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			if (hit.collider != null) {
                Debug.Log("You clicked " + hit.collider.gameObject.name);
                if(hit.collider.gameObject.name == "bird"){
                    GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.BirdSquack);
                    hit.collider.attachedRigidbody.AddForce(Vector2.down * 200.0f);
                    GameManager.Instance.addMoney(1);
                }
                else if(hit.collider.gameObject.name == "bird (1)"){
                    GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.BirdSquack);
                    hit.collider.attachedRigidbody.AddForce(Vector2.down *200.0f);
                    GameManager.Instance.addMoney(1);
                }
                else if(hit.collider.gameObject.name == "bird (2)"){
                    GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.BirdSquack);
                    hit.collider.attachedRigidbody.AddForce(Vector2.up * 200.0f);
                    GameManager.Instance.addMoney(1);
                }
            }
		}
    }
}
