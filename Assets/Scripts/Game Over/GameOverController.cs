using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameOverController : MonoBehaviour {

	public GameObject input;
	public InputField nameInput;
	public Text nameBoard;
	public Text scoreBoard;
	public GameObject board;
	public GameObject message;

	private string nameText;
	private int levelText;

	void Awake() {
		nameInput.characterLimit = 8;
	}

	// Use this for initialization
	void Start () {
		board.SetActive (false);
		message.SetActive (false);
		StartCoroutine ("Starting");
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator Starting() {
		input.SetActive (false);
		yield return new WaitForSeconds (3f);
		GameObject.Find ("GameOverText").SetActive (false);
		input.SetActive (true);
	}

	void InitScoreBoard() {
		board.SetActive (true);

		nameBoard.text = "";
		scoreBoard.text = "";
		Debug.Log(FirebaseConn.instance.scores [1]);
		if (FirebaseConn.instance.hasConnection) {
			do {
				nameBoard.text = "";
				scoreBoard.text = "";
				for (int i = 0; i < FirebaseConn.instance.scores.Count; i++) {
					Debug.Log(FirebaseConn.instance.names [i]);
					nameBoard.text += FirebaseConn.instance.names [i] + "\n";
					scoreBoard.text += FirebaseConn.instance.scores [i] + "\n";	
				} 
			} while(!FirebaseConn.instance.nameTaskDone && !FirebaseConn.instance.scoreTaskDone);

		}

		else {
			for (int i = 0; i < 5; i++) {
				Debug.Log(FirebaseConn.instance.names [i]);

				nameBoard.text += "NO DATA" + "\n";
				scoreBoard.text += "NO DATA" + "\n";	
			} 
		}
		//FirebaseConn.instance.SetValueChangeListeners ();
	}

	public void AddScoreName() {
		if (nameInput.text.Length > 0) {
			nameText = nameInput.text.ToString ();
			levelText = GameManager.instance.level;
			input.SetActive (false);
			if (FirebaseConn.instance.hasConnection) {
				FirebaseConn.instance.AddScore(nameText, levelText);
				message.SetActive (false);
				Invoke ("InitScoreBoard", 1f);
			}
			else {
				message.SetActive (true);
			}
		}
	}

	public void Retry() {
		if (FirebaseConn.instance.hasConnection) {
			FirebaseConn.instance.AddScore(nameText, levelText);
			message.SetActive (false);
			Invoke ("InitScoreBoard", 1f);
		}
		else {
			message.SetActive (true);
		}
	}

	public void SkipInput() {
		InitScoreBoard ();
	}

	public void SkipName() {
		string name = "";
		int level = GameManager.instance.level;

		FirebaseConn.instance.AddScore(name, level);
		input.SetActive (false);
		Invoke ("InitScoreBoard", 3f);
	}

	public void BackToTitle() {
		GameManager.instance.BackToTitle ();
	}
}
