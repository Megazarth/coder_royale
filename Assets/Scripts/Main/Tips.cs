using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour {

	public Text tipsText;

	private string[] tips;

	void Awake() {
		tips = new string[5];
		tips[0] = "When you find the exit, try to explore some more.";
		tips[1] = "Once you entered a room, you cannot exit until all enemies are defeated";
		tips[2] = "Be wary of ranged enemy.";
		tips[3] = "Boxes might contain health or energy pickups.";
		tips[4] = "Use your money wisely. Things will get harder the higher you go.";
	}

	// Use this for initialization
	void Start () {
		if (GameManager.instance.level-1 < tips.Length) 
			tipsText.text = tips [GameManager.instance.level-1];
		else
			tipsText.text = tips [Random.Range(0, tips.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
