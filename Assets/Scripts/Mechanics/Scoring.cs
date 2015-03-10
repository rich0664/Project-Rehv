﻿using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {

	GameObject tire;
	MeshRenderer scoreRender;
	AudioSource scoreSound;
	//MeshRenderer markerRender;
	TextMesh scoreText;
	string tireType;
	UniversalTire uniTire;

	BoxCollider scoreTrigger;
	GameObject jumpPoint;

	bool canPlaceScore = false;

	// Use this for initialization
	void Start () {
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		uniTire =	tire.GetComponent<UniversalTire>();
		scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh> ();
		scoreRender = GameObject.Find ("ScoreText").GetComponent<MeshRenderer> ();
		scoreSound = GameObject.Find("ScoreText").GetComponent<AudioSource> ();
		scoreTrigger = GameObject.Find ("ScoreTrigger").GetComponent<BoxCollider> ();
		jumpPoint = GameObject.Find ("JumpPoint");
		tireType = GameObject.Find ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn;
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

			LaunchingUI.currentScore = distance;

			if(distance > highscore){
			SaveLoad.SaveFloat(tireType + "_Highscore", distance);
				LaunchingUI.highscore = distance;
			}

		}

	}

	// Update is called once per frame
	void Update () {

	}
}