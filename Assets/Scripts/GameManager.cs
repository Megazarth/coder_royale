using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[HideInInspector] public int level = 0;

	[HideInInspector]public static GameManager instance = null;

	public GameObject tutorial;

	public float levelStartDelay = 3f;
	 
	private AudioSource audioSource;
	private Text levelText;
	private GameObject levelImage;

	public AudioClip defeat;

	void Awake() {
		print ("Game Manager Awake!");
		if (instance == null) {
			instance = this;
		}
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Return)){
			SceneManager.LoadScene("Status");
		}
			
		
		if (Input.GetKey (KeyCode.Escape)) {
			//GameManager.instance.StartGameOver();
		}
			
	}

	private void HideLevelImage() {
		levelImage.SetActive (false);
		if (GameManager.instance.level == 1)
			StartCoroutine (tutorial.GetComponent<Tutorial> ().StartTutorial ());
		else
			Destroy (tutorial);
	}

	void InitGame() {
			levelImage = GameObject.Find ("LevelImage");
			levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
			levelText.text = "Level " + level;
			levelImage.SetActive (true);
			Invoke ("HideLevelImage", levelStartDelay);
	}
		
	public void BackToTitle() {
		Destroy (GameObject.Find ("PlayerStatus"));
		Destroy (gameObject);
		SceneManager.LoadScene ("Start");
	}

	public void StartGameOver() {
		audioSource.Stop ();
		audioSource.clip = defeat;
		audioSource.Play ();
		SceneManager.LoadScene ("Game Over");
	}

	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		print ("OnLevelFinishedLoading");
		//Add one to our level number.
		if (scene.name == "Main") {
			level++;
			Cursor.visible = false;
			InitGame();
		}

		else
			Cursor.visible = true;
	}

	void OnEnable() {
		//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		//Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
		//Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
}
