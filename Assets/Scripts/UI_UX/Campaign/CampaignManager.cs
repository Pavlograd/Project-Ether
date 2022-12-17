using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class CampaignManager : MonoBehaviour
{
    private GameObject _level;
    [SerializeField] private DonjonInfo _donjonInfo;
    [SerializeField] private GameObject _levelSelect;
    [SerializeField] private Text _mobs;
    [SerializeField] private Text _traps;
    [SerializeField] private SetScrollViewCampaign _setScrollViewCampaign;
    public List<TextAsset> levels;
    // TODO CHANGE THAT LATER MAURIN
    string fileContents;

    private void Awake()
    {
        _setScrollViewCampaign.InitScrollView();
    }

    public void LoadDonjonInfo(Text buttonText)
    {
        int levelNumber = int.Parse(buttonText.text.Replace("Level ", "")) - 1;

        if (levels.Count < levelNumber)
        {
            _levelSelect.SetActive(false);
            return;
        }
        _levelSelect.SetActive(true);

        fileContents = levels[levelNumber].text; // To load donjon later

        DonjonData donjonData = _donjonInfo.GetDonjonInfo(levels[levelNumber].text);

        string mobsInfo = "";
        while (donjonData.mobsData.Count > 0)
        {
            ElementaryType elementaryTypeToFind = donjonData.mobsData[0].elementaryType;
            mobsInfo += CountOccurenceForMobs(donjonData.mobsData, elementaryTypeToFind).ToString() + " " + elementaryTypeToFind + "\n";
            donjonData.mobsData.RemoveAll(temp => temp.elementaryType == elementaryTypeToFind);
        }
        if (mobsInfo == "")
            mobsInfo = "No mobs";
        _mobs.text = mobsInfo;
        _traps.text = donjonData.trapsData.Count.ToString();
    }

    public void StartDonjon()
    {
        // TODO CHANGE THAT LATER MAURIN
        // WHY ME ?????

        if (!API.RemoveEnergy()) return; // Not enough energy

        string path = Application.persistentDataPath + "/" + Random.Range(0, 256) + "randomLevel.json";
        File.WriteAllText(path, fileContents);
        CrossSceneInfos.donjonPath = path;
        FindObjectOfType<LevelLoader>().LoadLevel("NewDonjon");
    }

    private int CountOccurenceForMobs(List<MobData> list, ElementaryType valueToFind)
    {
        return ((from temp in list where temp.elementaryType.Equals(valueToFind) select temp).Count());
    }
}
