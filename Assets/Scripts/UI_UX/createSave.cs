using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class createSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PlayerClass player = API.GetUserDataForSave();
        //string json = JsonUtility.ToJson(player);
        //File.WriteAllText(Application.persistentDataPath + "/save.json", json);

    }

    public void SaveCampaign(string LvL, string roomParam)
    {

        // Now add to save
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
        RoomClass room = new RoomClass();
        DonjonClass donjon = new DonjonClass();
        //room.room = roomParam;
        room.name = LvL;
        donjon.rooms.Add(room);
        player.tower.Add(donjon);

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public void Create()
    {
        PlayerClass player = new PlayerClass();
        player.initValues();
        string json = JsonUtility.ToJson(player);

        Debug.Log(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        SaveCampaign("LvL1", "{Room:{Walls:[0:0:wall_corner_front_left,0:1:wall_corner_front_left,0:2:wall_corner_front_left,0:3:wall_corner_front_left,1:0:wall_corner_front_left,1:3:wall_corner_front_left,2:0:wall_corner_front_left,2:3:wall_corner_front_left,3:0:wall_corner_front_left,3:1:wall_corner_front_left,3:2:wall_corner_front_left,3:3:wall_corner_front_left,],Ground:[0:0:floor_1,0:1:floor_1,0:2:floor_1,1:0:floor_1,1:1:floor_1,1:2:floor_1,2:0:floor_1,2:1:floor_1,2:2:floor_1,],}}");
        SaveCampaign("LvL2", "{Room:{Walls:[1:3:wall_corner_front_left,1:4:wall_corner_front_left,1:5:wall_corner_front_left,2:2:wall_corner_front_left,2:3:wall_corner_front_left,2:5:wall_corner_front_left,2:6:wall_corner_front_left,3:2:wall_corner_front_left,3:6:wall_corner_front_left,4:2:wall_corner_front_left,4:6:wall_corner_front_left,5:2:wall_corner_front_left,5:6:wall_corner_front_left,6:2:wall_corner_front_left,6:3:wall_corner_front_left,6:5:wall_corner_front_left,6:6:wall_corner_front_left,7:3:wall_corner_front_left,7:4:wall_corner_front_left,7:5:wall_corner_front_left,],Ground:[1:3:floor_1,1:4:floor_1,1:5:floor_1,2:3:floor_1,2:4:floor_1,2:5:floor_1,3:3:floor_1,3:4:floor_1,3:5:floor_1,4:3:floor_1,4:4:floor_1,4:5:floor_1,5:3:floor_1,5:4:floor_1,5:5:floor_1,6:3:floor_1,6:4:floor_1,6:5:floor_1,7:3:floor_1,7:4:floor_1,7:5:floor_1,],}}");
        SaveCampaign("LvL3", "{Room:{Walls:[0:0:wall_corner_front_left,0:1:wall_corner_front_left,0:2:wall_corner_front_left,0:3:wall_corner_front_left,1:0:wall_corner_front_left,1:3:wall_corner_front_left,2:0:wall_corner_front_left,2:3:wall_corner_front_left,3:0:wall_corner_front_left,3:3:wall_corner_front_left,4:0:wall_corner_front_left,4:3:wall_corner_front_left,5:0:wall_corner_front_left,5:3:wall_corner_front_left,6:0:wall_corner_front_left,6:1:wall_corner_front_left,6:2:wall_corner_front_left,6:3:wall_corner_front_left,],Ground:[0:0:floor_1,0:1:floor_1,0:2:floor_1,1:0:floor_1,1:1:floor_1,1:2:floor_1,2:0:floor_1,2:1:floor_1,2:2:floor_1,3:0:floor_1,3:1:floor_1,3:2:floor_1,4:0:floor_1,4:1:floor_1,4:2:floor_1,5:0:floor_1,5:1:floor_1,5:2:floor_1,],}}");
    }
}
