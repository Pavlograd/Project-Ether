using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using UnityEditor.Animations;

public class PlayerAnimationController
{
    public Animator playerController;
    public Animator staffController;
    public Sprite preview;
}

public class playerInformation : MonoBehaviour
{
    public TMP_Text level;
    public TMP_Text PlayerName;

    public static playerInformation instance;

    private TextureHolder textureHolderObj = null;

    PlayerClass _pc = null;

    private List<GameObject> texturesList = new List<GameObject>(); 

    [SerializeField] private List<Animator> _playerControllers;
    [SerializeField] private List<Animator> _staffControllers;
    [SerializeField] private List<Sprite> _previews;
    [SerializeField] private Image _menuPreview;
    [SerializeField] private Image _editMenuPreview;

    private Dictionary<int, PlayerAnimationController> _pacList = new Dictionary<int, PlayerAnimationController>();

    private void Start()
    {
        int i = 0;
        
        if (_playerControllers.Count != _staffControllers.Count && _playerControllers.Count != _previews.Count)
            print("Error to throw : different count of player/staff controllers and previews");
        for (int j = 0; j < _playerControllers.Count; ++j)
        {
            PlayerAnimationController pac = new PlayerAnimationController();
            if (_playerControllers[j])
                pac.playerController = _playerControllers[j];
            if (_playerControllers[j])
                pac.staffController = _staffControllers[j]; 
            pac.preview = _previews[j];
            _pacList[j] = pac;
        }
        
        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);
            _pc = JsonUtility.FromJson<PlayerClass>(fileContents);
            CrossSceneInfos.skinId = _pc.skinId;
        }
        else

        {
            API_User_Datas playerData = API.GetUserDatas();

            if (playerData != null)
            {
                _pc = new PlayerClass();
                _pc.skinId = playerData.skinId;
                CrossSceneInfos.skinId = playerData.skinId;
            }
        }

        if (_menuPreview && _editMenuPreview)
        {
            // Debug.Log(CrossSceneInfos.skinId);
            while (i < _pacList.Count)
            {
                // Debug.Log(i);
                if (i.ToString() == CrossSceneInfos.skinId)
                    break;
                ++i;
            }
            _menuPreview.sprite = _pacList[i].preview;
            _editMenuPreview.sprite = _pacList[i].preview;
            if (_pc.skinId != CrossSceneInfos.skinId)
            {
                CrossSceneInfos.skinId = _pc.skinId;
                SetPreview();
            }
        }

        // Does the file exist?
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            level.text = $"Level: {player.level.ToString()}";
            PlayerName.text = player.name.ToString();
        }
        

    }

    public Dictionary<int, PlayerAnimationController> GetPacList()
    {
        return _pacList;
    }

    public void SetPreview()
    {
        int i = 0;
        string id = CrossSceneInfos.skinId;
        _pc.skinId = id;
        API.SetPlayerSkinId(id);

        string json = JsonUtility.ToJson(_pc);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        
        while (i < _playerControllers.Count)
        {
            if (i.ToString() == CrossSceneInfos.skinId)
                break;
            ++i;
        }
        if (i > _pacList.Count)
        {
            _menuPreview.sprite = _pacList[0].preview;
            _editMenuPreview.sprite = _pacList[0].preview;
        }
        else
        {
            _menuPreview.sprite = _pacList[i].preview;
            _editMenuPreview.sprite = _pacList[i].preview;
        }
    }
}