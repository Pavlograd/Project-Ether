using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEditor : MonoBehaviour
{
    public void LoadSceneEditor()
    {
        CrossSceneInfos.CrossSceneInformation = GameObject.Find("Load").GetComponent<LoadRoom>().activeRoom.ToString();

        Debug.Log(CrossSceneInfos.CrossSceneInformation);
        SceneManager.LoadScene("RoomEditor");
    }
}
