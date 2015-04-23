using UnityEngine;
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

	LineRenderer ScoreLine;

	public bool canPlaceScore = false;

	public float distance;

	// Use this for initialization
	void Awake () {

		tire = GameObject.FindGameObjectWithTag ("MainTire");
		uniTire = tire.GetComponent<UniversalTire> ();

		if (!uniTire.spawnPoint.isPrint && !uniTire.spawnPoint.isEditor) {
			scoreText = GameObject.Find ("ScoreText").GetComponent<TextMesh> ();
			scoreRender = GameObject.Find ("ScoreText").GetComponent<MeshRenderer> ();
			scoreSound = GameObject.Find ("ScoreText").GetComponent<AudioSource> ();
			scoreTrigger = GameObject.Find ("ScoreTrigger").GetComponent<BoxCollider> ();
			jumpPoint = GameObject.Find ("JumpPoint");
			tireType = GameObject.Find ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn;
			ScoreLine = GameObject.Find ("ScoreText").GetComponentInChildren<LineRenderer>();
		}
	}


	void OnTriggerEnter(Collider other) {
		if (other == scoreTrigger) {
			canPlaceScore = true;
		}
	}

	void OnCollisionEnter(Collision collision) {


		if (canPlaceScore) {
			canPlaceScore=false;

			if(GameObject.Find("CompetitionStuff") != null){
				GameObject.Find("CompetitionStuff").GetComponent<JumpCompetition>().ContinueButton.SetActive(true);
			}

			float highscore = SaveLoad.LoadFloat(tireType + "_Highscore");

			distance = Vector3.Distance (
				jumpPoint.transform.position, 
				tire.transform.position);


			Vector3 tirePos = tire.transform.position;
			RaycastHit hit;
			if (Physics.Raycast (tirePos, -Vector3.up, out hit)) {
				tirePos = hit.point;
				tirePos.y+=.25f;
			}

			if (Physics.Raycast (tirePos, -Vector3.right, out hit)) {
				ScoreLine.SetPosition(0,hit.point);
			}
			if (Physics.Raycast (tirePos, Vector3.right, out hit)) {
				ScoreLine.SetPosition(1,hit.point);
			}

			tirePos.y+=2.72f;

			scoreText.transform.position = tirePos;
			scoreText.gameObject.SetActive(true);
			scoreSound.Play();
			scoreText.text = distance.ToString();
			scoreRender.enabled = true;
			scoreText.gameObject.SetActive(true);

			LaunchingUI.currentScore = distance;

			if(distance > highscore){
			SaveLoad.SaveFloat(tireType + "_Highscore", distance);
				LaunchingUI.highscore = distance;
			}

		}

	}



}
