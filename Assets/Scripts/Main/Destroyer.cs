using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

	void Start() {
		Destroy (gameObject, 1f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("RoomSpawnPoint"))
			Destroy (other.gameObject);
	}
}
