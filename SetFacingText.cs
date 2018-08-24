using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	Author James Power 20067779 
 *
 *	Script attached to the text mesh component player name which is a child of the Irok prefab
*/

public class SetFacingText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(Camera.main.transform.position);
		this.transform.Rotate(new Vector3(0,180,0));
		
	}
}
