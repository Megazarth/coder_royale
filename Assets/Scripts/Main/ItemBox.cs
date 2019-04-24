using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : BasicEntity {

	public GameObject[] items;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!base.isAlive) {
			RandomDrop ();
			Destroy (gameObject);
		}
	}

	private void RandomDrop() {
		int index = Random.Range (0, 1 + GameManager.instance.level);
		Debug.Log (index);
		switch (index) {

		case 0:
			{
				Instantiate (items [index], transform.position, Quaternion.identity);
			}
			break;

		case 1:
			{
				Instantiate (items [index], transform.position, Quaternion.identity);
			}
			break;

		default:
			return;
		}
	}
}
