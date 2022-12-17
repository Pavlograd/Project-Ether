using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentMob : MonoBehaviour
{
    [SerializeField] string mobName;
    private SandBoxManager sandBoxManager;

    void Start()
    {
        sandBoxManager = GameObject.Find("DonjonManager").GetComponent<SandBoxManager>();
    }

    public void ChangeMob()
    {
        sandBoxManager.PlaceTemporaryMob(mobName);
    }
}
