using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicEntity : MonoBehaviour {
	public float speed;
	public float health;
	public float strength;

	public bool isAlive = true;

	protected SpriteRenderer sr;

	protected virtual void Awake() {
		sr = GetComponent<SpriteRenderer> ();
	}

	protected IEnumerator DamageIndicator() {
		sr.color = Color.red;
		yield return new WaitForSeconds (0.1f);
		sr.color = Color.white;
	}

	public virtual void TakeDamage (float damage) {
		StartCoroutine (DamageIndicator ());
		health -= damage;
		Debug.Log ("Damage = " + damage);
		if (health <= 0) {
			isAlive = false;
			health = 0;
		}
			
	}
}
