using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour {

	public static PlayerStatus instance = null;



	public float health;
	public float energy;
	public float strength;

	[HideInInspector]public float maxHealth;
	[HideInInspector]public float maxEnergy;

	[HideInInspector]public int healthCost;
	[HideInInspector]public int energyCost;
	[HideInInspector]public int strengthCost;

	[HideInInspector]public int money;

	void Awake() {
		print ("Player Status Awake!");
		if (instance == null) {
			instance = this;
		}
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		Init ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Init() {
		healthCost = 1;
		energyCost = 2;
		strengthCost = 5;
	}

	public void LevelUp(int maxHealth, int maxEnergy, int strength, int money) {
		this.maxHealth = (float) maxHealth;
		this.maxEnergy = (float) maxEnergy;
		this.strength =  (float) strength;
		this.money = money;
		SceneManager.LoadScene ("Main");
	}
}
