using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour {

	public float energy;
	public AudioClip[] crateBreak;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.PlayOneShot (crateBreak [Random.Range (0, crateBreak.Length)], 2f);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			other.GetComponent<Player> ().addEnergy (energy);
			Destroy (gameObject);
		}
		return;
	}
}
