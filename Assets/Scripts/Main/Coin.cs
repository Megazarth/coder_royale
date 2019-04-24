using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public LayerMask playerLayer;
	public float speed;
	private GameObject player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (InSight())
			transform.position = Vector2.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
	}

	private bool InSight () {
		RaycastHit2D hit = Physics2D.CircleCast (transform.position, 3f, Vector2.zero, 0f, playerLayer);

		if (hit.transform != null) {
			return true;
		}

		return false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			other.GetComponent<Player> ().addMoney (1);
		}
	}
}
