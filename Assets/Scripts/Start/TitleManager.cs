using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	public Text gameText;

	// Use this for initialization
	void Start () {
		StartCoroutine ("Blinking");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return))
			SceneManager.LoadScene ("Main");
	}

	private IEnumerator Blinking() {
		while(true) {
			gameText.enabled = false;
			yield return new WaitForSeconds(0.5f);
			gameText.enabled = true;
			yield return new WaitForSeconds(0.5f);

		}
	}
}
