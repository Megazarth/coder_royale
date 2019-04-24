using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseConn : MonoBehaviour {

	public static FirebaseConn instance;

	private DatabaseReference reference;
	[HideInInspector]public List<string> names;
	[HideInInspector]public List<int> scores;

	[HideInInspector]public bool hasConnection;
	[HideInInspector]public bool scoreTaskDone;
	[HideInInspector]public bool nameTaskDone;

	void Awake() {
		names = new List<string>();
		scores = new List<int>();
		hasConnection = false;
		scoreTaskDone = false;
		nameTaskDone = false;
		if (instance == null) {
			instance = this;
		}
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start() {

	}


	// Update is called once per frame
	void Update () {
		
	}

	public void InitConnection() {
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
			Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
			hasConnection = true;
			// Set this before calling into the realtime database.
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://coderroyale.firebaseio.com/");

			// Get the root reference location of the database.
			reference = FirebaseDatabase.DefaultInstance.RootReference;

			if (reference != null) {
				Debug.Log ("Database Connection Established!");
				GetName ();
				GetScore ();
			}
		}
	}

	public void SetValueChangeListeners() {
		FirebaseDatabase.DefaultInstance.GetReference ("name").ValueChanged += NameValueChanged;
		FirebaseDatabase.DefaultInstance.GetReference ("score").ValueChanged += ScoreValueChanged;
	}

	void GetName() {
		reference.Child ("name").GetValueAsync ().ContinueWith (task => {
						if (task.IsFaulted) {
							Debug.Log(task.Exception.ToString());
						}
						else if (task.IsCompleted) {
							nameTaskDone = true;
							DataSnapshot dataSnapshot = task.Result;

							foreach (var obj in dataSnapshot.Children) {
								names.Add(obj.Value.ToString());
							}
						}
					});
	}

	void GetScore() {
		reference.Child("score").GetValueAsync().ContinueWith(task => {
						if (task.IsFaulted) {
							Debug.Log(task.Exception.ToString());
						}
						else if (task.IsCompleted) {
							scoreTaskDone = true;
							DataSnapshot dataSnapshot = task.Result;

							foreach (var obj in dataSnapshot.Children) {
					int score = int.Parse(obj.Value.ToString());
								scores.Add(score);
					Debug.Log("Happen");
							}
						}
					});
	}

	void SetName() {
		reference.Child ("name").GetValueAsync ().ContinueWith (task => {
			if (task.IsFaulted) {
				Debug.Log(task.Exception.ToString());
			}
			else if (task.IsCompleted) {
				DataSnapshot dataSnapshot = task.Result;
				int index = 0;
				foreach (var obj in dataSnapshot.Children) {
					reference.Child("name").Child(obj.Key.ToString()).SetValueAsync(names[index++]);
				}
			}
		});
	}

	void SetScore() {
		reference.Child("score").GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted) {
				Debug.Log(task.Exception.ToString());
			}
			else if (task.IsCompleted) {
				DataSnapshot dataSnapshot = task.Result;
				int index = 0;
				foreach (var obj in dataSnapshot.Children) {
					reference.Child("score").Child(obj.Key.ToString()).SetValueAsync(scores[index++].ToString());
				}
			}
		});
	}

	public void AddScore(string name, int score) {
		for (int i = 0; i < scores.Count; i++) {
			if (score > scores[i]) {
				names.Insert (i, name);
				scores.Insert(i, score);

				names.RemoveAt (names.Count - 1);
				scores.RemoveAt (scores.Count - 1);

				break;
			}
		}

		SetName ();
		SetScore ();
			
	}

	void ScoreValueChanged(object sender, ValueChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		DataSnapshot dataSnapshot = args.Snapshot;
		scores.Clear ();

		foreach (var obj in dataSnapshot.Children) {
			
		}
	}

	void NameValueChanged(object sender, ValueChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		DataSnapshot dataSnapshot = args.Snapshot;
		names.Clear ();

		foreach (var obj in dataSnapshot.Children) {
			
		}
	}


		
}
