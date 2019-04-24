using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : Enemy {

	public AudioClip[] attackSound;

	protected override void Awake() {
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		if (GameManager.instance.level > 1) {
			base.health += base.health * (int)Mathf.Log(GameManager.instance.level, 2f);
			base.strength += base.strength * (int)Mathf.Log(GameManager.instance.level, 2f);
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	public void AttackPlayer() {
		Debug.Log ("Mercenary Attacked");
		player.GetComponent<Player> ().TakeDamage (base.strength);
		base.audioSource.PlayOneShot (attackSound [Random.Range (0, attackSound.Length)], 0.3f);
	}

}
