using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	private string[] tutorial;
	private Text tutorialText;

	public float delay;

	void Awake() {
		tutorialText = GetComponent<Text> ();
		tutorialText.text = "";
		tutorial = new string[3];
		tutorial[0] = "Use <color=red>WASD</color> to move.";
		tutorial [1] = "<color=red>Left Click</color> to attack.";
		tutorial [2] = "<color=red>Right Click</color> for <color=red>Ultimate.</color>";	
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator StartTutorial() {
		tutorialText.text = tutorial [0];
		yield return new WaitForSeconds (delay);
		tutorialText.text = tutorial [1];
		yield return new WaitForSeconds (delay);
		tutorialText.text = tutorial [2];
		yield return new WaitForSeconds (delay);
		Destroy (gameObject);
	}
}
