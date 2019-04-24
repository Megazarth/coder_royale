using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntered : MonoBehaviour {

	public GameObject fences;
	private BoxCollider2D trigger;
	private Spawner spawner;
	private Transform enemyHolder;

	void Start () {
		spawner = GetComponent<Spawner> ();
		fences.SetActive (false);
		this.trigger = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyHolder != null) {
			if (enemyHolder.childCount == 0 && fences.activeSelf) {
				fences.SetActive (false);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			this.trigger.enabled = false;
			spawner.Init ();
			Debug.Log (fences);
			if (!spawner.lastRoom)
				fences.SetActive (true);
			enemyHolder = spawner.GetEnemyHolder ();
			Debug.Log ("Player entered!");

		}
	}
		
}
