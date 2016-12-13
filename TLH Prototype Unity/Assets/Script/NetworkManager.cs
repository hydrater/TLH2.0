using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
	const string VERSION = "Prototype";

	//Connect to server upon game start
	void Awake() 
	{
        DontDestroyOnLoad(transform.gameObject);
		if (!PhotonNetwork.connected)
					PhotonNetwork.ConnectUsingSettings(VERSION);
		Cursor.lockState =  CursorLockMode.Confined;
		Cursor.visible = false;
    }

	//application fires a protocol to this function to link together
    public void onSceneChange()
    {
		switch(SceneManager.GetActiveScene().name)
		{
			default:

			break;

			case "Lobby":
				//PhotonNetwork.playerList[0].name;
			break;
		}

    }

    //Linked to the button
    public void enterRoom()
    {
		string roomName = GameObject.Find("InputField").GetComponent<InputField>().text;
		PhotonNetwork.playerName = GameObject.Find("InputField").GetComponent<InputField>().text;
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 6;
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    //For debugging purposes only
	void Update()
	{
		//Debug.Log();
	}
}
