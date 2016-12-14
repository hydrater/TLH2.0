using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour
{
	const string VERSION = "Prototype";

	//Connect to server upon game start
	void Awake() 
	{
		SceneManager.activeSceneChanged += onSceneChange;
        DontDestroyOnLoad(transform.gameObject);
		if (!PhotonNetwork.connected)
					PhotonNetwork.ConnectUsingSettings(VERSION);
		
    }

	//Function is called every time the scene is changed
	public void onSceneChange(Scene previousScene, Scene newScene)
    {
		switch(SceneManager.GetActiveScene().name)
		{
			default:

			break;

			case "Lobby":
			photonView.RPC("updatePlayerDisplay", PhotonTargets.All);
			break;

			case "PrototypeLevel":
				Cursor.lockState =  CursorLockMode.Confined;
				Cursor.visible = false;
			break;
		}

    }

    //Linked to the button
    public void enterRoom()
    {
		string roomName = GameObject.Find("Room").GetComponent<InputField>().text;
		PhotonNetwork.playerName = GameObject.Find("Username").GetComponent<InputField>().text;
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 6;
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    //Change scene once connected to room
	void OnJoinedRoom()
	{
		SceneManager.LoadScene("Lobby");
	}

	void OnPhotonJoinRoomFailed()
	{
		//ROOM IS FULL ERROR
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer other)
	{
		updatePlayerDisplay();
	}

    //For debugging purposes only
	void Update()
	{
		//Debug.Log(SceneManager.GetActiveScene().name);
	}

	[PunRPC]
	void updatePlayerDisplay()
	{
		Transform playerList = GameObject.Find("Player List").transform;
		for(byte i = 0; i<PhotonNetwork.playerList.Length; ++i)
		{
			playerList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PhotonNetwork.playerList[i].name;
		}
	}

	[PunRPC]
	void lobbyReady()
	{
		
	}
}
