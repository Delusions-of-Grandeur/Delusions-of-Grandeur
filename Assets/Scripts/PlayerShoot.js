#pragma strict

var character : Transform; //main character
 var bullet : Transform; // the bullet prefab
 private var spawnPt : GameObject; // holds the spawn point object
 
 function Update(){   
   if(Input.GetButton ("Fire1")){ // only do anything when the button is pressed:
     var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
     var hit : RaycastHit;
     if (Physics.Raycast (ray, hit, 100)){
       Debug.DrawLine (character.position, hit.point);
       // cache oneSpawn object in spawnPt, if not cached yet
       //if (!spawnPt) spawPt = GameObject.Find("oneSpawn");
       var projectile = Instantiate(bullet, spawnPt.transform.position, Quaternion.identity); 
       // turn the projectile to hit.point
       projectile.transform.LookAt(hit.point); 
       // accelerate it
       projectile.GetComponent.<Rigidbody>().velocity = projectile.transform.forward * 10;
     }
   }
 }