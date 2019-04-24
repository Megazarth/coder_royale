using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

	public GameObject explodeAnimation;
	private bool hasCollide = false;
	private float damage;

	void Start() {
		damage = GameObject.FindGameObjectWithTag ("Remy").GetComponent<Remy> ().strength;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag ("Player")) {
			return;
		}

		if (other.CompareTag ("Player")) {
			other.GetComponent<Player> ().TakeDamage (damage);
		}

		if (!hasCollide) {
			hasCollide = true;
			Instantiate (explodeAnimation , transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}
}
