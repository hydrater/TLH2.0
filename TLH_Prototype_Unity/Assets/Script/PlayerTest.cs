using UnityEngine;
using System.Collections;


public class PlayerTest : Photon.MonoBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	GameObject[] objectsToDisable;

	[SerializeField]
	GameObject playerName;

	//Camera sceneCamera;

	// Use this for initialization
	void Start () {
		if (!photonView.isMine) {
			for (int i = 0; i < componentsToDisable.Length; i++) {
				componentsToDisable[i].enabled = false;
			}

			for (int i = 0; i < objectsToDisable.Length; i++) {
				objectsToDisable[i].gameObject.SetActive(false);
			}
		}
		if (photonView.isMine) {
			playerName.gameObject.SetActive (false);
		}
		/*else {
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive(false);
			}
		}*/
	}
	/*void OnDisable(){
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}
	}*/

}

