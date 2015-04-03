using UnityEngine;
using System.Collections;

public class Flyer : MonoBehaviour {

	public float eventTime;
	public int eventDay;
	DayNight dayCycle;
	bool isActive = true;
	bool isSigned = false;
	PlayerHub hubPlayer;
	GameObject highlight;
	GameObject highlightGroup;

	// Use this for initialization
	void Start () {
		dayCycle = GameObject.Find ("Sun").GetComponent<DayNight> ();
		hubPlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ();
		highlight = gameObject.GetComponentInChildren<BoxCollider>().gameObject;
		highlightGroup = GameObject.Find (highlight.name + "/Group");
		highlightGroup.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (dayCycle.timeHour >= eventTime && isActive && dayCycle.Day >= eventDay) {
			Rigidbody fRB = gameObject.AddComponent<Rigidbody>();
			fRB.drag = 1.5f;
			isActive = false;
		}

		if (!isActive && gameObject.transform.position.y < -1)
			Destroy (gameObject);

		if (hubPlayer.isViewing) {
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)){
				//Debug.Log(hit.transform.gameObject);
				if(hit.transform.gameObject == highlight){
					highlightGroup.SetActive(true);
					if(Input.GetMouseButtonDown(0)){
						Vector3 flyerPos = transform.position;
						flyerPos.z -= 0.35f;
					   	hubPlayer.boardViewingPos = flyerPos;
						//hubPlayer.signUpButton.SetActive(true);
					}
				}else{
					highlightGroup.SetActive(false);
				}
			}

		}

	}


}
