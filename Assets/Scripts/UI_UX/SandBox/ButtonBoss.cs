using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBoss : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] private GameObject _mobs;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        AIHealthManager[] childScripts = _mobs.GetComponentsInChildren<AIHealthManager>();

        Button button = GetComponent<Button>();

        _image.color = Color.white;

        button.enabled = true;

        for (int i = 0; i < childScripts.Length; i++)
        {
            AIHealthManager myChildScript = childScripts[i];

            if (myChildScript.transform.name == "Boss" || myChildScript.transform.name == "Boss(Clone)")
            {
                Debug.Log("Disable button boss");
                _image.color = Color.grey;

                button.enabled = false;
            }
        }
    }
}
