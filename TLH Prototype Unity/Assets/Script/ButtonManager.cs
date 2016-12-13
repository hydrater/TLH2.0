using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void startBtn(string startGame)
    {
        SceneManager.LoadScene(startGame);
    }
}
