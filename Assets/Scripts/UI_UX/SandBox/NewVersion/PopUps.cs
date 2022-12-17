using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUps : MonoBehaviour
{
    [Header("Error")]
    [SerializeField] TMP_Text error;
    [SerializeField] Animator errorAnimator;
    [Header("Info")]
    [SerializeField] TMP_Text info;
    [SerializeField] Animator infoAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowError(string text)
    {
        error.text = "Error : " + text;
        errorAnimator.SetBool("active", true);
        Invoke("HideError", 2.5f);
    }

    void HideError()
    {
        errorAnimator.SetBool("active", false);
    }

    public void ShowInfo(string text)
    {
        info.text = "Info : " + text;
        infoAnimator.SetBool("active", true);
        Invoke("HideInfo", 2.5f);
    }

    void HideInfo()
    {
        infoAnimator.SetBool("active", false);
    }
}
