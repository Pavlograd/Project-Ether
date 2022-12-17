using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentTexture : MonoBehaviour
{
    [SerializeField] Image image;
    private SandBoxManager sandBoxManager;

    void Start()
    {
        sandBoxManager = GameObject.Find("DonjonManager").GetComponent<SandBoxManager>();
    }

    public void ChangeTexture()
    {
        sandBoxManager.ChangeTexture(image.sprite);
    }
}
