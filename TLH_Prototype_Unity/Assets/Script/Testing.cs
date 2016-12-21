using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Testing : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	/*[SerializeField]
	GameObject[] objectsToDisable;*/

	//Camera sceneCamera;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			for (int i = 0; i < componentsToDisable.Length; i++) {
				componentsToDisable[i].enabled = false;
			}

			/*for (int i = 0; i < objectsToDisable.Length; i++) {
				objectsToDisable[i].gameObject.SetActive(false);
			}*/
		} /*else {
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
