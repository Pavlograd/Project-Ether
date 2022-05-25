using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryClass
{
    public List<SkillClass> skills = new List<SkillClass>();
    public List<RessourceClass> traps = new List<RessourceClass>();
    public List<RessourceClass> mobs = new List<RessourceClass>();
    public List<RessourceClass> ressources = new List<RessourceClass>();
    public List<TextureClass> textureInventory = new List<TextureClass>();
    public List<ColorClass> colorInventory = new List<ColorClass>();

}