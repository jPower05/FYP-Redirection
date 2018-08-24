using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 *  Author James Power 20067779
 * 
 * 	Script attached to main camera in SplashScreen level
 * 	Fades in and out the logo and after 3 seconds automatically loads the next scene which is the main menu scene
 * 
 */

public class SplashFade : MonoBehaviour {

	public Image splashScreenLogo;

	IEnumerator Start(){
		splashScreenLogo.canvasRenderer.SetAlpha (0.0f);	//initially logo is completely transparent
		Fade(1.0f, 2.5f, false);
		yield return new WaitForSeconds (2.5f);	//wait 2.5 seconds until continuing 
		Fade(0.0f, 2.5f, false);
		yield return new WaitForSeconds (2.5f);	//wait 2.5 seconds until continuing 
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);	//automatically transition to next scene

	}

	void Fade(float alpha, float duration , bool action){
		splashScreenLogo.CrossFadeAlpha (alpha, duration, action);
	}
}
