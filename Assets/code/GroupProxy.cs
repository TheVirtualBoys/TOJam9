using UnityEngine;
using System.Collections;

public class GroupProxy : MonoBehaviour {

	public Transform character1;
	public Transform character2;
	public Transform character3;

	public Vector3 char1Pos;
	public Vector3 char2Pos;
	public Vector3 char3Pos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		character1.transform.position = transform.position + char1Pos;
		character2.transform.position = transform.position + char2Pos;
		character3.transform.position = transform.position + char3Pos;
	}
}
