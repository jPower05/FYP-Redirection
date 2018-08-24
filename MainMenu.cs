using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * 	Author James Power 20067779
 * 
 * Attached to the MainMenu in the MainMenu scene
 * Controls the button actions in the scene and the swapping between menus
 * 
*/

public class MainMenu : MonoBehaviour {

	//Clicking the play button loads the next scene
	public void PlayGame () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	//clicking the quit button will quit the application
	public void QuitGame(){
		Application.Quit ();	
	}

}
