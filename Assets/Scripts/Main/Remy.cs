using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remy : BasicEntity {

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

	private float myTime = 0.0F;
	public float fireRate = 0.5F;
	public float bulletSpeed = 500f;
	private float nextFire = 0.5F;

	public GameObject bullet;

	private bool moveStarted;

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
	void Start () {
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		audioSource = GetComponent<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		inRange = false;
		currentState = enemyState.free;
		StartCoroutine (movement);

		if (GameManager.instance.level > 1) {
			base.health += base.health * (int)Mathf.Log(GameManager.instance.level, 2f);
			base.strength += base.strength * (int)Mathf.Log(GameManager.instance.level, 2f);
		}
	}

	// Update is called once per frame
	void Update () {
		myTime = myTime + Time.deltaTime;
		switch (currentState) {

		case enemyState.free:
			{
				CheckLocation ();

				if (InSight ()) {
					StopCoroutine (movement);
					moveStarted = false;
					rb2d.velocity = Vector2.zero;
				}
					
				if (!moveStarted && !InSight()) {
					moveStarted = true;
					StartCoroutine (movement);
					Debug.Log ("Reset Movement");
				}
					

				if (!base.isAlive)
					currentState = enemyState.death;
			}
			break;
		case enemyState.pursuing:
			{
				if (pursue) 
					CalculateAngleForAnim ();

				else
					currentState = enemyState.free;

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

	protected bool InSight () {
		
		if (InLineOfSight(1, 0)) {
			CalculateAngleForLook ();
			Fire (0.4f, -0.28f, Vector2.right);
			return true;
		}

		else if (InLineOfSight(-1, 0)) {
			CalculateAngleForLook ();
			Fire (0.4f, -0.28f, Vector2.left);
			return true;
		}

		else if (InLineOfSight(0, 1)) {
			CalculateAngleForLook ();
			Fire (0.4f, -0.28f, Vector2.up);
			return true;
		}

		else if (InLineOfSight(0, -1)) {
			CalculateAngleForLook ();
			Fire (0.4f, -0.28f, Vector2.down);
			return true;
		}

		return false;
	}

	protected void CalculateAngleForLook() {
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

	protected void CalculateAngleForAnim() {
		rb2d.velocity = Vector2.zero;
		float angleBetween = AngleBetweenVector2 (transform.position, player.transform.position);
		bool attacking = false;

		if (angleBetween >= 45 && angleBetween < 135) {
			vertical = 1;
			horizontal = 0;
			if (InLineOfSight(horizontal, vertical)) {
				attacking = true;
				Fire (0f, 0.5f, Vector2.up);
			}
		} else if (angleBetween >= 135 || angleBetween < -135) {
			vertical = 0;
			horizontal = -1;
			if (InLineOfSight(horizontal, vertical)) {
				attacking = true;
				Fire (-0.4f, -0.28f, Vector2.left);
			}
		} else if (angleBetween >= -135 && angleBetween < -45) {
			vertical = -1;
			horizontal = 0;
			if (InLineOfSight(horizontal, vertical)) {
				attacking = true;
				Fire (0f, -0.5f, Vector2.down);
			}
		} else if (angleBetween >= -45 && angleBetween < 45) {
			vertical = 0;
			horizontal = 1;
			if (InLineOfSight(horizontal, vertical)) {
				attacking = true;
				Fire (0.4f, -0.28f, Vector2.right);
			}
		}

		if (!attacking) {
			animator.SetInteger ("Vertical", vertical);
			animator.SetInteger ("Horizontal",horizontal);
			transform.position = Vector2.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
		}

	}

	private void Fire (float x, float y, Vector2 direction) {
		if (myTime > nextFire) {
			animator.SetTrigger ("Attack");
			audioSource.Play ();
			nextFire = myTime + fireRate;
			GameObject bulletPrefab = Instantiate (bullet, 
				new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z), 
				Quaternion.identity);
			bulletPrefab.GetComponent<Rigidbody2D> ().AddForce (direction * bulletSpeed);
		}
		else {
			animator.SetInteger ("Vertical", 0);
			animator.SetInteger ("Horizontal",0);
		}
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

	private bool InLineOfSight(int horizontal, int vertical) {
		Vector2 start = new Vector2 (transform.position.x, transform.position.y);
		Vector2 end = new Vector2 (start.x + (horizontal * 3f), start.y + (vertical * 3f));
		RaycastHit2D hit = Physics2D.Linecast (start, end, playerLayer);

		if (hit.transform != null) {
			pursue = false;
			StopCoroutine (movement);
			return true;
		}

		return false;
	}

	private IEnumerator Movement() {
		int newMoveIndex;
		Vector3 enemyPosition = gameObject.transform.position;
		pursue = false;
		moveStarted = true;
		while (gameObject.activeInHierarchy) {
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
		moveStarted = false;
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
		int range = Random.Range (1, 2 + GameManager.instance.level);

		for (int i = 0; i <= range; i++) {
			Instantiate (coin, transform.position, Quaternion.identity);
		}			
	}

	public void SetBoundary (Transform boundary) {
		this.boundary = boundary;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			return;
		} 
		else if (other.CompareTag ("PlayerBullet")) {
			base.TakeDamage (player.GetComponent<Player>().strength);
		}
		else {
			StopCoroutine (movement);
			movement = Movement ();
			StartCoroutine (movement);
		}
	}
		
}
