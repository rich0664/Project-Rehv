using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	public float minWindSpeed;
	public float maxWindSpeed;
	public float minVerticalWindSpeed;
	public float maxVerticalWindSpeed;
	public float minTurbulance;
	public float maxTurbulance;

	public float tireWindScaleDown = 0.015f;

	public Cloth windSock;

	public ConstantForce sockTugForce;

	public ConstantForce tireForce;


	// Update is called once per frame
	void Update () {

		if (tireForce == null) {

			tireForce = GameObject.FindGameObjectWithTag("MainTire").GetComponent<ConstantForce>();

			tireForce.enabled = true;

			RandomizeWind();

		}

	}

	void RandomizeWind(){

		Vector3 wind = new Vector3(0,0,0);
		Vector3 turbulance = new Vector3(0,0,0);
		Vector3 tireWind = new Vector3(0,0,0);

		wind.x = Random.Range (minWindSpeed, maxWindSpeed);
		wind.z = Random.Range (minWindSpeed, maxWindSpeed);
		wind.y = Random.Range (minVerticalWindSpeed, maxVerticalWindSpeed);

		turbulance.x = Random.Range (minTurbulance, maxTurbulance);
		turbulance.y = Random.Range (minTurbulance, maxTurbulance);
		turbulance.z = Random.Range (minTurbulance, maxTurbulance);

		tireWind.x = Random.Range (minWindSpeed * tireWindScaleDown, maxWindSpeed * tireWindScaleDown);
		tireWind.z = Random.Range (minWindSpeed * tireWindScaleDown, maxWindSpeed * tireWindScaleDown);
		tireWind.y = Random.Range (minVerticalWindSpeed * tireWindScaleDown, maxVerticalWindSpeed * tireWindScaleDown);


		if (Random.Range (0, 2) == 1){
			wind.x = -wind.x;
			tireWind.x = -tireWind.x;
		}
		if (Random.Range (0, 4) == 1){
			wind.y = -wind.y;
			tireWind.y = -tireWind.y;
		}
		if (Random.Range (0, 2) == 1){
			wind.z = -wind.z;
			tireWind.z = -tireWind.z;
		}


		windSock.externalAcceleration = wind;
		windSock.randomAcceleration = turbulance;
		sockTugForce.force = wind/4;

		tireForce.force = tireWind;


		/*Debug.Log (windSock.externalAcceleration.x + " : "  + 
		           windSock.externalAcceleration.y + " : "  + 
		           windSock.externalAcceleration.z);*/

	}



}
