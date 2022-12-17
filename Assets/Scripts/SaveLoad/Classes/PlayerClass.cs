using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerClass
{
    public uint id = 0;
    public string name = "username";
    public ushort level = 1;
    public string skinId = "0";
    public int crystal = 1000;
    public int cash = 1000;
    public int mentoring = 1000;
    public int textureSlot = 2;
    public int maxTextureSlot = 10;
    public bool hasDoneTutorial = false;
    public bool hasDoneMainTutorial = false;
    public SocialClass social = new SocialClass();
    public GearClass gear = new GearClass();
    public InventoryClass inventory = new InventoryClass();
    public DonjonClass donjon = new DonjonClass();
    public List<DonjonClass> tower = new List<DonjonClass>(); //have to be moved next sprint
    public List<PublicShopClass> publicShop = new List<PublicShopClass>(); //have to be moved next sprint 

    public void initValues()
    {
        //islands = new List<IslandClass>();
    }
}