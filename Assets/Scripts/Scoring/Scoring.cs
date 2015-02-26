using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {

	GameObject tire;
	MeshRenderer scoreRender;
	TextMesh scoreText;

	public BoxCollider scoreTrigger;
	//public GameObject startPoint;

	bool canPlaceScore = false;

	// Use this for initialization
	void Start () {
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		scoreText = GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<TextMesh> ();
		scoreRender = GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<MeshRenderer> ();
	}


	void OnTriggerEnter(Collider other) {
		if (other == scoreTrigger) {
			canPlaceScore = true;
		}
	}

	void OnCollisionEnter(Collision collision) {


		if (canPlaceScore) {
			canPlaceScore=false;

			float distance = Vector3.Distance (
				GameObject.FindGameObjectWithTag ("ScoreText").transform.position, 
				tire.transform.position);

			Vector3 tirePos = tire.transform.position;
			tirePos.y+=3f;

			GameObject.FindGameObjectWithTag ("ScoreText").transform.position = tirePos;
			scoreText.text = distance.ToString();
			scoreRender.enabled = true;
		}

	}

	// Update is called once per frame
	void Update () {

	}
}
