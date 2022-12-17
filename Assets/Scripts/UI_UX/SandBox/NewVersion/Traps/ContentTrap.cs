using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentTrap : MonoBehaviour
{
    [SerializeField] string trapName;
    private SandBoxManager sandBoxManager;

    void Start()
    {
        sandBoxManager = GameObject.Find("DonjonManager").GetComponent<SandBoxManager>();
    }

    public void ChangeTrap()
    {
        sandBoxManager.PlaceTemporaryTrap(trapName);
    }
}
