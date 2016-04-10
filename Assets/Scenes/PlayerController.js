#pragma strict

var speed = 5.0;
var crossHairTexture : Texture2D;

function Start () {
	Cursor.visible = false;
}

function Update () {
	var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
	var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
	var y = Input.GetAxis("Jump") * Time.deltaTime * speed;
	transform.Translate(x,y,z);
}

function OnGUI () {
	var vectorX = Input.mousePosition.x;
	var vectorY = Input.mousePosition.y;
	GUI.DrawTexture(Rect(vectorX - 15,-vectorY + Screen.height-15,30,30), crossHairTexture);
}