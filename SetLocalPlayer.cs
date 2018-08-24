using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Attaches to the IrokNewMovementNetworked Script

public class SetLocalPlayer : NetworkBehaviour {

	public GameObject message;
    [SyncVar]
    public string playerName = "player";

    [SyncVar]
    public Color playerColor;

    [SyncVar]
    public Quaternion rotation;

	[SyncVar]
	public int numPlayersInColVol = 0;   

	public float testTimer = 0.0f;

	//public GameObject netManager;

	bool change = false;

	bool help = false;

	bool displayChat = true;
    //[SyncVar] //
    //public Vector3 mirrorRotate;		//

    bool collided = false;

	GameObject msgSubmit;
	GameObject msgText;

	GameObject messagingGui;

	public Vector3 change_Sphere_Flat_Location1;
	public Vector3 change_Sphere_Flat_Location2;
	public Vector3 change_Sphere_Flat_Location3;

	public Vector3 change_Flat__Sphere_Location1;
	public Vector3 change_Flat__Sphere_Location2;
	public Vector3 change_Flat__Sphere_Location3;

	bool sphereToFlat1 = false;
	bool flatToSphere1 = false;
	bool sphereToFlat2 = false;
	bool flatToSphere2 = false;
	bool flatToSphere3 = false;

	GameObject initialSpawn;


	public bool moveable = false;	//this determines whether the mirror is able to be moved or not
    
    [SyncVar]
	GameObject otherObj;

	public GameObject numManager;

	[Command]
	public void CmdRotate(int methodNum){


		int methodNumber = methodNum;
		//RPC RotateFunction
		RpcRotate(methodNumber);
			

	}

	[Command]
	public void CmdParentTo(int methodNum, GameObject parent, GameObject child){
		int methodNumber = methodNum;
		RpcParent (methodNumber, parent, child);

	}

	[Command]
	public void CmdDestroyGameObject(GameObject gameObjToDestroy){
		Debug.Log ("Destroytying");
		//keep gameobject but disable mesh so players fall through it
		gameObjToDestroy.GetComponent<MeshRenderer>().enabled = false;


	}



	[Command]
	public void CmdUpdateNumPlayersInColVol(){
		Debug.Log ("Old value = " + numPlayersInColVol);
		numPlayersInColVol++;
		//RpcUpdateNumPlayersInColVol (updatedNumPlayersInColVol++);
		//RPC to update the value for all clients.
		Debug.Log ("New value = " + numPlayersInColVol);

	}

	[Command]
	public void CmdResetGameObjects(){
		GameObject[] mirrorGameObjects = GameObject.FindGameObjectsWithTag ("MirrorGameObject");
		Debug.Log ("size of array = " + mirrorGameObjects.Length);
		GameObject reEnable = GameObject.Find ("ChangePlatform");

		reEnable.GetComponent<MeshRenderer> ().enabled = true;	

		foreach (GameObject obj in mirrorGameObjects) {
			Debug.Log ("looping");
			obj.transform.position = obj.GetComponent<TransformInfo> ().initialPos;
			Debug.Log ("I think I changed the position");
			obj.transform.rotation = obj.GetComponent<TransformInfo> ().initialRot;
			Debug.Log ("I think I changed the rotation");
		}


	}

	[Command]
	public void CmdServerSendMessage(string msgText, string playerName){
		//create a new message panel
		//GameObject go = Instantiate(message) as GameObject;
		//GameObject chatPanel = GameObject.Find ("ChatPanel");
		//go.transform.SetParent (chatPanel.transform);
		//go.transform.GetChild (0).GetComponent<Text> ().text = msgText;
		RpcReciprocateMessage(msgText, playerName);


	}

	[ClientRpc]
	public void RpcReciprocateMessage(string s, string name){
		GameObject go = Instantiate(message) as GameObject;
		GameObject chatPanel = GameObject.Find ("ChatPanel");
		go.transform.SetParent (chatPanel.transform);
		go.transform.GetChild (0).GetComponent<Text> ().text = s;

	}

	[ClientRpc]
	void RpcUpdateNumPlayersInColVol(int updatedNumPlayersInColVol){
		numPlayersInColVol = updatedNumPlayersInColVol;
		Debug.Log (numPlayersInColVol + "Increased numPlayersInColVol");
	}


	[ClientRpc]
	void RpcParent(int methodNum, GameObject parent, GameObject child){
		if (methodNum == 6) {
			Debug.Log ("Parent = " + child.transform.parent.name);
			child.transform.SetParent (null);
			Debug.Log ("Name = " + child.transform.name);
			//Debug.Log ("Parent = " + child.transform.parent.name);

			Debug.Log ("Unparented");
		}
		if (methodNum == 7) {
			child.transform.SetParent (parent.transform);
			Debug.Log ("Name =  " + child.transform.parent.name); 
			Debug.Log ("Unparented");

		}
	}


	[ClientRpc]
	void RpcRotate(int methodNum){
		if (methodNum == 1){
			otherObj.transform.Rotate(0.0f, 1.0f, 0.0f * Time.deltaTime);

			Vector3 eulerAngles = otherObj.transform.rotation.eulerAngles;
 			//Debug.Log("transform.rotation angles x: " + eulerAngles.x + " y: " + eulerAngles.y + " z: " + eulerAngles.z); 
		}


		if (methodNum == 2){
			//Press J
		 	GameObject rotate;   
		 	rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
	      
		 	rotate.transform.Rotate(0.0f, 1.0f, 0.0f * Time.deltaTime);
		}

		if (methodNum == 3){
			GameObject rotate;   
	 		rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
      
	 		rotate.transform.Rotate(0.0f, -1.0f, 0.0f * Time.deltaTime);
		}
		if (methodNum == 4){
			GameObject rotate;   
	 		rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
      
	 		rotate.transform.Rotate(0.0f, 0.0f, 10.0f * Time.deltaTime);
		}
		if (methodNum == 5){
			GameObject rotate;   
	 		rotate = otherObj.transform.GetChild(0).GetChild(0).gameObject;
      
	 		rotate.transform.Rotate(0.0f, 0.0f, -10.0f * Time.deltaTime);
		}		
	}

	// Use this for initialization
	void Start()
	{

		GameObject test = GameObject.Find ("GravityWell");
		messagingGui = GameObject.Find ("MessagingGui");
		Debug.Log ("Found the gravity well");

		change_Sphere_Flat_Location1 = GameObject.Find ("change_Sphere_Flat_Location1").transform.position;
		change_Flat__Sphere_Location1 = GameObject.Find ("change_Flat_Sphere_Location1").transform.position;
		change_Sphere_Flat_Location2 = GameObject.Find ("change_Sphere_Flat_Location2").transform.position;
		change_Flat__Sphere_Location2 = GameObject.Find ("change_Flat_Sphere_Location2").transform.position;

		numManager = GameObject.Find ("ChangeScriptManager");

		initialSpawn = GameObject.Find ("SpawnPos1");

		//netManager = GameObject.FindGameObjectWithTag ("NetworkManager");
		//only enable the movement script on the local version of the player
		GetComponentInChildren<TextMesh> ().text = playerName;
		if (isLocalPlayer) {

			Debug.Log ("MY NAME IS " + playerName);
			//Debug.Log ("I am the local player!!!!!");
			GetComponent<TP_Movement> ().enabled = true;	//local player can move
			//set the target of the camera to be the local player	
			//Debug.Log("Attempting to target the local player");
			Camera.main.GetComponent<TP_Camera> ().TargetLookAt = this.gameObject.transform.GetChild (0);


			//GetComponentInChildren<TextMesh> ().gameObject.SetActive = false;
			this.transform.GetComponentInChildren<TextMesh> ().gameObject.SetActive (false);

			if (!isServer) {
				Debug.Log ("I AM A CLIENT");
			}

			msgSubmit = GameObject.Find ("SubmitMessage");
			msgSubmit.GetComponent<Button> ().onClick.AddListener (OnButtonClick);	//add a listener to the button
			msgText = GameObject.Find ("MessageText");

		}

		//Renderer rend = this.GetComponentInChildren<Renderer> ();
		Renderer rend = this.GetComponent<Renderer>();
		rend.material.shader = Shader.Find ("MK/Glow/Selective/Transparent/Diffuse");
		//Debug.Log ("Found shader");
		rend.material.SetColor ("_MKGlowColor", playerColor);    

		//Stops the players spawning on each other
		this.transform.position = new Vector3(Random.Range(initialSpawn.transform.position.x -20, initialSpawn.transform.position.x +20 ),initialSpawn.transform.position.y,initialSpawn.transform.position.z);    
	}


	void OnButtonClick(){
		if (isLocalPlayer) {
			//Debug.Log ("CLICKED");

			//Debug.Log(inputField.GetComponentInChildren<Text>().text);

			if (msgText.GetComponent<InputField> ().text == "") {
				Debug.Log ("Empty");
				//no point calling server if the text field is empty
				return;
			} else {
				Debug.Log (msgText.GetComponent<InputField> ().text);
				string messageToSend = msgText.GetComponent<InputField> ().text;
				//tell the server to create a message
				CmdServerSendMessage(messageToSend, this.playerName);
				//reset the message text field
				msgText.GetComponent<InputField> ().text = "";
			}

		}

	}
		

	// Update is called once per frame
	void Update () {

		if (isLocalPlayer){
			if (isServer) {
				if (Input.GetKeyDown (KeyCode.Delete)) {
					CmdResetGameObjects ();
				}
			}
		}

		if (isLocalPlayer) {
			if(Input.GetKeyDown(KeyCode.C)){
				//enable / disable being able to see the chat
				displayChat = !displayChat;
			}

			if (displayChat == false) {
				//disable the chat gui components
				messagingGui.SetActive(false);
			}
			if (displayChat == true) {
				//disable the chat gui components
				messagingGui.SetActive(true);
			}
		}



		if (isLocalPlayer) {


			if (sphereToFlat1 == true) {
				Change_Sphere_Flat (change_Sphere_Flat_Location1);
				sphereToFlat1 = false;
			}
			if (flatToSphere1 == true) {
				Change_Flat_Sphere (change_Flat__Sphere_Location1);
				flatToSphere1 = false;
			}
			if (sphereToFlat2 == true) {
				Change_Sphere_Flat (change_Sphere_Flat_Location2);
				sphereToFlat2 = false;
			}
			if (flatToSphere2 == true) {
				Change_Flat_Sphere (change_Flat__Sphere_Location2);
				flatToSphere2 = false;
			}
			if (flatToSphere3 == true) {
				Change_Flat_Sphere (change_Flat__Sphere_Location1);
				flatToSphere3 = false;
			}

			//check for Respawn
			if (numManager.GetComponent<ChangeScriptManager> ().respawn == true) {
				//respawn the player to starting position
				if (isServer) {
					Change_Sphere_Flat (new Vector3(Random.Range(initialSpawn.transform.position.x -20, initialSpawn.transform.position.x +20 ),initialSpawn.transform.position.y,initialSpawn.transform.position.z));    
				}
				if (!isServer) {
					Change_Sphere_Flat (new Vector3(Random.Range(initialSpawn.transform.position.x -20, initialSpawn.transform.position.x +20 ),initialSpawn.transform.position.y,initialSpawn.transform.position.z));    
				}

			}

			//end Game

			if (numManager.GetComponent<ChangeScriptManager> ().numPlayersInColVol == 3) {
				if (isServer) {
					Camera.main.GetComponent<TP_Camera> ().enabled = false;
					Camera.main.GetComponent<TP_SphericalCamera> ().enabled = true;
					Camera.main.GetComponent<TP_SphericalCamera> ().targetLookat = this.gameObject.transform.GetChild (0).gameObject;
					GetComponent<TP_Movement> ().enabled = false;
					GetComponent<CharacterController> ().enabled = false;
					this.gameObject.AddComponent<Rigidbody> ();
					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
					GetComponent<Rigidbody> ().useGravity = false;
					GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
					GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					GetComponent<CapsuleCollider> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().gravitySource = GameObject.Find ("LastPlanet").transform;
				}
				if (!isServer) {
					Camera.main.GetComponent<TP_Camera> ().enabled = false;
					Camera.main.GetComponent<TP_SphericalCamera> ().enabled = true;
					Camera.main.GetComponent<TP_SphericalCamera> ().targetLookat = this.gameObject.transform.GetChild (0).gameObject;	
					GetComponent<TP_Movement> ().enabled = false;
					GetComponent<CharacterController> ().enabled = false;
					this.gameObject.AddComponent<Rigidbody> ();
					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
					GetComponent<Rigidbody> ().useGravity = false;
					GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
					GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					GetComponent<CapsuleCollider> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().gravitySource = GameObject.Find ("LastPlanet").transform;
				}
			}
				
			//first convert from flat to spherical movement (affects all players)
				
			if (numManager.GetComponent<ChangeScriptManager> ().numPlayersInColVol == 2) {
				numManager.GetComponent<ChangeScriptManager> ().numPlayersInColVol = 0;
				if (isServer) {
					CmdDestroyGameObject(GameObject.Find("ChangePlatform"));
					Camera.main.GetComponent<TP_Camera> ().enabled = false;
					Camera.main.GetComponent<TP_SphericalCamera> ().enabled = true;
					Camera.main.GetComponent<TP_SphericalCamera> ().targetLookat = this.gameObject.transform.GetChild (0).gameObject;
					GetComponent<TP_Movement> ().enabled = false;
					GetComponent<CharacterController> ().enabled = false;
					this.gameObject.AddComponent<Rigidbody> ();
					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
					GetComponent<Rigidbody> ().useGravity = false;
					GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
					GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					GetComponent<CapsuleCollider> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().gravitySource = GameObject.Find ("FirstPlanet").transform;
				}
				if (!isServer) {
					Camera.main.GetComponent<TP_Camera> ().enabled = false;
					Camera.main.GetComponent<TP_SphericalCamera> ().enabled = true;
					Camera.main.GetComponent<TP_SphericalCamera> ().targetLookat = this.gameObject.transform.GetChild (0).gameObject;	
					GetComponent<TP_Movement> ().enabled = false;
					GetComponent<CharacterController> ().enabled = false;
					this.gameObject.AddComponent<Rigidbody> ();
					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
					GetComponent<Rigidbody> ().useGravity = false;
					GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
					GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					GetComponent<CapsuleCollider> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().enabled = true;
					GetComponent<TP_Spherical_Movement> ().gravitySource = GameObject.Find ("FirstPlanet").transform;
				}
			}
		}



		if(isLocalPlayer){
		//return if no collision
			if (collided == false) return;

			int methodNum = 1;

			//if there has been a collision
			if (collided){

				

				if (Input.GetKey(KeyCode.H)){
					Debug.Log ("hit H");	
					methodNum = 1;
					CmdRotate(methodNum);
		 		}
				//rotate the mirror part
				if (Input.GetKey(KeyCode.J)){
					methodNum = 2;
					CmdRotate(methodNum);
		 		}	
		 		if (Input.GetKey(KeyCode.B)){
					methodNum = 3;
					CmdRotate(methodNum);
		 		}
		 		if (Input.GetKey(KeyCode.N)){
					methodNum = 4;
					CmdRotate(methodNum);
		 		}
		 		if (Input.GetKey(KeyCode.M)){
					methodNum = 5;
					CmdRotate(methodNum);
		 		}

			//How the mirrors position is moved by the player

				//If the player hits the shift key 
			if (Input.GetKeyDown (KeyCode.E)) {
					if (moveable) {
						//if already parented_UNPARENT
						methodNum = 6;
						CmdParentTo (6, this.gameObject, otherObj);
						//otherObj.transform.SetParent(null); 	//unparent the mirror from the player
						moveable = false;
						return;	//jump out of method or else it will go down to the next loop and it will always be parented
					}

					if (!moveable) {
						methodNum = 7;
						CmdParentTo (7, this.gameObject, otherObj);
						moveable = true;
						//Debug.Log ("Is the mirror parented " + moveable);
						return;
					}

				}

			}
		}	
							
	}

	void Change_Sphere_Flat(Vector3 positionToMoveTo){
		

		if (isServer) {
			this.GetComponent<Transform> ().rotation = Quaternion.identity;//reset the objects rotation
			Camera.main.GetComponent<TP_SphericalCamera> ().enabled = false;	//disable sphericalCamera
			GetComponent<TP_Spherical_Movement> ().enabled = false;	//disable sphericalMovement
			GetComponent<TP_Movement> ().enabled = true;	//enable flat movement
			GetComponent<CharacterController> ().enabled = true;	//enable character controller
			Camera.main.GetComponent<TP_Camera> ().enabled = true;	//enable TP_Camera
			Destroy(GetComponent<Rigidbody>());
			GetComponent<CapsuleCollider> ().enabled = false;
			Camera.main.GetComponent<TP_Camera> ().TargetLookAt = this.gameObject.transform.GetChild (0);
			GetComponent<TP_Movement> ().enabled = true;
		}
		if (!isServer) {
			this.GetComponent<Transform> ().rotation = Quaternion.identity;//reset the objects rotation
			Camera.main.GetComponent<TP_SphericalCamera> ().enabled = false;	//disable sphericalCamera
			GetComponent<TP_Spherical_Movement> ().enabled = false;	//disable sphericalMovement
			GetComponent<TP_Movement> ().enabled = true;	//enable flat movement
			GetComponent<CharacterController> ().enabled = true;	//enable character controller
			Camera.main.GetComponent<TP_Camera> ().enabled = true;	//enable TP_Camera
			Destroy(GetComponent<Rigidbody>());
			GetComponent<CapsuleCollider> ().enabled = false;
			Camera.main.GetComponent<TP_Camera> ().TargetLookAt = this.gameObject.transform.GetChild (0);
			GetComponent<TP_Movement> ().enabled = true;
		}
		this.GetComponent<Transform> ().position = positionToMoveTo;
	}


	void Change_Flat_Sphere(Vector3 positionToMoveTo){

		if (isServer) {
			Camera.main.GetComponent<TP_Camera> ().enabled = false;
			Camera.main.GetComponent<TP_SphericalCamera> ().enabled = true;
			Camera.main.GetComponent<TP_SphericalCamera> ().targetLookat = this.gameObject.transform.GetChild (0).gameObject;
			GetComponent<TP_Movement> ().enabled = false;
			GetComponent<CharacterController> ().enabled = false;
			this.gameObject.AddComponent<Rigidbody> ();
			GetComponent<Rigidbody> ().isKinematic = true;
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
			GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			GetComponent<CapsuleCollider> ().enabled = true;
			GetComponent<TP_Spherical_Movement> ().enabled = true;
		}
		if (!isServer) {
			Camera.main.GetComponent<TP_Camera> ().enabled = false;
			Camera.main.GetComponent<TP_SphericalCamera> ().enabled = true;
			Camera.main.GetComponent<TP_SphericalCamera> ().targetLookat = this.gameObject.transform.GetChild (0).gameObject;	
			GetComponent<TP_Movement> ().enabled = false;
			GetComponent<CharacterController> ().enabled = false;
			this.gameObject.AddComponent<Rigidbody> ();
			GetComponent<Rigidbody> ().isKinematic = true;
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
			GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			GetComponent<CapsuleCollider> ().enabled = true;
			GetComponent<TP_Spherical_Movement> ().enabled = true;
		}

		GetComponent<Transform> ().position = positionToMoveTo;
	}
		

	void OnTriggerEnter(Collider collider) {

		//Get collided game object
        
        //Check for mirror tag
		if (collider.gameObject.tag == "MirrorGameObject") {
	
			otherObj = collider.gameObject;
			//set bool collided to true so update will check for key press
			collided = true;

		} else if (collider.gameObject.tag == "Sphere_Flat_1") {
			sphereToFlat1 = true;

		} else if (collider.gameObject.tag == "Flat_Sphere_1") {
			flatToSphere1 = true;

		} else if (collider.gameObject.tag == "Sphere_Flat_2") {
			sphereToFlat2 = true;

		} else if (collider.gameObject.tag == "Flat_Sphere_2") {
			flatToSphere2 = true;

		} else if (collider.gameObject.tag == "Flat_Sphere_3") {
			flatToSphere3 = true;
		} else if (collider.tag == "EndGame") {
			SceneManager.LoadScene ("MainMenu");
		}
		else {
			return;
		}


			
		
    }	

    void onTriggerExit(){
		otherObj = null;	//stops player rotating a mirror they are not colliding with
    	collided = false;
		//numPlayersInColVol--;
		//Debug.Log (numPlayersInColVol + "Decreased numPlayersInColVol");

    }
}






			
