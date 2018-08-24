using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MK.Glow;

/*
 * Author James Power 20067779
 * 
 * Occluding camera for the player when flat (TP_Movement) is enabled
 * 
 * Credit: This class was adapted and changed from 3D Buzz Creating a Custom 3rd Person Character and Camera System C#
 * 	https://www.3dbuzz.com/training/view/3rd-person-character-system
 * 
 * 
*/

public class TP_Camera : MonoBehaviour {

	public static TP_Camera Instance;

	public Transform TargetLookAt;

	public float distance = 5f;
	public float min_distance = 3f;
	public float max_distance = 10f;
	public float distanceSmooth = 0.05f;
	public float distanceResumeSmooth = 1f;	//smoothing when snapping camera back to position before occlusion
	private float distanceSmoothed = 0f;
	private float preOcclusionDistance = 0f;

	public float x_MouseSensitivity = 5f;
	public float y_MouseSensitivity = 5f;
	public float mouseWheelMouseSensitivity = 5f;
	public float y_MinLimit = -40f;
	public float y_MaxLimit =  80f;

	public float x_Smooth = 0.05f;
	public float y_Smooth = 0.1f;
	public float occlusionDistanceStep = 0.5f;
	public int maxOcclusionChecks = 10;

	private float mouseX = 0f;
	private float mouseY = 0f;
	private float vel_X = 0f;
	private float vel_Y= 0f;
	private float vel_Z = 0f;
	private float startDistance = 0f;
	private float desiredDistance = 0f;
	private Vector3 position = Vector3.zero;
	private Vector3 desiredPosition = Vector3.zero;
	private float velDistance = 0f;


	void Awake(){
		Instance = this;
	}

	public static void UseExistingOrCreateNewMainCamera(){
		GameObject tempCamera;
		GameObject targetLookAt;
		TP_Camera myCamera;

		if (Camera.main != null) {
			//Debug.Log ("There is a main camera");
			tempCamera = Camera.main.gameObject;
		
		//Wont reach here for multiplayer
		} else {
			//Debug.Log ("No main camera. Have to create one");
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent <Camera>();
			tempCamera.AddComponent <MKGlowFree>();
			tempCamera.tag = "MainCamera";
		}
	}

	void Start () {
		//validate distance (Clamp between min/max)
		distance = Mathf.Clamp(distance, min_distance, max_distance);
		startDistance = distance;
		Reset ();
	}
	

	void LateUpdate () {
		
		//Debug.Log ("Do these please");
		HandlePlayerInput ();	//check for the players inputs
		var count = 0;
		//use do loop so it loops at least once
		//Checks for occlusion at least once
		do{
			CalculateDesiredPosition();	
			count++;
		}while(CheckIfOccluded(count));


		//CheckCameraPoints (TargetLookAt.position, desiredPosition);	//visual testing only
		UpdatePosition ();
	}

	//public incase controller needs to reset the camera
	public void Reset(){
		mouseX = 0f;
		mouseY = 10f;
		distance = startDistance;
		desiredDistance = distance;
		preOcclusionDistance = distance;
	}

	void CalculateDesiredPosition(){

		ResetDesiredDistance ();	//reset distance to pre occlusion if needed
		//Get the distance based on smoothing
		distance = Mathf.SmoothDamp(distance, desiredDistance,ref velDistance, distanceSmoothed);	//removed ref from in front of velDistance
		//Get the desired position
		//must reverse axis from mouse
		desiredPosition = CalculatePosition(mouseY, mouseX, distance);

	}

	Vector3 CalculatePosition(float rotationX, float rotationY, float distance){

		//Creating a direction vector
		Vector3 direction = new Vector3(0,0,-distance);	//behind the player
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
		return TargetLookAt.position + rotation * direction;
	}

	bool CheckIfOccluded(int count){
		//return false;
		var isOccluded = false;
		var nearestDistance = CheckCameraPoints (TargetLookAt.position, desiredPosition);
		if(nearestDistance != -1){
			//Debug.Log ("Something was hit");
			if (count < maxOcclusionChecks) {
				isOccluded = true;
				distance -= occlusionDistanceStep;	//step the camera forward

				if (distance < 0.25f) {
					distance = 0.25f;	//may need to be adjusted for suitability
				}

			} else {
				distance = nearestDistance - Camera.main.nearClipPlane;	//stop camera being inside player
			}

			desiredDistance = distance;
			distanceSmoothed = distanceResumeSmooth;	//smoothing when camera backs out
		}
		return isOccluded;

	}

	void ResetDesiredDistance(){
		if (desiredDistance < preOcclusionDistance) {	//is desiredDistance closer than the preOccluded ditance

			//camera has been repositioned using occlusion
			var position = CalculatePosition(mouseY, mouseX, preOcclusionDistance);

			//new position so create new points plane and check for occlusion
			var nearestDistance = CheckCameraPoints(TargetLookAt.position, position);

			//if checkCameraPoints returns -1 (no occlusion needs to occur)
			if (nearestDistance == -1 || nearestDistance > preOcclusionDistance) {
				desiredDistance = preOcclusionDistance;	//start smoothing camera back to original position
			}
		}
			
	}


	float CheckCameraPoints(Vector3 from, Vector3 to){

		var nearestDistance = -1f;	//if the nearest distance returned stays at -1 there has been no collision

		RaycastHit hitInfo;
		CameraHelper.clipPlanePoints clipPlanePoints = CameraHelper.ClipPlaneAtNear (to);

		//Draw debug lines

		Debug.DrawLine(from, to + transform.forward * - Camera.main.nearClipPlane, Color.red); //Draw behind the camera the distance between the camera and the clip plane
		Debug.DrawLine(from, clipPlanePoints.upperLeft);
		Debug.DrawLine(from, clipPlanePoints.lowerLeft);
		Debug.DrawLine(from, clipPlanePoints.upperRight);
		Debug.DrawLine(from, clipPlanePoints.lowerRight);

		Debug.DrawLine(clipPlanePoints.upperLeft, clipPlanePoints.upperRight);
		Debug.DrawLine(clipPlanePoints.upperRight, clipPlanePoints.lowerRight);
		Debug.DrawLine(clipPlanePoints.lowerRight, clipPlanePoints.lowerLeft);
		Debug.DrawLine(clipPlanePoints.lowerLeft, clipPlanePoints.upperLeft);

		if(Physics.Linecast(from,clipPlanePoints.upperLeft, out hitInfo) && hitInfo.collider.tag != "Player"){
			nearestDistance = hitInfo.distance;
		}
		if(Physics.Linecast(from,clipPlanePoints.lowerLeft, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1){ //check if it is closer
				nearestDistance = hitInfo.distance;
			}
		}
		if(Physics.Linecast(from,clipPlanePoints.upperRight, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1){ //check if it is closer
				nearestDistance = hitInfo.distance;
			}
		}
		if(Physics.Linecast(from,clipPlanePoints.lowerRight, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1){ //check if it is closer
				nearestDistance = hitInfo.distance;
			}
		}
		if(Physics.Linecast(from,clipPlanePoints.lowerRight, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1){ //check if it is closer
				nearestDistance = hitInfo.distance;
			}
		}
		if(Physics.Linecast(from, to + transform.forward * Camera.main.nearClipPlane, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1){ //check if it is closer
				nearestDistance = hitInfo.distance;
			}
		}


		return nearestDistance;

	}





	void UpdatePosition(){
		
		var posX = Mathf.SmoothDamp (position.x, desiredPosition.x, ref vel_X, x_Smooth);	//removed ref from in front of vel_X
		var posY = Mathf.SmoothDamp (position.y, desiredPosition.y, ref vel_Y, y_Smooth);	//removed ref from in front of vel_Y
		var posZ = Mathf.SmoothDamp (position.z, desiredPosition.z, ref vel_Z, x_Smooth);	//removed ref from in front of vel_Z
	
		position = new Vector3 (posX, posY, posZ);

		transform.position = position;	//set the cameras world position to this calculated position
	
		transform.LookAt (TargetLookAt);
	}

	void HandlePlayerInput(){

		var deadZone  = 0.01f;

		if (Input.GetMouseButton (1)) {
			// The RightMouseButton is down get mouseAxisInput
			mouseX += Input.GetAxis("Mouse X") * x_MouseSensitivity;
			//inverse as flipping mouse direction
			mouseY -= Input.GetAxis("Mouse Y") * y_MouseSensitivity;
		}	

		// Where y mouse rotation is clamped
		mouseY = CameraHelper.ClampAngle(mouseY, y_MinLimit, y_MaxLimit);

		if (Input.GetAxis ("Mouse ScrollWheel") < -deadZone || Input.GetAxis ("Mouse ScrollWheel") > deadZone) {
			desiredDistance = Mathf.Clamp(distance - Input.GetAxis ("Mouse ScrollWheel") * mouseWheelMouseSensitivity, min_distance, max_distance);	//sets the desird distance thee player wants (clamped)

			preOcclusionDistance = desiredDistance;	//setting the pre occlusion distance to the desired diatance value
			distanceSmoothed = distanceSmooth;		
		}
	}
}
