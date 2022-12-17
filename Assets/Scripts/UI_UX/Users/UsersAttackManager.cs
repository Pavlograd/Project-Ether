using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
public class UsersAttackManager : MonoBehaviour
{
    private GameObject _level;
    [SerializeField] private DonjonInfo _donjonInfo;
    [SerializeField] private GameObject _levelSelect;
    [SerializeField] private Text _mobs;
    [SerializeField] private Text _traps;
    [SerializeField] private SetScrollviewUsers _setScrollViewCampaign;
    public List<DonjonClass> donjons;
    public List<string> names;
    // TODO CHANGE THAT LATER MAURIN
    string fileContents;
    string nameSelected = "";

    private void Awake()
    {
        string username = API.GetUser().username;

        _levelSelect.SetActive(false);

        API_Users users = API.GetUsers();

        Debug.Log(users.users.Count);

        foreach (API_User user in users.users)
        {
            if (user.username != username)
            {
                API_Donjon objectDonjon = API.GetUserDonjon(user.username);

                if (objectDonjon != null)
                {
                    DonjonClass donjonClass = JsonUtility.FromJson<DonjonClass>(objectDonjon.data);

                    // Check if donjon has the requirements
                    if (donjonClass.tested)
                    {
                        Debug.Log(user.username);
                        Debug.Log(objectDonjon.data);
                        donjons.Add(donjonClass);
                        names.Add(user.username);
                    }
                }
            }
        }

        _setScrollViewCampaign.InitScrollView();
    }

    public void LoadDonjonInfo(Text buttonText)
    {
        int index = names.FindIndex((x) => x == buttonText.text);

        nameSelected = buttonText.text;

        _levelSelect.SetActive(true);

        DonjonData donjonData = _donjonInfo.GetDonjonInfoFromClass(donjons[index]);

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
        if (!API.RemoveEnergy()) return; // Not enough energy

        // Set path as name of player selected
        // DonjonLoaderV2 Start handle the rest
        CrossSceneInfos.donjonPath = nameSelected;

        FindObjectOfType<LevelLoader>().LoadLevel("NewDonjon");
    }

    private int CountOccurenceForMobs(List<MobData> list, ElementaryType valueToFind)
    {
        return ((from temp in list where temp.elementaryType.Equals(valueToFind) select temp).Count());
    }
}
