using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {

	GameObject tire;
	MeshRenderer scoreRender;
	AudioSource scoreSound;
	//MeshRenderer markerRender;
	TextMesh scoreText;
	string tireType;
	TestTire testTire;

	BoxCollider scoreTrigger;
	GameObject jumpPoint;

	bool canPlaceScore = false;

	// Use this for initialization
	void Start () {
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		testTire =	tire.GetComponent<TestTire>();
		scoreText = GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<TextMesh> ();
		scoreRender = GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<MeshRenderer> ();
		scoreSound = GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<AudioSource> ();
		scoreTrigger = GameObject.FindGameObjectWithTag ("ScoreTrigger").GetComponent<BoxCollider> ();
		jumpPoint = GameObject.FindGameObjectWithTag ("JumpPoint");
		tireType = tire.GetComponent<TestTire> ().tireType;
	}


	void OnTriggerEnter(Collider other) {
		if (other == scoreTrigger) {
			canPlaceScore = true;
		}
	}

	void OnCollisionEnter(Collision collision) {


		if (canPlaceScore) {
			canPlaceScore=false;

			float highscore = SaveLoad.LoadFloat(tireType + "_Highscore");

			float distance = Vector3.Distance (
				jumpPoint.transform.position, 
				tire.transform.position);


			Vector3 tirePos = tire.transform.position;
			tirePos.y+=3f;

			GameObject.FindGameObjectWithTag ("ScoreText").transform.position = tirePos;
			scoreSound.Play();
			scoreText.text = distance.ToString();
			scoreRender.enabled = true;

			testTire.currentScore = distance;

			if(distance > highscore){
			SaveLoad.SaveFloat(tireType + "_Highscore", distance);
				testTire.highscore = distance;
			}

		}

	}

	// Update is called once per frame
	void Update () {

	}
}
