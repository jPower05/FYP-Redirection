using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 	Author James Power 20067779
 * 
 * 	Attached to the main Camera
 * 	Operates as a loose follow camera for the player when spherical movement is enabled
 * 
 * 	Credit: Adapted and altered from the Pushy Pixels Cooking with Unity Mario Galaxy playlist on Youtube.  
 * 	https://www.youtube.com/playlist?list=PLlHjNcdoyw6UK30xrTUhjM-usQOOE5jhN
 * 
*/
public class TP_SphericalCamera : MonoBehaviour {
	public GameObject targetLookat; 	//the target the main camera will focus on. Needed for multiplayer extension
	private Vector3 cameraOffset;
	public float cameraSpeed = 80.0f;
	public float cameraRotationSpeed = 30.0f;
	public float distanceCameraIsBehindPlayer = 12.0f;
	public float distanceCameraIsAbovePlayer = 3.0f;
	//private GameObject player;
	void Start () {
		
	}

	void Update () {

		if (targetLookat.gameObject == null) {
			return;
		}
		//GameObject player = targetLookat;
		Vector3 goalPosition;
		Quaternion goalRotation;
		cameraOffset = targetLookat.transform.position - targetLookat.transform.position + new Vector3 (0, -distanceCameraIsAbovePlayer, distanceCameraIsBehindPlayer);
		//move camera to player position only for rotating. not seen in frame
		goalPosition = Vector3.Lerp(transform.position, targetLookat.transform.position, cameraSpeed * Time.deltaTime);	//camera behind the player	
		//change the camera orientation
		goalRotation = Quaternion.LookRotation(targetLookat.transform.up, -targetLookat.transform.forward );
		//move camera back to original position relative to oofset annd LookRotation
		goalPosition -= (transform.rotation * cameraOffset)  ;	//make it behind the player

		//following the player
		Vector3 goalDirection = (goalPosition - transform.position);
		Vector3 goalOffset = goalDirection * cameraSpeed * Time.deltaTime;
		//if need to move the camera
		if (Vector3.Dot ((goalPosition - transform.position), goalPosition - (transform.position + goalOffset)) > 0.01f) {
			//transform.position += goalOffset * Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, transform.position + goalOffset, cameraSpeed * Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp(transform.position, goalPosition, cameraSpeed * Time.deltaTime);
		}
		//rotate the camera (smoothly)
		transform.rotation = Quaternion.RotateTowards (transform.rotation, goalRotation, cameraRotationSpeed * Time.deltaTime);
		//transform.rotation = Quaternion.Lerp (transform.rotation, goalRotation, cameraRotationSpeed*0.45f * Time.deltaTime);
	}
}
