using UnityEngine;
using System.Collections;

public class TireMachine : MonoBehaviour {

	public bool shouldPrint;
	public TireSpawn printSpawn;

	// Use this for initialization
	void Start () {
		if( SaveLoad.LoadInt("ShouldPrint") == 1){
			shouldPrint = true;
			SaveLoad.SaveInt("ShouldPrint", 0);
		} else {
			shouldPrint = false;
		}

		if (shouldPrint) {
			printSpawn.spawnTire (SaveLoad.LoadString ("PrintTire"));
		}
	}


	//END CLASS------------------------------------------------------------------------------------------------------
}
