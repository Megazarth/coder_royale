using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTexture : MonoBehaviour {

	public static TileTexture instance = null;

	public GameObject grassFloor;
	public Sprite grassCornerUpLeft;
	public Sprite grassCornerUpRight;
	public Sprite grassCornerDownLeft;
	public Sprite grassCornerDownRight;
	public Sprite[] grassWallUp;
	public Sprite[] grassWallRight;
	public Sprite[] grassWallDown;
	public Sprite[] grassWallLeft;

	public Sprite sandFloor;
	public Sprite sandCornerUpLeft;
	public Sprite sandCornerUpRight;
	public Sprite sandCornerDownLeft;
	public Sprite sandCornerDownRight;
	public Sprite[] sandWallUp;
	public Sprite[] sandWallRight;
	public Sprite[] sandWallDown;
	public Sprite[] sandWallLeft;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		if (instance != this)
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
