using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BasicEntity {

	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	private enum State {
		up, down, left, right
	}

	private State currentState;

	private Animator animator;
	private Rigidbody2D rb2d;

	private float myTime = 0.0F;
	public float fireRate = 0.5F;
	public float bulletSpeed = 500f;
	private float nextFire = 0.5F;

	public GameObject bullet;
	public GameObject ultimate;

	public Slider playerHealth;
	public Slider playerEnergy;
	public Text playerCoin;

	public AudioClip fireSound;
	public AudioClip ultiSound;
	public AudioClip heal;
	public AudioClip energize;

	private AudioSource audioSource;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Start() {
		Init ();
	}	
	// Update is called once per frame
	void Update () {
		if (base.isAlive)
			ForPlayer ();
		else {
			animator.SetInteger ("Vertical", 0);
			animator.SetInteger ("Horizontal",0);
			animator.SetTrigger ("Death");
		}	
	}

	private void ForPlayer() {
		int horizontal = 0;
		int vertical = 0;
		myTime = myTime + Time.deltaTime;

		if (!Input.GetButtonDown("Fire1")) {
			horizontal = (int) Input.GetAxisRaw ("Horizontal");
			vertical = (int) Input.GetAxisRaw ("Vertical");
		}

		rb2d.velocity = new Vector2 (horizontal * base.speed, vertical * base.speed);


		if (horizontal == 0 || vertical == 0) {
			animator.SetInteger ("Horizontal", horizontal);
			animator.SetInteger ("Vertical", vertical);
		}

		if (horizontal < 0 && vertical == 0)
			currentState = State.left;
		else if (horizontal > 0 && vertical == 0)
			currentState = State.right;
		else if (vertical < 0 && horizontal == 0)
			currentState = State.down;
		else if (vertical > 0 && horizontal == 0)
			currentState = State.up;

		switch (currentState) {
		case State.up:
			{
				float x = 0f;
				float y = 0.5f;
					Fire (x, y, Vector2.up);
			}
			break;
		case State.down:
			{
				float x = 0f;
				float y = -0.5f;
					Fire (x, y, Vector2.down);
			}
			break;
		case State.left:
			{
				float x = -0.5f;
				float y = -0.25f;
					Fire (x, y, Vector2.left);
			}
			break;
		case State.right:
			{
				float x = 0.5f;
				float y = -0.25f;
					Fire (x, y, Vector2.right);
			}
			break;
		}
	}

	public void StartFloating() {
		StartCoroutine ("FloatUp");
	}

	private IEnumerator FloatUp() {
		Vector3 position = transform.position;
		float t = 0.0f;
		while (t < 1.0f) {
			transform.position = new Vector3 (position.x, Mathf.Lerp (position.y, position.y + 1f, t), 0f);
			t += 0.5f * Time.deltaTime;
			yield return null;
		}
		GameManager.instance.StartGameOver ();
	}

	public void Init() {
		currentState = State.down;
		transform.position = Vector3.zero;

		base.health = PlayerStatus.instance.health;
		playerHealth.maxValue = PlayerStatus.instance.maxHealth;
		playerHealth.value = base.health;


		playerEnergy.maxValue = PlayerStatus.instance.maxEnergy;
		playerEnergy.value = PlayerStatus.instance.energy;

		playerCoin.text = PlayerStatus.instance.money.ToString();
	}

	private void Fire (float x, float y, Vector2 direction) {
		if (Input.GetButton ("Fire1") && myTime > nextFire) {
			animator.SetTrigger ("Attack");
			audioSource.PlayOneShot (fireSound, 0.3f);
			nextFire = myTime + fireRate;
			GameObject bulletPrefab = Instantiate (bullet, 
				new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z), 
				Quaternion.identity);
			bulletPrefab.GetComponent<Rigidbody2D> ().AddForce (direction * bulletSpeed);
		}
		else if (Input.GetButton ("Fire2") && myTime > nextFire && PlayerStatus.instance.energy >= 10f) {
			animator.SetTrigger ("Attack");
			audioSource.PlayOneShot (ultiSound, 0.3f);
			nextFire = myTime + fireRate;
			GameObject bulletPrefab = Instantiate (ultimate, 
				new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z), 
				Quaternion.identity);
			bulletPrefab.GetComponent<Rigidbody2D> ().AddForce (direction * bulletSpeed);

			PlayerStatus.instance.energy -= 10;
			playerEnergy.value = PlayerStatus.instance.energy;
		}

	}

	public override void TakeDamage(float damage) {
		base.TakeDamage (damage);
		playerHealth.value = base.health;
		PlayerStatus.instance.health = base.health;
	}

	public void addMoney(int money) {
		PlayerStatus.instance.money += money;
		playerCoin.text = PlayerStatus.instance.money.ToString();
	}

	public void addLife(float life) {
		base.health += life;
		if (base.health > PlayerStatus.instance.maxHealth)
			base.health = PlayerStatus.instance.maxHealth;
		playerHealth.value = base.health;
		PlayerStatus.instance.health = base.health;
		audioSource.PlayOneShot (heal, 0.3f);
	}

	public void addEnergy(float energy) {
		PlayerStatus.instance.energy += energy;
		if (PlayerStatus.instance.energy > PlayerStatus.instance.maxEnergy)
			PlayerStatus.instance.energy = PlayerStatus.instance.maxEnergy;
		playerEnergy.value = PlayerStatus.instance.energy;
		audioSource.PlayOneShot (energize, 0.3f);
	}
}
