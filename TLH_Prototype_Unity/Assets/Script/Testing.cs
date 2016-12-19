using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Testing : NetworkBehaviour {
	
	GameObject Tester;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<vp_FPController> ().enabled = true;
			GetComponent<vp_FPInput> ().enabled = true;
			GetComponent<vp_FPPlayerEventHandler> ().enabled = true;
			GetComponent<vp_WeaponHandler> ().enabled = true;
			GetComponent<vp_SimpleCrosshair> ().enabled = true;
			GetComponent<vp_FootstepManager> ().enabled = true;
			GetComponent<vp_SimpleHUD> ().enabled = true;
			GetComponent<vp_PainHUD> ().enabled = true;
			GetComponent<vp_FPPlayerDamageHandler> ().enabled = true;
			Camera.main.transform.position = this.transform.position - this.transform.forward*10 + this.transform.up * 5;
			Camera.main.transform.LookAt(this.transform.position);
			Camera.main.transform.parent = this.transform;
		}
	}

}
