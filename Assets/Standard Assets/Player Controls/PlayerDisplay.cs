using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour {

	public int money;
	public Text moneyText;

	// Use this for initialization
	void Start () {
		money = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		Screen.lockCursor = true;
		moneyText.text = "Money: " + money.ToString();
	}
}
