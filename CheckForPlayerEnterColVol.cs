using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 *	Author James Power 20067779 
 * 
 * 	Attached to the col vol gameobject but initially disabled
 * 	Manages players entering the collsion volume
 * 	If two players are in the movement is switched from flat to spherical
 * 
*/

public class CheckForPlayerEnterColVol : NetworkBehaviour  {

	public GameObject numManager;

	void OnTriggerEnter( Collider col){


		if (col.gameObject.tag == "Player") {
			if (numManager.GetComponent<ChangeScriptManager> ().numPlayersInColVol >= 2) {
				return;
			}
			numManager.GetComponent<ChangeScriptManager> ().numPlayersInColVol++;
		}
	}


}
