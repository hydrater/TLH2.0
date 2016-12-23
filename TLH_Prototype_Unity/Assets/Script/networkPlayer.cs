using UnityEngine;
using System.Collections;

public class networkPlayer : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
		}
	
	}



	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();

		}
	}

	/*[PunRPC]
	void networkWeaponSwitch(){
		Transform numOfPlayerCam = GameObject.Find("PlayerTest").transform;
		for (int i = PhotonNetwork.playerList.Length - 1; i >= 0; --i) {
			playerWeaponHanders.Insert
		}
		for (int i = PhotonNetwork.playerList.Length - 1; i >= 0; --i) {
			numOfPlayerCam.transform.GetChild(i).GetComponentInChildren<vp_FPCamera> ();
				// first off, always store all weapons contained under our main FPS camera (if any)
				List<vp_Weapon> camWeapons = null;
				vp_FPCamera camera = transform.GetComponentInChildren<vp_FPCamera>();
				if (camera != null)
				{
					camWeapons = GetWeaponList(Camera.main.transform);
					if ((camWeapons != null) && (camWeapons.Count > 0))
						m_WeaponLists.Add(camWeapons);

				}

				List<vp_Weapon> allWeapons = new List<vp_Weapon>(transform.GetComponentsInChildren<vp_Weapon>());

				// if the camera weapons were all the weapons we have, return
				if ((camWeapons != null) && (camWeapons.Count == allWeapons.Count))
				{
					Weapons = m_WeaponLists[0];
					return;
				}

				// identify every unique gameobject that holds weapons as direct children
				List<Transform> weaponContainers = new List<Transform>();
				foreach (vp_Weapon w in allWeapons)
				{

					if ((camera != null) && camWeapons.Contains(w))
						continue;
					if (!weaponContainers.Contains(w.Parent))
						weaponContainers.Add(w.Parent);
				}

				// create one weapon list for every container found
				foreach (Transform t in weaponContainers)
				{
					List<vp_Weapon> weapons = GetWeaponList(t);
					DeactivateAll(weapons);
					m_WeaponLists.Add(weapons);
				}

				// abort and disable weapon handler in case no weapons were found
				if (m_WeaponLists.Count < 1)
				{
					Debug.LogError("Error (" + this + ") WeaponHandler found no weapons in its hierarchy. Disabling self.");
					enabled = false;
					return;
				}

				// start out with the first weapon list by default. on a 1st person
				// player, this would typically be the weapons stored under the camera
				Weapons = m_WeaponLists[0];
		}
	}*/
}
