using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 	Author James Power 20067779
 * 
 * 	Gets the initial position and rotation of mirrors so they can be reset when the players respawn
 * 
*/
public class TransformInfo : MonoBehaviour {

	public Vector3 initialPos;
	public Quaternion initialRot;

	// Use this for initialization
	void Start () {
		initialPos = this.gameObject.transform.localPosition;
		initialRot = this.gameObject.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
