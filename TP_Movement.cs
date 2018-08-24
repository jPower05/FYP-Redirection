using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * Author James Power 20067779
 * 
 * Script attaches to IrokNewMovementNetworked prefab - disabled by default and the local players version is enabled through SetLocalPlayer script
 * 
 * 	Credit: This class was adapted and changed from 3D Buzz Creating a Custom 3rd Person Character and Camera System C#
 * 	https://www.3dbuzz.com/training/view/3rd-person-character-system
 * 
*/

public class TP_Movement : MonoBehaviour {

	public float moveSpeed = 10f;	//the speed the chararacter moves at. Editable in inspector window
	public float gravity = 21f;
	public float terminalVelocity = 20f;
	public float jumpSpeed = 6f;

	public Vector3 moveVector { get; set; }	//the vector which controls the movement. 
	public float verticalVelocity { get; set;}	

	CharacterController controller;	//the character controller attached the the character


	void Awake(){

		//get the character controller component
		controller = GetComponent ("CharacterController") as CharacterController;	//cast the return value of getComponent to type CharacterController
		//Debug.Log("Got the character controller");

		TP_Camera.UseExistingOrCreateNewMainCamera ();	//call method from TP_Camera

	}
		
	void Update () {

		if (Camera.main == null)
			return;


		getLocomotionInput ();
		handleActionInput ();		//Jump
		processMotion();
		//Debug.Log("Processing the movement");
		snapAllignCharacterWithCamera ();

	}

	void getLocomotionInput(){
		//declare a dead zone for more precise movement
		var deadZone = 0.1f;
		//acceleration
		verticalVelocity = moveVector.y;

		moveVector = Vector3.zero;	//stops motion adding on each other
		//zero the moveVector value as it is re calculated each frame


		//move forward/backwards only takes place on the z axis
		//Vertical checks for the w and s keys
		if ((Input.GetAxis ("Vertical") > deadZone) || (Input.GetAxis ("Vertical") < -deadZone)) {
			moveVector += new Vector3 (0, 0, Input.GetAxis ("Vertical"));
		}

		//move left/right only takes place on the x axis
		//Horizontal checks for the w and s keys
		if ((Input.GetAxis ("Horizontal") > deadZone) || (Input.GetAxis ("Horizontal") < -deadZone)) {
			
			moveVector += new Vector3 (Input.GetAxis ("Horizontal"),0, 0 );
		}
	}


	void processMotion(){

		//Transform MoveVector to WorldSpace
		moveVector = transform.TransformDirection(moveVector);

		//Normalize MoveVector if Magnitude > 1
		if(moveVector.magnitude > 1){
			moveVector = Vector3.Normalize (moveVector);	//assign moveVector the value of the normalized moveVecotr
		}

		//Multiply MoveVector by MoveSpeed
		moveVector *= moveSpeed;

		//Multiply MoveVector by Delta Time

		//reapply vertical velocity to the y of moveVector
		moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);

		applyGravity ();

		//Move the Irok in World Space
		controller.Move(moveVector * Time.deltaTime);		//BEING CALLED ON TURNED OFF CHARACTER CON


	}

	//handle press key for jump
	void handleActionInput(){

		if (Input.GetButton ("Jump"))
			jump ();
	}

	void jump(){
		if (controller.isGrounded)
			verticalVelocity = jumpSpeed;
	}


	void applyGravity(){
		//check terminal velocity
		if (moveVector.y > -terminalVelocity) {
			//apply gravity on y axis (downward vector)
			moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);
		}
		if (controller.isGrounded && moveVector.y < -1)
			moveVector = new Vector3(moveVector.x, -1, moveVector.z);

	}

	void snapAllignCharacterWithCamera(){

		//only snap camera to player if moving

		if (moveVector.x != 0 || moveVector.z != 0) {
			transform.rotation = Quaternion.Euler (transform.eulerAngles.x,
				Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}
}
