using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadPortals : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _button;
    [SerializeField] private LoadRoom _roomLoader;

    void DestroyAllChild(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void LoadPortalsMenuFromSave()
    {
        DestroyAllChild(_content);
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        string[] portals = new string[1]; //player.donjon.rooms[_roomLoader.activeRoom].portals.Split(',');

        Vector3 position = new Vector3(250.0f, 850.0f, 0.0f);
        RectTransform contentTransform = _content.GetComponent<RectTransform>();

        for (int x = 0; x < portals.Length - 1; x++)
        {
            string[] portal = portals[x].Split(':');
            GameObject newButton = Instantiate(_button, position, Quaternion.identity, contentTransform);

            // Set Index Room inn save to button
            newButton.GetComponent<ButtonLoadPortal>().indexPortal = x;
            newButton.GetComponent<ButtonLoadPortal>().roomConnected = int.Parse(portal[2]);
            newButton.GetComponent<ButtonLoadPortal>().portalConnected = int.Parse(portal[3]);

            // Set room's name as text for button
            newButton.transform.Find("Text").GetComponent<Text>().text = "Portal " + (x + 1).ToString();

            position.y += -200.0f;
        }
    }
}
