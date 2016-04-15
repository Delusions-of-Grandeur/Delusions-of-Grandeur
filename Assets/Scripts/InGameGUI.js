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
private var beforeLastHitObjUpgrade : GameObject;
private var lastHitObj : GameObject;
var allMats : Material[];
//

//build selection items
var onColor : Color;
var offColor : Color;
var allStructures : GameObject[];
var transparentStructures : GameObject[];
//

private var playerScript : UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl;
private var playerDisplay : PlayerDisplay;

public var buildLvl1Gattling : int;
public var buildLvl2Gattling : int;
public var buildLvl1Sniper : int;
public var buildLvl2Sniper : int;

function Start()
{
	//reset the structure index, refresh the GUI
	structureIndex = 0;
	hoverMat = allMats[0];

}


function Update ()
{
//	print(playerDisplay.getMoney())
   	// get a reference to the target script (ScriptName is the name of your script):
   	var thePlayer = GameObject.FindWithTag("Player");
   	playerScript = thePlayer.GetComponent("UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl");
   	playerDisplay = thePlayer.GetComponent("PlayerDisplay");

	if(Input.GetKeyDown(KeyCode.Alpha1)){
		structureIndex = 0;
	} else if(Input.GetKeyDown(KeyCode.Alpha2)){
		structureIndex = 2;
	}

	if(Input.GetKeyDown(KeyCode.B)){ // build
		functionIndex = 0;
		hoverMat = allMats[0];

		if(beforeLastHitObjUpgrade != null ){
			for (var child : Transform in beforeLastHitObjUpgrade.transform){
	       		if (child.gameObject.name == "Transparent GatlingTower Lvl 2(Clone)"){
	        		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent GatlingTower Lvl 2(Clone)").gameObject);
	       		}
	      		if (child.gameObject.name == "Transparent SniperTower Lvl 2(Clone)"){
	         		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent SniperTower Lvl 2(Clone)").gameObject);
	     		}
	  		}
   		}
		beforeLastHitObjUpgrade = null;
	} else if(Input.GetKeyDown(KeyCode.N)){ // Sell
		functionIndex = 1;
		hoverMat = allMats[1];
		if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
			Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
		}
		beforeLastHitObj = null;

		if(beforeLastHitObjUpgrade != null ){
			for (var child : Transform in beforeLastHitObjUpgrade.transform){
	       		if (child.gameObject.name == "Transparent GatlingTower Lvl 2(Clone)"){
	        		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent GatlingTower Lvl 2(Clone)").gameObject);
	       		}
	      		if (child.gameObject.name == "Transparent SniperTower Lvl 2(Clone)"){
	         		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent SniperTower Lvl 2(Clone)").gameObject);
	     		}
	  		}
   		}
		beforeLastHitObjUpgrade = null;
	} else if(Input.GetKeyDown(KeyCode.U)){ // Upgrade
		functionIndex = 2;
		hoverMat = allMats[2];
		if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
			Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
		}
		beforeLastHitObj = null;
	}

	if(!playerScript.aim) //if the build panel is open...
	{
		
		//create a ray, and shoot it from the mouse position, forward into the game
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		var hit : RaycastHit;
		if(Physics.Raycast (ray, hit, 1000, placementLayerMask)) //if the RAY hits anything on right LAYER, within 1000 meters, save the hit item in variable "HIT", then...
		{
//			print("Building mode");
			if(lastHitObj) //if we had previously hit an object...
			{
				lastHitObj.GetComponent.<Renderer>().material = originalMat; //visually de-select that object
			}

			lastHitObj = hit.collider.gameObject; //replace the "selected plane" with this new plane that the raycast just hit

			//adding transparent towers
			if(lastHitObj.tag == "PlacementPlane_Open"){
				if(beforeLastHitObjUpgrade != null ){
					for (var child : Transform in beforeLastHitObjUpgrade.transform){
			       		if (child.gameObject.name == "Transparent GatlingTower Lvl 2(Clone)"){
			        		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent GatlingTower Lvl 2(Clone)").gameObject);
			       		}
			      		if (child.gameObject.name == "Transparent SniperTower Lvl 2(Clone)"){
			         		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent SniperTower Lvl 2(Clone)").gameObject);
			     		}
			  		}
   				}
				beforeLastHitObjUpgrade = null;

				originalMat = lastHitObj.GetComponent.<Renderer>().material; //store the new plane's starting material, so we can reset it later
				lastHitObj.GetComponent.<Renderer>().material = hoverMat; //set the plane's material to the highlighted look

				if(functionIndex == 0){
					var tempStructure : GameObject = Instantiate(transparentStructures[structureIndex], lastHitObj.transform.position, Quaternion.identity);
					tempStructure.transform.parent = lastHitObj.transform;
					//tempStructure.transform.position.y = 0.9030163; // this is because they are displaced in the prepfab should fix

					if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
						Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
					}

					beforeLastHitObj = lastHitObj;
				}
			} else if(lastHitObj.tag == "PlacementPlane_Taken"){
				if(beforeLastHitObj != null && beforeLastHitObj.transform.GetChild(0).gameObject != null){
					Destroy(beforeLastHitObj.transform.GetChild(0).gameObject);
				}
				beforeLastHitObj = null;

				if (functionIndex != 0){
					originalMat = lastHitObj.GetComponent.<Renderer>().material; //store the new plane's starting material, so we can reset it later
					lastHitObj.GetComponent.<Renderer>().material = hoverMat; //set the plane's material to the highlighted look
				}

				if(functionIndex == 2){
					var tempStructureUpgrade : GameObject;
					if(lastHitObj.transform.GetChild(0).gameObject.tag == "GatlingTower1") {
						tempStructureUpgrade = Instantiate(transparentStructures[1], lastHitObj.transform.position, Quaternion.identity);
					} else if(lastHitObj.transform.GetChild(0).gameObject.tag == "SniperTower1"){
						tempStructureUpgrade = Instantiate(transparentStructures[3], lastHitObj.transform.position, Quaternion.identity);
					}

					tempStructureUpgrade.transform.parent = lastHitObj.transform;

					print(beforeLastHitObjUpgrade);
					if(beforeLastHitObjUpgrade != null ){
						for (var child : Transform in beforeLastHitObjUpgrade.transform){
				       		if (child.gameObject.name == "Transparent GatlingTower Lvl 2(Clone)"){
				        		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent GatlingTower Lvl 2(Clone)").gameObject);
				       		}
				      		if (child.gameObject.name == "Transparent SniperTower Lvl 2(Clone)"){
				         		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent SniperTower Lvl 2(Clone)").gameObject);
				     		}
				  		}
   					}
					beforeLastHitObjUpgrade = lastHitObj;
				}
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

			if(beforeLastHitObjUpgrade != null ){
				for (var child : Transform in beforeLastHitObjUpgrade.transform){
		       		if (child.gameObject.name == "Transparent GatlingTower Lvl 2(Clone)"){
		        		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent GatlingTower Lvl 2(Clone)").gameObject);
		       		}
		      		if (child.gameObject.name == "Transparent SniperTower Lvl 2(Clone)"){
		         		Destroy(beforeLastHitObjUpgrade.transform.Find("Transparent SniperTower Lvl 2(Clone)").gameObject);
		     		}
		  		}
   			}

			beforeLastHitObjUpgrade = null;
		}

		//drop turrets on click
		if(Input.GetMouseButtonDown(0) && lastHitObj) //left mouse was clicked, and we have a tile selected
		{
			if(lastHitObj.tag == "PlacementPlane_Open" && functionIndex == 0) //if the selected tile is "open"...
			{
				var newStructure : GameObject;
				if(structureIndex == 0 && playerDisplay.money >= buildLvl1Gattling){
					newStructure = Instantiate(allStructures[structureIndex], lastHitObj.transform.position, Quaternion.identity);
					newStructure.transform.parent = lastHitObj.transform;
					lastHitObj.tag = "PlacementPlane_Taken";
					playerDisplay.money -= buildLvl1Gattling;
				} else if(structureIndex == 2 && playerDisplay.money >= buildLvl1Sniper){
					newStructure = Instantiate(allStructures[structureIndex], lastHitObj.transform.position, Quaternion.identity);
					newStructure.transform.parent = lastHitObj.transform;
					lastHitObj.tag = "PlacementPlane_Taken";
					playerDisplay.money -= buildLvl1Sniper;
				}

			} else if (lastHitObj.tag == "PlacementPlane_Taken" && functionIndex == 1){

				if(lastHitObj.transform.GetChild(0).gameObject.tag == "GatlingTower1"){
					playerDisplay.money += buildLvl1Gattling;
				} else if(lastHitObj.transform.GetChild(0).gameObject.tag == "SniperTower1"){
					playerDisplay.money += buildLvl1Sniper;
				} else if(lastHitObj.transform.GetChild(0).gameObject.tag == "GatlingTower2"){
					playerDisplay.money += buildLvl2Gattling;
				} else if(lastHitObj.transform.GetChild(0).gameObject.tag == "SniperTower2"){
					playerDisplay.money += buildLvl2Sniper;
				}

				Destroy(lastHitObj.transform.GetChild(0).gameObject);
				lastHitObj.tag = "PlacementPlane_Open";
			} else if (lastHitObj.tag == "PlacementPlane_Taken" && functionIndex == 2){
				print(lastHitObj.transform.GetChild(0).tag);
				if(lastHitObj.transform.GetChild(0).gameObject.tag == "GatlingTower1" && playerDisplay.money >= buildLvl2Gattling){
					print("gatling");
					playerDisplay.money -= buildLvl2Gattling;
					Destroy(lastHitObj.transform.GetChild(0).gameObject);
					var upgradeCubeStructure : GameObject = Instantiate(allStructures[1], lastHitObj.transform.position, Quaternion.identity);
					upgradeCubeStructure.transform.parent = lastHitObj.transform;
				} else if(lastHitObj.transform.GetChild(0).gameObject.tag == "SniperTower1" && playerDisplay.money >= buildLvl2Sniper){
					print("sniper");
					playerDisplay.money -= buildLvl2Sniper;
					Destroy(lastHitObj.transform.GetChild(0).gameObject);
					var upgradeSphereStructure : GameObject = Instantiate(allStructures[3], lastHitObj.transform.position, Quaternion.identity);
					upgradeSphereStructure.transform.parent = lastHitObj.transform;
				}
			}
		}
	}
}
