using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditElement : MonoBehaviour
{
    [SerializeField] List<GameObject> buttonsTrap;
    [SerializeField] List<GameObject> buttonsMob;
    [SerializeField] List<GameObject> buttonsDoor;
    [SerializeField] List<GameObject> buttonsTexture;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(2, 0, 0); // Calculate position ui
        }
    }

    public void ShowButtons(RoomElement elementType)
    {
        HideAll();

        switch (elementType)
        {
            case RoomElement.DOOR:
                ShowList(buttonsDoor);
                break;
            case RoomElement.TRAP:
                ShowList(buttonsTrap);
                break;
            case RoomElement.MOB:
                ShowList(buttonsMob);
                break;
            default:
                ShowList(buttonsTexture);
                break;
        }
    }

    public void HideAll()
    {
        HideList(buttonsDoor);
        HideList(buttonsMob);
        HideList(buttonsTrap);
        HideList(buttonsTexture);
    }

    void HideList(List<GameObject> list)
    {
        foreach (var item in list)
        {
            item.SetActive(false);
        }
    }

    void ShowList(List<GameObject> list)
    {
        foreach (var item in list)
        {
            item.SetActive(true);
        }
    }
}
