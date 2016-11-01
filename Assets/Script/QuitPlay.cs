using UnityEngine;
using System.Collections;

public class QuitPlay : MonoBehaviour {

	public void Quit(){
		Application.Quit();
	}

	public void Play(){
		Application.LoadLevel("main");
	}

}
