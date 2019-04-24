using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedBridge : MonoBehaviour {

	public GameObject bridge;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("BridgeEnd"))
			Destroy (bridge);
	}
		
}
