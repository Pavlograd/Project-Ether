using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class MoneyInventory : MonoBehaviour
{
    public int crystalCount;
    //public string name;
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public Sprite emptyItem;
    public TextMeshProUGUI crystalCountText;
    public int type;

    public static MoneyInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Error: use only one inventory instance in the scene.");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        itemImage.sprite = emptyItem;
        if (type == 1)
        {
            itemName.text = "crystal";
        } else if (type == 2) {
            itemName.text = "cash";
        } else
        {
            itemName.text = "mentoring";
        }
        crystalCountText.text = crystalCount.ToString();
        setCrystal();
        crystalCountText.text = crystalCount.ToString();
    }

    public void AddCrystal(int count)
    {
        crystalCount += count;
        crystalCountText.text = crystalCount.ToString();
    }

    public void removeCrystal(int count)
    {
        if (count > crystalCount)
        {
            return;
        }
        crystalCount -= count;
        crystalCountText.text = crystalCount.ToString();
    }

    public void setCrystal()
    {
        API_User_Datas objectUserDatas = API.GetUserDatas();
        // Does the file exist?

        
        if (type == 1)
        {
            crystalCountText.text = objectUserDatas.crystal.ToString();
            crystalCount = objectUserDatas.crystal;
        }
        else if (type == 2)
        {
            crystalCountText.text = objectUserDatas.cash.ToString();
            crystalCount = objectUserDatas.cash;
        }
        else
        {
            crystalCountText.text = objectUserDatas.mentoring.ToString();
            crystalCount = objectUserDatas.mentoring;

        }
    }

    public void saveCrystal()
    {
        if (type == 1)
        {
            API.SaveCrystal(crystalCount);
        }
        else if (type == 2)
        {
            API.SaveCash(crystalCount);
        }
        else
        {
            API.SaveMentoring(crystalCount);
        }
    }

    [System.Serializable]
    public class Save
    {
        public int level;
        public int crystal;
        public int cash;
        public int mentoring;
        public int textureSlot;
        public int maxTextureSlot;
        public bool hasDoneTutorial;
    }
}
