using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public GameObject normalTile;

	public GameObject itemBox;

	public GameObject[] bridges;

	public GameObject[] wallTilesLeft;
	public GameObject[] wallTilesRight;
	public GameObject[] wallTilesUp;
	public GameObject[] wallTilesDown;

	public GameObject wallCornerUpLeft;
	public GameObject wallCornerUpRight;
	public GameObject wallCornerDownLeft;
	public GameObject wallCornerDownRight;

	private Transform boardHolder;
	private Transform bridgeHolder;
	private Transform boxHolder;

	private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.

	//Clears our list gridPositions and prepares it to generate a new board.
	void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();

		//Loop through x axis (columns).
		for(int x = -7 + (int) transform.position.x; x <= 7 + (int) transform.position.x; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 7 + (int) transform.position.y; y  >= -7 + (int) transform.position.y; y--)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup() {
		boardHolder = new GameObject ("Board").transform;
		boardHolder.SetParent(gameObject.transform);


		for (int i = 0; i < wallTilesLeft.Length; i++) {
			wallTilesLeft [i].GetComponent<SpriteRenderer> ().sprite = TileTexture.instance.grassWallLeft[Random.Range(0, TileTexture.instance.grassWallLeft.Length)];
		}
		for (int i = 0; i < wallTilesRight.Length; i++) {
			wallTilesRight [i].GetComponent<SpriteRenderer> ().sprite = TileTexture.instance.grassWallRight[Random.Range(0, TileTexture.instance.grassWallRight.Length)];
		}
		for (int i = 0; i < wallTilesUp.Length; i++) {
			wallTilesUp [i].GetComponent<SpriteRenderer> ().sprite = TileTexture.instance.grassWallUp[Random.Range(0, TileTexture.instance.grassWallUp.Length)];
		}
		for (int i = 0; i < wallTilesDown.Length; i++) {
			wallTilesDown [i].GetComponent<SpriteRenderer> ().sprite = TileTexture.instance.grassWallDown[Random.Range(0, TileTexture.instance.grassWallDown.Length)];
		}

		for(int x = -7 + (int) transform.position.x; x <= 7 + (int) transform.position.x; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 7 + (int) transform.position.y; y  >= -7 + (int) transform.position.y; y--)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.

				if (gameObject.name == "Entry Room") {
					GameObject toInstantiate = normalTile;
					GameObject instance = Instantiate(toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);

				}
				else {
					GameObject toInstantiate = TileTexture.instance.grassFloor;
					toInstantiate.GetComponent<SpriteRenderer> ().color = Color.grey;
					GameObject instance = Instantiate(toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);
				}

			}
		}
	}
		
	private void SpawnBridges() {
		bridgeHolder = new GameObject ("Bridges").transform;
		bridgeHolder.SetParent (gameObject.transform);

		for (int i = 0; i < bridges.Length; i++) {
			GameObject instance = Instantiate (bridges [i], gameObject.transform.position, Quaternion.identity);
			instance.transform.SetParent (bridgeHolder);
		}
	}

	// Use this for initialization
	void Start () {
		BoardSetup ();
		InitialiseList ();
		SpawnBridges ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
