using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyInfos : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SetEnergy", 0f, 61f);
        SetEnergy();
    }

    void SetEnergy()
    {
        text.text = API.GetEnergy().ToString();
    }

    public void RemoveEnergy()
    {
        API.RemoveEnergy(10);
        SetEnergy();
    }

    public void CheckIfEnoughEnergy()
    {
        bool response = API.RemoveEnergy();

        if (response)
        {
            levelLoader.LoadLevel("RandomGeneration");
        }
    }
}
