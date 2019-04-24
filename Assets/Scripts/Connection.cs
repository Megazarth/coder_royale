using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connection : MonoBehaviour {

	public GameObject message;

	// Use this for initialization
	void Start () {
		CheckForInternetConnetion ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckForInternetConnetion() {
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
			Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
			message.SetActive (false);
			FirebaseConn.instance.InitConnection ();
			SceneManager.LoadScene ("Start");
		}

		else {
			message.SetActive (true);
		}
	}

	public void OnCancel() {
		SceneManager.LoadScene ("Start");
	}
}
