using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BasicEntity {

	public Transform boundary;

	public GameObject coin;
	public LayerMask playerLayer;
	public int minimumWaitTime;
	public int maximumWaitTime;

	protected GameObject player;
	protected Animator animator;
	protected Rigidbody2D rb2d;

	protected int horizontal;
	protected int vertical;

	protected bool inRange;
	protected bool pursue; //Pursue player no matter what
	protected bool coinSpawned;
	private IEnumerator movement;
	private int moveIndex;

	protected RaycastHit2D hit;

	protected enum enemyState {
		free, pursuing, death
	}

	protected enemyState currentState;

	protected AudioSource audioSource;

	protected override void Awake() {
		base.Awake ();
		movement = Movement ();
		moveIndex = 0;
		pursue = false;
		coinSpawned = false;
	}

	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		audioSource = GetComponent<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		inRange = false;
		currentState = enemyState.free;
		StartCoroutine (movement);
	}

	// Update is called once per frame
	protected virtual void Update () {

		switch (currentState) {

		case enemyState.free:
			{
				CheckLocation ();
				if (!base.isAlive)
					currentState = enemyState.death;
				if (InSight ())
					currentState = enemyState.pursuing;
				
			}
			break;
		case enemyState.pursuing:
			{
				if (!inRange) {
					StopCoroutine (movement);
					CalculateAngleForAnim (); //Pursue Player
					transform.position = Vector2.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
				}
				else
					animator.SetTrigger("Attack");
				if (!InSight () && !pursue) {
					StartCoroutine (movement);
					currentState = enemyState.free;
				}

				if (!base.isAlive)
					currentState = enemyState.death;
					
			}
			break;
		case enemyState.death:
			{
				
				GetComponent<Collider2D> ().enabled = false;
				StopCoroutine (movement);
				if (!coinSpawned) {
					Debug.Log ("Coin Spawned!");
					SpawnCoin ();
					coinSpawned = true;
				}
				rb2d.velocity = Vector2.zero;
				animator.SetInteger ("Vertical", 0);
				animator.SetInteger ("Horizontal",0);
				animator.SetTrigger ("Death");
			}
			break;
		}
	}

	protected virtual bool InSight () {
		hit = Physics2D.CircleCast (transform.position, 3f, Vector2.zero, 0f, playerLayer);

		if (hit.transform != null) {
			pursue = false;
			StopCoroutine (movement);
			return true;
		}

		return false;
	}

	protected void CalculateAngleForAnim() {
		rb2d.velocity = Vector2.zero;
		float angleBetween = AngleBetweenVector2 (transform.position, player.transform.position);

		if (angleBetween >= 45 && angleBetween < 135) {
			vertical = 1;
			horizontal = 0;
		} else if (angleBetween >= 135 || angleBetween < -135) {
			vertical = 0;
			horizontal = -1;
		} else if (angleBetween >= -135 && angleBetween < -45) {
			vertical = -1;
			horizontal = 0;
		} else if (angleBetween >= -45 && angleBetween < 45) {
			vertical = 0;
			horizontal = 1;
		}
			
		animator.SetInteger ("Vertical", vertical);
		animator.SetInteger ("Horizontal",horizontal);
	}

	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
		Vector2 difference = vec2 - vec1;
		float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
		return Vector2.Angle (Vector2.right, difference) * sign;
	}
	private void SetMoveDirection(int horizontal, int vertical) {
		animator.SetInteger ("Vertical", vertical);
		animator.SetInteger ("Horizontal",horizontal);
		rb2d.velocity = new Vector2 (horizontal * speed, vertical * speed);
	}

	private IEnumerator Movement() {
		int newMoveIndex;
		Vector3 enemyPosition = gameObject.transform.position;

		while (gameObject.activeInHierarchy && !pursue) {
			newMoveIndex = Random.Range (1, 6 + GameManager.instance.level);


			if (newMoveIndex != moveIndex) {
				moveIndex = newMoveIndex;
			}
			else {
				while(newMoveIndex == moveIndex) {
					newMoveIndex = Random.Range (1, 6 + GameManager.instance.level);
				}
			}
				

			switch (newMoveIndex) {

			case 1: //Move Up
				{
					if (enemyPosition.y >= 7 + boundary.position.y)
						SetMoveDirection (0, -1);
					else 
						SetMoveDirection (0, 1);
				}
				break;

			case 2: //Move Down
				{
					if (enemyPosition.y <= -7 + boundary.position.y)
						SetMoveDirection (0, 1);
					else 
						SetMoveDirection (0, -1);
				}
				break;

			case 3: //Move Left
				{
					if (enemyPosition.x <= -7 + boundary.position.x) 
						SetMoveDirection (1, 0);
					else 
						SetMoveDirection (-1, 0);
				}
				break;

			case 4: //Move Right
				{
					if (enemyPosition.x >= 7 + boundary.position.x) 
						SetMoveDirection (-1, 0);
					else 
						SetMoveDirection (1, 0);
				}
				break;

			case 5: //Idle
				{
					SetMoveDirection (0, 0);
				}
				break;

			default: //Pursue Player
				{
					currentState = enemyState.pursuing;
					rb2d.velocity = Vector2.zero;
					pursue = true;
				}
				break;
			}

			yield return new WaitForSeconds (Random.Range(minimumWaitTime, maximumWaitTime));
		}
	}

	public void StartFloating() {
		StartCoroutine ("FloatUp");
		Destroy (gameObject, 1f);
	}

	private IEnumerator FloatUp() {
		Vector3 position = transform.position;
		float t = 0.0f;
		while (t < 1.0f) {
			transform.position = new Vector3 (position.x, Mathf.Lerp (position.y, position.y + 1f, t), 0f);
			t += 0.5f * Time.deltaTime;
			yield return null;
		}
	}

	private void CheckLocation() {
		Vector3 enemyPosition = gameObject.transform.position;
		if (enemyPosition.x >= 7 + boundary.position.x || enemyPosition.x <= -7 + boundary.position.x || 
			enemyPosition.y >= 7 + boundary.position.y || enemyPosition.y <= -7 + boundary.position.y) {
			print ("Reinit movement!");
			StopCoroutine (movement);
			movement = Movement ();
			StartCoroutine (movement);
		}
	}

	private void SpawnCoin() {
			Instantiate (coin, transform.position, Quaternion.identity);
	}

	public void SetBoundary (Transform boundary) {
		this.boundary = boundary;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			StopCoroutine (movement);
			animator.SetInteger ("Vertical", 0);
			animator.SetInteger ("Horizontal", 0);
			inRange = true;
			Debug.Log ("Player in Range!");
		} 
		else if (other.CompareTag ("PlayerBullet")) {
			base.TakeDamage (player.GetComponent<Player>().strength);
		}

		else if (other.CompareTag ("Ultimate")) {
			base.TakeDamage (player.GetComponent<Player>().strength * 3);
		}

		else {
			StopCoroutine (movement);
			movement = Movement ();
			StartCoroutine (movement);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			inRange = false;
			Debug.Log ("Player out of Range!");
		}
	}
}
