using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	Author James Power 20067779 
 * 
*/

public class ChangeScriptManager : MonoBehaviour {

	public int numPlayersInColVol = 0;
	public bool readyToChange;
	public bool respawn;

	// Use this for initialization
	void Start () {
		readyToChange = false;
		respawn = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Debug.Log ("NumPlayersInColVol " + numPlayersInColVol);
	}
		
}
