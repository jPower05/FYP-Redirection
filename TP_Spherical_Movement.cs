using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	Author James Power 20067779 
 * 
 * 	Attached to the Irok prefab but initially disabled
 * 	Movement for the Irok on the spherical world section.
 * 
 * 	Credit: Adapted and altered from the Pushy Pixels Cooking with Unity Mario Galaxy playlist on Youtube.  
 * 	https://www.youtube.com/playlist?list=PLlHjNcdoyw6UK30xrTUhjM-usQOOE5jhN
*/

public class TP_Spherical_Movement : MonoBehaviour {
	public float playerMovementSpeed = 1.0f;
	public Transform gravitySource;	//the sphere which the player will have gravity acted on it
	public float gravity = 9.81f;
	Rigidbody rigidbody;
	private Vector3 gravityVector;
	private bool grounded = false;
	public float jumpValue = 10.0f;
	public Vector3 velocity = Vector3.zero;
	public float jumpOffset = 1.0f;
	public float gravityRotationRate = 30.0f;
	public bool insidePlanet = false;
	private Camera mainCam;

	void Awake() {
		rigidbody = GetComponent<Rigidbody> ();
		mainCam = Camera.main;
	}

	void Update(){
		//w/s movement
		Vector3 cameraMovementDirection = mainCam.transform.forward * Input.GetAxis ("Vertical") + mainCam.transform.right * Input.GetAxis ("Horizontal");
		gravityVector = (gravitySource.position - transform.position).normalized * gravity *Time.deltaTime;

		if (insidePlanet) {
			StayOnSphere ();
		}
		Vector3 rightwardVector = Vector3.Cross (cameraMovementDirection, gravityVector);
		Vector3 playerForwardDirection = Vector3.Cross(gravityVector, rightwardVector).normalized;
		//transform.position += playerForwardDirection *playerMovementSpeed * Time.deltaTime;
		transform.position = Vector3.Lerp (transform.position, transform.position + playerForwardDirection, playerMovementSpeed * Time.deltaTime);
		//jumping
		if (Input.GetButton ("Jump") && grounded) {
			
			Vector3 jumpDirection = (transform.position - gravitySource.position).normalized;
			velocity += jumpDirection * jumpValue;
			grounded = false;
		}
		velocity += gravityVector;
		Quaternion rot;
		if(cameraMovementDirection.sqrMagnitude > 0.01f){
			rot = Quaternion.LookRotation(playerForwardDirection, -gravityVector);
		}
		else{
			rot = Quaternion.LookRotation(transform.forward, -gravityVector);
		}
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, gravityRotationRate * Time.deltaTime);
		transform.position += velocity * Time.deltaTime;

	}

	void OnCollisionStay(Collision collision){
		print ("help");
	}
		
	void OnTriggerStay(Collider col){
		if (col.tag == "Planet") {
			insidePlanet = true;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Planet") {
			insidePlanet = true;
		}
		if (col.tag == "GravityWell") {
			gravitySource = col.transform.parent;
		}
			
	}

	void StayOnSphere(){
		Collider col = gravitySource.GetComponent<Collider>();
		grounded = true;	//on the ground. jump enabled
		velocity = Vector3.zero;	//reset velocity when on sphere
		//transform.position = gravitySource.position - gravityVector.normalized * (col.transform.localScale.y *0.5f);
		transform.position = Vector3.Lerp (transform.position, gravitySource.position - gravityVector.normalized * (col.transform.localScale.y * 0.5f), gravity * Time.deltaTime);
		insidePlanet = false;
	}
}
