using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour
{
	const string VERSION = "Prototype";
	bool isReady = false;

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
				GameObject.Find("ReadyBtn").GetComponent<Button>().onClick.AddListener(readyButton);
				GameObject.Find("LeaveBtn").GetComponent<Button>().onClick.AddListener(leaveButton);
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
		isReady = false;
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
		photonView.RPC("lobbyReady", PhotonTargets.All, isReady, PhotonNetwork.playerName);
	}


    //For debugging purposes and convenient keyboard controls
    bool tabfocus = false;
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			if (tabfocus) 
			{
				GameObject.Find ("Room").GetComponent<InputField> ().Select ();
				tabfocus = !tabfocus;
			}
			else
			{
				GameObject.Find("Username").GetComponent<InputField>().Select();
				tabfocus = !tabfocus;
			}
		}
		if (Input.GetKeyUp(KeyCode.Return))
		{
			enterRoom();
		}
		//Debug.Log();
	}

	[PunRPC]
	void updatePlayerDisplay()
	{
		Transform playerList = GameObject.Find("Player List").transform;
		for(byte i = 0; i < 6; ++i)
		{
			if (i<PhotonNetwork.playerList.Length)
			{
				playerList.transform.GetChild(i).gameObject.SetActive(true);
				playerList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PhotonNetwork.playerList[i].name;
			}
			else
			{
				playerList.transform.GetChild(i).gameObject.SetActive(false);
				break;
			}
		}
	}

	public void readyButton()
	{
		isReady = !isReady;
		photonView.RPC("lobbyReady", PhotonTargets.All, isReady, PhotonNetwork.playerName);
	}

	public void leaveButton()
	{
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("_Title");
	}

	private IEnumerator gameStart;

	//Bugs cannot unready, cannot check all
	[PunRPC]
	void lobbyReady(bool ready, string username)
	{
		byte readyAmt = 0;
		Transform playerList = GameObject.Find("Player List").transform;
		for(byte i = 0; i < PhotonNetwork.playerList.Length; ++i)
		{
			if(playerList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text == username)
			{
				playerList.transform.GetChild(i).GetComponent<Image>().color = ready ? Color.green : Color.red;
				if (playerList.transform.GetChild(i).GetComponent<Image>().color == Color.green)
					++readyAmt;
			}
		}
		if (readyAmt == PhotonNetwork.playerList.Length)
		{
			gameStart = gameStartTimer();
			StartCoroutine(gameStart);
		}
		else
		{
			StopCoroutine(gameStart);
			GameObject.Find("TimerTxt").GetComponent<Text>().text = "";
		}
	}

	IEnumerator gameStartTimer()
	{
		Text timer = GameObject.Find("TimerTxt").GetComponent<Text>();
		for (sbyte i = 5; i >= 0; --i)
		{
			timer.text = string.Format("0:0{0}", i.ToString());
			yield return new WaitForSeconds(1f);
		}
		//display loading scene
		SceneManager.LoadSceneAsync("PrototypeLevel");
	}
}
