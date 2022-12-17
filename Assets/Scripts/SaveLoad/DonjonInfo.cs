using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;

// SCRIPT CONTAINING INFOS OF THE DONJON
// USE IT TO GET INFOS OF THE DONJON FOR MENUS
// DON'T USE IT TO PLAY IN THE DONJON
// USE LoadDonjon.cs INSTEAD


public class DonjonInfo : MonoBehaviour
{
    public DonjonData GetDonjonInfo(string fileContents)
    {
        DonjonClass donjon = JsonUtility.FromJson<PlayerClass>(fileContents).donjon;

        return GetDonjonInfoFromClass(donjon);
    }

    public DonjonData GetDonjonInfoFromClass(DonjonClass donjon)
    {
        DonjonData donjonData = new DonjonData();

        donjonData.mobsData = GetMobsData(donjon.rooms);
        donjonData.trapsData = GetTrapsData(donjon.rooms);
        return donjonData;
    }

    private List<MobData> GetMobsData(List<RoomClass> rooms)
    {
        List<MobData> mobsData = new List<MobData>();

        for (int i = 0; i < rooms.Count; i++)
        {
            foreach (TileClass item in rooms[i].mobs)
            {
                MobData mobData = new MobData();

                mobData.position = new Vector2(item.x, item.y);
                mobData.prefabName = item.name;
                mobData.elementaryType = ElementaryType.NORMAL;
                mobsData.Add(mobData);
            }
        }
        return mobsData;
    }

    private List<TrapData> GetTrapsData(List<RoomClass> rooms)
    {
        List<TrapData> trapsData = new List<TrapData>();

        for (int i = 0; i < rooms.Count; i++)
        {
            foreach (TileClass item in rooms[i].mobs)
            {
                TrapData trapData = new TrapData();

                trapData.position = new Vector2(item.x, item.y);
                trapData.prefabName = item.name;
                trapsData.Add(trapData);
            }
        }
        return trapsData;
    }
}

public struct DonjonData
{
    public List<MobData> mobsData;
    public List<TrapData> trapsData;
}

public struct MobData
{
    public string prefabName;
    public Vector2 position;
    public ElementaryType elementaryType;
}

public struct TrapData
{
    public string prefabName;
    public Vector2 position;
}