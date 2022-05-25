using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;

public class DonjonInfo : MonoBehaviour
{
    public DonjonData GetDonjonInfo(string fileContents)
    {
        DonjonClass donjon = JsonUtility.FromJson<PlayerClass>(fileContents).donjon;

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
            string[] mobsJson = rooms[i].mobs.Split(',');

            for (int y = 0; y < mobsJson.Length - 1; y++)
            {
                MobData mobData = new MobData();
                string[] mobJson = mobsJson[y].Split(':');

                mobData.position = new Vector2(0f, 0f);
                mobData.position.x = float.Parse(mobJson[0], CultureInfo.InvariantCulture);
                mobData.position.y = float.Parse(mobJson[1], CultureInfo.InvariantCulture);
                mobData.prefabName = mobJson[2];
                mobData.elementaryType = (ElementaryType)Enum.Parse(typeof(ElementaryType), mobJson[3]);
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
            string[] trapsJson = rooms[i].traps.Split(',');

            for (int y = 0; y < trapsJson.Length - 1; y++)
            {
                TrapData trapData = new TrapData();
                string[] trapJson = trapsJson[y].Split(':');

                trapData.position = new Vector2(0f, 0f);
                trapData.position.x = float.Parse(trapJson[0], CultureInfo.InvariantCulture);
                trapData.position.y = float.Parse(trapJson[1], CultureInfo.InvariantCulture);
                trapData.id = int.Parse(trapJson[2]);
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
    public int id;
    public Vector2 position;
}