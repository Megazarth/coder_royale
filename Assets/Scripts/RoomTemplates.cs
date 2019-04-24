using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {

	public GameObject[] bottomRooms;
	public GameObject[] topRooms;
	public GameObject[] leftRooms;
	public GameObject[] rightRooms;

	public List<GameObject> rooms;

	private int roomCount;

	void Start() {
		roomCount = rooms.Count;
		StartCoroutine (RoomFinishGenerating ());
	}

	private void SpawnExit() {
		int index;
		if (rooms.Count > 5 + GameManager.instance.level) 
			index = Random.Range (1, 6);
		else
			index = Random.Range (1, rooms.Count);
		Debug.Log ("Index = " + index);
		GameObject exitRoom = rooms [index];
		exitRoom.GetComponent<Spawner> ().lastRoom = true;
	}

	private IEnumerator RoomFinishGenerating() {
		do {
			if (roomCount != rooms.Count) {
				roomCount = rooms.Count;
			}
			yield return new WaitForSeconds (0.3f);
		} while (roomCount != rooms.Count);
		SpawnExit ();
	}
}
