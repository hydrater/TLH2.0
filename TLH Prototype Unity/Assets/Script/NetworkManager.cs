using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	void Awake() 
	{
        DontDestroyOnLoad(transform.gameObject);
    }
}
