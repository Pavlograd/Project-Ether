using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadPortal : MonoBehaviour
{
    public int indexPortal = 0;
    public int roomConnected = 0;
    public int portalConnected = 0;
    PortalsLinkManager _portalsLinkManager;

    void Start()
    {
        _portalsLinkManager = GameObject.Find("PortalsLink").GetComponent<PortalsLinkManager>();

        if (indexPortal == 0)
        {
            ChangeCurrentPortal();
        }
    }

    public void ChangeCurrentPortal()
    {
        _portalsLinkManager.ChangeCurrentPortal(this);
    }
}
