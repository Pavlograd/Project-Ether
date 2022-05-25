using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTrapManager : MonoBehaviour
{
    public int id;

    public int GetIdIsland() {
        // Island grandParent = gameObject.transform.parent.parent.GetComponent<Island>();
        // return grandParent.id;
//TODO ADAPT 2D
        return 0;
    }

    public int GetIdTrapHitBox() {
        // HitBoxTrapManager parent = gameObject.transform.parent.GetComponent<HitBoxTrapManager>();
        // return parent.id;
//TODO ADAPT 2D
        return 0;
    }
}
