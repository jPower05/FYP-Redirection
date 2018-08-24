using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *	Author James Power 20067779 
 * 
 * 	Attached to the HealthManager gameobject
 * 	Controls the Gui element Health Bar which acts as the timer to solve puzzles
*/

public class Health : MonoBehaviour {

	Slider healthBar;
	public float timer;
	float maxHealth;
	GameObject text;
	public bool needReset = false;
	// Use this for initialization
	void Start () {
		healthBar = GameObject.Find ("HealthBar").GetComponent<Slider>();
		maxHealth = healthBar.maxValue;
		timer = maxHealth;
		text = GameObject.Find ("EnergyBarValue");
		text.GetComponent<Text> ().text = healthBar.value.ToString ();

	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		healthBar.value = timer;
		//Debug.Log ("Health bar = " + healthBar.value);
		text.GetComponent<Text> ().text = healthBar.value.ToString ();
		if (healthBar.value == maxHealth) {
			UpdateChangeScriptManager (false);
		}

		if(healthBar.value == 0){
			//reset the timer
			UpdateChangeScriptManager (true);
			healthBar.value = maxHealth;
			timer = maxHealth;



		}

		if (needReset) {
			resetHealthBar ();
		}
	}

	void resetHealthBar(){
		timer = maxHealth;
		needReset = false;
	}

	void UpdateChangeScriptManager(bool updateStatus){
		GameObject.Find ("ChangeScriptManager").GetComponent<ChangeScriptManager> ().respawn = updateStatus;

	}
}
