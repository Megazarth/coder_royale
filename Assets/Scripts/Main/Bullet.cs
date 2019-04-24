using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject[] explodeAnimation;
	private bool hasCollide = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("RoomSpawnPoint") || other.CompareTag ("Player") || other.CompareTag("BridgeEnd")) {
			return;
		}

		if (other.CompareTag("Box")) {
			other.GetComponent<ItemBox> ().TakeDamage(10f);
		}

		if (!hasCollide) {
			hasCollide = true;
			Instantiate (explodeAnimation [Random.Range (0, explodeAnimation.Length)], transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}
}
