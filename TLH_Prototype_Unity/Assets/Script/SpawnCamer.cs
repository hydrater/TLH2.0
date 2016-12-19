using UnityEngine;
using System.Collections;

public class SpawnCamer : MonoBehaviour {

	public GameObject FPScamera = null;

	// Use this for initialization
	void Start () {
		Instantiate (FPScamera, this.transform.position, this.transform.rotation);
	}
}
