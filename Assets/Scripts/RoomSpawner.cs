using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

	public enum Direction {
		up, down, left, right
	}
	public Direction openingDirection;

	private RoomTemplates templates;
	private int rand;
	private bool spawned = false;

	public float waitTime = 5f;

	void Start() {
		Destroy (gameObject, waitTime);
		templates = GameObject.FindGameObjectWithTag ("Rooms").GetComponent<RoomTemplates> ();
		Invoke ("Spawn", Random.Range(0.2f, 0.3f));
	}

	void Spawn() {

		if (spawned == false) {
			switch (openingDirection) {
				case Direction.up:
					{
						rand = Random.Range (0, templates.topRooms.Length);
						Instantiate (templates.topRooms [rand], transform.position, Quaternion.identity);
					}
					break;
				case Direction.down:
					{
						rand = Random.Range (0, templates.bottomRooms.Length);
						Instantiate (templates.bottomRooms [rand], transform.position, Quaternion.identity);
					}
					break;
				case Direction.left:
					{
						rand = Random.Range (0, templates.leftRooms.Length);
						Instantiate (templates.leftRooms [rand], transform.position, Quaternion.identity);
					}
					break;
				case Direction.right:
					{
						rand = Random.Range (0, templates.rightRooms.Length);
						Instantiate (templates.rightRooms [rand], transform.position, Quaternion.identity);
					}
					break;
			}
			spawned = true;
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("RoomSpawnPoint")) {
			if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
				Destroy (gameObject);
			}
		}
		spawned = true;
	}

}
