using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeDisplay : MonoBehaviour {
	public Text display;

	// Use this for initialization
	void Start () {
		display.text = "You won in " + Mathf.Floor(GameManager.completionTime) + " seconds!";
	}

}
