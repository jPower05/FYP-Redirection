using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 	Author James Power 20067779
 * 
 * 	Script on player character that checks for a mirror collision
 * 	On collision checks for key press
 * 	Rotates mirror objects based on the key pressed
*/

public class CollisionCheck : MonoBehaviour {

	bool collided = false;

	public GameObject otherObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//return if no collision
		if (collided == false) return;

		//if there has been a collision
		if (collided){
			//check for key press X (rotate the y value of entire MirrorGameObject)
			if (Input.GetKey(KeyCode.H)){

				otherObj.transform.Rotate(0.0f, 1.0f, 0.0f * Time.deltaTime);

				Vector3 eulerAngles = otherObj.transform.rotation.eulerAngles;

			}	
			//rotate the mirror part
			if (Input.GetKey(KeyCode.J)){
	 			//rotate1();
	 			
			}	
			//rotate the mirror part
			if (Input.GetKey(KeyCode.B)){
	 			GameObject rotate;   
	 			rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
      
	 			rotate.transform.Rotate(0.0f, -1.0f, 0.0f * Time.deltaTime);
	 			
			}	
			//rotate the mirror part
			if (Input.GetKey(KeyCode.N)){

	 			GameObject rotate;   
	 			rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
      
	 			rotate.transform.Rotate(0.0f, 0.0f, 10.0f * Time.deltaTime);
	 			
			}	
			//rotate the mirror part
			if (Input.GetKey(KeyCode.M)){
				
	 			GameObject rotate;   
	 			rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
      
	 			rotate.transform.Rotate(0.0f, 0.0f, -10.0f * Time.deltaTime);
	 			
			}	
		}
		
	}

	void OnTriggerEnter(Collider collider) {

		//Get collided game object
        otherObj = collider.gameObject;
        //Check for mirror tag
        if(otherObj.tag == "MirrorGameObject")        
     	{
     		//Test for collision
        	//set bool collided to true so update will check for key press
        	collided = true;

    	}
    }	

    void onTriggerExit(){
    	collided = false;
    }
}
