using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public bool isStart, isQuit;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseUp()
    {
        if (isStart)
        {
            Application.LoadLevel(1);
        }

        if (isQuit)
        {
            Application.Quit();
        }
    }
}
