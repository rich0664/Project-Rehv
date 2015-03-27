using UnityEngine;
using System.Collections;

public class Addon : MonoBehaviour {

	public GameObject parent;
	public int addonIndex;

	string poString;
	Color startcolor;
	FixedJoint fJoint;

	public void setParent(){
		//fJoint = transform.gameObject.GetComponent<FixedJoint> ();
		Transform transParent = parent.transform;
		transform.SetParent (transParent, true);
		//fJoint.connectedBody = parent.GetComponent<Rigidbody> ();
	}

}
