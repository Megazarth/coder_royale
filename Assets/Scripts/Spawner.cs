using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject itemBox;
	public GameObject exit;
	public bool lastRoom;
	public GameObject remy;
	public GameObject[] enemies;

	private Transform boxHolder;
	private Transform enemyHolder;
	private List <Vector3> gridPositions;

	void Awake() {
		lastRoom = false;
		gridPositions = new List <Vector3> ();
	}

	void Start () {
	}

	public void Init() {
		int childNumber = gameObject.transform.Find ("Board").childCount;

		for (int i = 0; i < childNumber; i++) {
			gameObject.transform.Find ("Board").GetChild (i).gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}

		if (lastRoom) {
			Instantiate (exit, transform.position, Quaternion.identity);
		}
		else {
			InitialiseList ();
			SpawnEnemies ();
			SpawnBoxes ();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Clears our list gridPositions and prepares it to generate a new board.
	void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();

		//Loop through x axis (columns).
		for(int x = -6 + (int) transform.position.x; x <= 6 + (int) transform.position.x; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 6 + (int) transform.position.y; y  >= -6 + (int) transform.position.y; y--)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}

	private void SpawnBoxes() {
		boxHolder = new GameObject ("Boxes").transform;
		boxHolder.SetParent (gameObject.transform);

		int range = Random.Range (2, 3 + GameManager.instance.level);

		for(int i = 1; i <= range; i++)
		{
			GameObject instance = Instantiate (itemBox, RandomPosition(), Quaternion.identity);
			instance.transform.SetParent (boxHolder);
		}
	}

	private void SpawnEnemies() {
		Debug.Log (transform.position);
		enemyHolder = new GameObject ("Enemies").transform;
		enemyHolder.SetParent (gameObject.transform);

		int range = Random.Range (2, 4 + GameManager.instance.level);

		for(int i = 1; i <= range; i++)
		{
			GameObject enemy = enemies [Random.Range (0, enemies.Length)];
			enemy.GetComponent<Enemy> ().SetBoundary (transform);
			GameObject instance = Instantiate (enemy, RandomPosition(), Quaternion.identity);
			instance.transform.SetParent (enemyHolder);
		}

		if (GameManager.instance.level >= 3) {
			int remyRange = Random.Range (0, 2 + GameManager.instance.level);
			for(int i = 1; i <= remyRange; i++)
			{
				GameObject enemy = remy;
				enemy.GetComponent<Remy> ().SetBoundary (transform);
				GameObject instance = Instantiate (enemy, RandomPosition(), Quaternion.identity);
				instance.transform.SetParent (enemyHolder);
			}
		}

	}

	private Vector3 RandomPosition ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);

		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
		Vector3 randomPosition = gridPositions[randomIndex];

		//Remove the entry at randomIndex from the list so that it can't be re-used.
		gridPositions.RemoveAt (randomIndex);

		//Return the randomly selected Vector3 position.
		return randomPosition;
	}

	public Transform GetEnemyHolder() {
		return enemyHolder;
	}
}
