using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusScreenController : MonoBehaviour {
	public Text health;
	public Text energy;
	public Text strength;

	public Text healthCost;
	public Text energyCost;
	public Text strengthCost;

	public Text coinNumber;

	private int currentHealth;
	private int currentEnergy;
	private int currentStrength;

	private int currentHealthCost;
	private int currentEnergyCost;
	private int currentStrengthCost;

	private int currentCoinNumber;


	// Use this for initialization
	void Start () {
		health.text = PlayerStatus.instance.maxHealth.ToString();
		energy.text = PlayerStatus.instance.maxEnergy.ToString();
		strength.text = PlayerStatus.instance.strength.ToString();

		healthCost.text = PlayerStatus.instance.healthCost.ToString();
		energyCost.text = PlayerStatus.instance.energyCost.ToString();
		strengthCost.text = PlayerStatus.instance.strengthCost.ToString();

		coinNumber.text = PlayerStatus.instance.money.ToString();

		currentHealth = (int) PlayerStatus.instance.maxHealth;
		currentEnergy = (int) PlayerStatus.instance.maxEnergy;
		currentStrength = (int) PlayerStatus.instance.strength;

		currentHealthCost = (int) PlayerStatus.instance.healthCost;
		currentEnergyCost = (int) PlayerStatus.instance.energyCost;
		currentStrengthCost = (int) PlayerStatus.instance.strengthCost;

		currentCoinNumber = PlayerStatus.instance.money;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void plusHealth() {
		if (currentCoinNumber >= currentHealthCost) {
			currentHealth += 10;
			currentCoinNumber -= currentHealthCost;
			currentHealthCost *= 2;

			health.text = currentHealth.ToString ();
			coinNumber.text = currentCoinNumber.ToString ();
			healthCost.text = currentHealthCost.ToString ();
		}
	}

	public void minusHealth() {
		if (currentHealth > PlayerStatus.instance.maxHealth) {
			currentHealth -= 10;
			currentHealthCost /= 2;
			currentCoinNumber += currentHealthCost;

			health.text = currentHealth.ToString ();
			coinNumber.text = currentCoinNumber.ToString ();
			healthCost.text = currentHealthCost.ToString ();
		}
	}

	public void plusEnergy() {
		if (currentCoinNumber >= currentEnergyCost) {
			currentEnergy += 5;
			currentCoinNumber -= currentEnergyCost;
			currentEnergyCost *= 2;

			energy.text = currentEnergy.ToString ();
			coinNumber.text = currentCoinNumber.ToString ();
			energyCost.text = currentEnergyCost.ToString ();
		}
	}

	public void minusEnergy() {
		if (currentEnergy > PlayerStatus.instance.maxEnergy) {
			currentEnergy -= 5;
			currentEnergyCost /= 2;
			currentCoinNumber += currentEnergyCost;

			energy.text = currentEnergy.ToString ();
			coinNumber.text = currentCoinNumber.ToString ();
			energyCost.text = currentEnergyCost.ToString ();
		}
	}

	public void plusStrength() {
		if (currentCoinNumber >= currentStrengthCost) {
			currentStrength += 1;
			currentCoinNumber -= currentStrengthCost;
			currentStrengthCost *= 2;

			strength.text = currentStrength.ToString ();
			coinNumber.text = currentCoinNumber.ToString ();
			strengthCost.text = currentStrengthCost.ToString ();
		}
	}

	public void minusStrength() {
		if (currentStrength > PlayerStatus.instance.strength) {
			currentStrength -= 1;
			currentStrengthCost /= 2;
			currentCoinNumber += currentStrengthCost;

			strength.text = currentStrength.ToString ();
			coinNumber.text = currentCoinNumber.ToString ();
			strengthCost.text = currentStrengthCost.ToString ();
		}
	}

	public void AcceptChange() {
		PlayerStatus.instance.LevelUp (currentHealth, currentEnergy, currentStrength, currentCoinNumber);
	}
}
