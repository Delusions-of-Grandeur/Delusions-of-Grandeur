#pragma strict

var structureIndex : int;
var functionIndex : int;
var throughObject : Transform;

//Placement Plane items
var placementPlanesRoot : Transform;
private var hoverMat : Material;
var placementLayerMask : LayerMask;
private var originalMat : Material;
private var beforeLastHitObj : GameObject;
private var lastHitObj : GameObject;
var allMats : Material[];
//

//build selection items
var onColor : Color;
var offColor : Color;
var allStructures : GameObject[];
var transparentStructures : GameObject[];
//

var player : GameObject;

public var canBuild : boolean;


function Start()
{
	//reset the structure index, refresh the GUI
	structureIndex = 0;
	hoverMat = allMats[0];
}


function Update () 
{
 
   	// get a reference to the target script (ScriptName is the name of your script):
   	//var targetScript = player.GetComponent(ThirdPersonUserController);

	if(Input.GetKeyDown(KeyCode.Alpha1)){
		structureIndex = 0;
	} else if(Input.GetKeyDown(KeyCode.Alpha2)){
		structureIndex = 2;
	}

	if(Input.GetKeyDown(KeyCode.B)){ // build
		functionIndex = 0;
		hoverMat = allMats[0];
	} else if(Input.GetKeyDown(KeyCode.N)){ // Sell
		functionIndex = 1;
		hoverMat = allMats[1];
	} else if(Input.GetKeyDown(KeyCode.U)){ // Upgrade
		functionIndex = 2;
		hoverMat = allMats[2];
	}

	if(canBuild) //if the build panel is open...
	{		
		//create a ray, and shoot it from the mouse position, forward into the game
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		var hit : RaycastHit;
		if(Physics.Raycast (ray, hit, 1000, placementLayerMask)) //if the RAY hits anything on right LAYER, within 1000 meters, save the hit item in variable "HIT", then...
		{

			if(lastHitObj) //if we had previously hit an object...
			{
				lastHitObj.GetComponent.<Renderer>().material = originalMat; //visually de-select that object
			}
			
			lastHitObj = hit.collider.gameObject; //replace the "selected plane" with this new plane that the raycast just hit

			//adding transparent towers
			if(lastHitObj.tag == "PlacementPlane_Open"){

				originalMat = lastHitObj.GetComponent.<Renderer>().material; //store the new plane's starting material, so we can reset it later
				lastHitObj.GetComponent.<Renderer>().material = hoverMat; //set the plane's material to the highlighted look

				var tempStructure : GameObject = Instantiate(transparentStructures[structureIndex], lastHitObj.transform.position, Quaternion.identity);
				tempStructure.transform.parent = lastHitObj.transform;
				tempStructure.transform.position.y = 0.9030163; // this is because they are displaced in the prepfab should fix

				if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
					Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
				}

				beforeLastHitObj = lastHitObj;
			} else if(lastHitObj.tag == "PlacementPlane_Taken"){
				if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
					Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
				}
				beforeLastHitObj = null;
			}
		}
		else //...if the raycast didn't hit anything (ie, the mouse moved outside the tiles) ...
		{
			if(lastHitObj) //if we had previously hit something...
			{
				lastHitObj.GetComponent.<Renderer>().material = originalMat; //visually de-select that object
				lastHitObj = null; //nullify the plane selection- this is so that we can't accidentally drop turrets without a proper and valid location selected
			}

			if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
				Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
				beforeLastHitObj = null;
			}
		}
		
		//drop turrets on click
		if(Input.GetMouseButtonDown(0) && lastHitObj) //left mouse was clicked, and we have a tile selected
		{
			if(lastHitObj.tag == "PlacementPlane_Open" && functionIndex == 0) //if the selected tile is "open"...
			{
//				//drop the chosen structure exactly at the tile's position, and rotation of zero. See how the "index" comes in handy here? :)
				var newStructure : GameObject = Instantiate(allStructures[structureIndex], lastHitObj.transform.position, Quaternion.identity);
				newStructure.transform.parent = lastHitObj.transform;
				newStructure.transform.position.y = 0.9030163; // this is because they are displaced in the prepfab should fix
				//set this tile's tag to "Taken", so we can't double-place structures
				lastHitObj.tag = "PlacementPlane_Taken";
			} else if (lastHitObj.tag == "PlacementPlane_Taken" && functionIndex == 1){
				Destroy(lastHitObj.transform.GetChild(0).gameObject);
				lastHitObj.tag = "PlacementPlane_Open";
			} else if (lastHitObj.tag == "PlacementPlane_Taken" && functionIndex == 2){
				print("test");
				print(lastHitObj.transform.GetChild(0).tag);
				if(lastHitObj.transform.GetChild(0).gameObject.tag == "Cube_Tower_1"){ 
					print("cube");
					Destroy(lastHitObj.transform.GetChild(0).gameObject);
					var upgradeCubeStructure : GameObject = Instantiate(allStructures[1], lastHitObj.transform.position, Quaternion.identity);
					upgradeCubeStructure.transform.position.y = 0.9030163; // this is because they are displaced in the prepfab should fix
					upgradeCubeStructure.transform.parent = lastHitObj.transform;
				} else if(lastHitObj.transform.GetChild(0).gameObject.tag == "Sphere_Tower_1"){ 
					print("sphere");
					Destroy(lastHitObj.transform.GetChild(0).gameObject);
					var upgradeSphereStructure : GameObject = Instantiate(allStructures[3], lastHitObj.transform.position, Quaternion.identity);
					upgradeSphereStructure.transform.position.y = 0.9030163; // this is because they are displaced in the prepfab should fix
					upgradeSphereStructure.transform.parent = lastHitObj.transform;
				}
			}
		}
	}	
}
