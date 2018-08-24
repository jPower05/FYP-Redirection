using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	Author James Power 20067779 
 * 
 * 	Attached to the Laser Source objects but initially disabled
 * 	Controls the laser beam rendering and it's collisions with game objects ie. mirrors
 * 
*/
[RequireComponent (typeof(LineRenderer))]
public class LineRendererReflection : MonoBehaviour
{
    //this game object's Transform
    private Transform goTransform;
    //the attached line renderer
    private LineRenderer lineRenderer;
    //a ray
    private Ray ray;
    //a RaycastHit variable, to gather informartion about the ray's collision
    private RaycastHit hit;
    //reflection direction
    private Vector3 direction;
    //the number of reflections
    public int nReflections = 5;
    //max length
    public float maxLength = 100000f;
    //the number of points at the line renderer
    private int numPoints;
    //private int pointCount;
    void Awake ()
    {
        //get the attached Transform component  
        goTransform = this.GetComponent<Transform> ();
        //get the attached LineRenderer component  
        lineRenderer = this.GetComponent<LineRenderer> ();
    }
    void Update ()
    {
        //clamp the number of reflections between 1 and int capacity  
        nReflections = Mathf.Clamp (nReflections, 1, nReflections);
        ray = new Ray (goTransform.position, goTransform.forward);    
        //start with just the origin
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition (0, goTransform.position);
        float remainingLength = maxLength;
        //bounce up to n times
        for (int i = 0; i < nReflections; i++) {
            // ray cast
            if (Physics.Raycast (ray.origin, ray.direction, out hit, remainingLength)) {
                //we hit, update line renderer
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition (lineRenderer.positionCount - 1, hit.point);
                // update remaining length and set up ray for next loop
                remainingLength -= Vector3.Distance (ray.origin, hit.point);
                ray = new Ray (hit.point, Vector3.Reflect(ray.direction, hit.normal));
                // break loop if we don't hit a Mirror
				if (hit.collider.tag == "BeamEndPoint_1") {
					GameObject.FindGameObjectWithTag ("BeamEndPoint_1").GetComponent<AudioSource> ().Play ();
					Destroy(GameObject.Find("ExitDoor_1"));
					Destroy(GameObject.Find("EnterDoor_1"));
					GameObject.Find ("LaserSource_1").GetComponent<LineRendererReflection> ().enabled = false;
					GameObject.Find ("HealthManager").GetComponent<Health> ().needReset = true;
				}

				if (hit.collider.tag == "BeamEndPoint_2") {
					GameObject.FindGameObjectWithTag ("BeamEndPoint_2").GetComponent<AudioSource> ().Play ();
					Destroy(GameObject.Find("ExitDoor_2"));
					Destroy(GameObject.Find("EnterDoor_2"));
					GameObject.Find ("LaserSource_2").GetComponent<LineRendererReflection> ().enabled = false;
					GameObject.Find ("HealthManager").GetComponent<Health> ().needReset = true;
				}
				if (hit.collider.tag == "BeamEndPoint_3") {
					GameObject.FindGameObjectWithTag ("BeamEndPoint_3").GetComponent<AudioSource> ().Play ();
					Destroy(GameObject.Find("EXIT"));
					GameObject.Find ("ChangeScriptManager").GetComponent<ChangeScriptManager> ().numPlayersInColVol = 3;
					GameObject.Find ("LaserSource_3").GetComponent<LineRendererReflection> ().enabled = false;
					GameObject.Find ("HealthManager").GetComponent<Health> ().needReset = true;
				}

				if (hit.collider.tag != "mirror")
                    break;

            } else {
                // We didn't hit anything, draw line to end of ramainingLength
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition (lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
                break;
            }
        }
    }
}