using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Money : MonoBehaviour {

    public Text moneyText;
    public int numMoney;
    //public GameObject bullet;

	// Use this for initialization
	void Start () {
        numMoney = 100;
	}
	
	// Update is called once per frame
	void Update () {
        moneyText.text = "Money: " + numMoney.ToString();
	}

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Cube")
        {
            numMoney++;
        }
    }
}
