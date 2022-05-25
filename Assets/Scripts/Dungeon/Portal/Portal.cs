using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Portal : MonoBehaviour
{
    public int room = 0;
    public int portal = 0;
    public int roomConnected = 0;
    public int portalConnected = 0;
    private GameObject _portals;

    void Start()
    {
        _portals = GameObject.Find("Portals");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerMovementManager player = collider.GetComponent<PlayerMovementManager>();

            if (player.ableToTeleport)
            {
                player.ableToTeleport = false;
                Teleport(player);
            }
            else
            {
                player.ableToTeleport = true;
            }
        }
    }

    void Teleport(PlayerMovementManager player)
    {
        Portal[] childScripts = _portals.GetComponentsInChildren<Portal>();

        for (int i = 0; i < childScripts.Length; i++)
        {
            Portal portal = childScripts[i];

            if (portal.room == roomConnected && portal.portal == portalConnected)
            {
                player.transform.position = portal.transform.position;

                GameObject.Find("Main Camera").transform.position = CalculateCenterPosition() + new Vector3(0, 0, -10.0f);

                GridGraph gg = AstarPath.active.data.gridGraph; // This get the first (and unique) graph

                gg.center = portal.transform.position; // Set cenetr to current player positions

                AstarPath.active.Scan(); // Rescan to manually update graph

                Debug.Log("Teleported");
                return;
            }
        }

        Debug.Log("Not teleportated");
    }

    Vector3 CalculateCenterPosition() // Will calculate the center of the room
    {
        Vector3 position = Vector3.zero;
        Vector3 otherPortalPosition = Vector3.zero;

        Portal[] childScripts = _portals.GetComponentsInChildren<Portal>();

        for (int i = 0; i < childScripts.Length; i++)
        {
            Portal portal = childScripts[i];

            if (portal.room == roomConnected && portal.portal == portalConnected)
            {
                position = portal.transform.position;
            }
            else if (portal.room == roomConnected) // Another portal in the same room IMPORTANT for calibration
            {
                otherPortalPosition = portal.transform.position;
            }
        }

        if (otherPortalPosition == Vector3.zero) // Last room
        {
            GameObject boss = GameObject.Find("Boss") != null ? GameObject.Find("Boss") : GameObject.Find("BossNotPlayer");
            otherPortalPosition = boss.transform.position;
        }

        float offsetToCenter = GetOffsetToCenter(position, otherPortalPosition);

        float x = position.x - (position.x % 100.0f) + offsetToCenter;
        float y = position.y - (position.y % 100.0f) + offsetToCenter;

        return new Vector3(x, y, 0.0f);
    }

    float GetOffsetToCenter(Vector3 position, Vector3 otherPortalPosition)
    {
        float moduloXPosition = position.x % 100.0f;
        float moduloYPosition = position.y % 100.0f;
        float moduloXOtherPosition = otherPortalPosition.x % 100.0f;
        float moduloYOtherPosition = otherPortalPosition.y % 100.0f;

        if (moduloXPosition == moduloXOtherPosition)
        {
            return moduloXPosition;
        }
        else if (moduloYPosition == moduloYOtherPosition)
        {
            return moduloYPosition;
        }
        else if (moduloXPosition == moduloYOtherPosition)
        {
            return moduloXPosition;
        }
        else if (moduloYPosition == moduloXOtherPosition)
        {
            return moduloYPosition;
        }

        return 0.0f;
    }
}
