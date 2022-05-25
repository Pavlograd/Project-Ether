using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float delay = 0.0f;
    // Keep it in case need level for loot
    private GameInfos gameInfos;
    private Animator _animatorChest;
    private Animator _animatorLoot;
    private GameObject _effect;
    private bool _activated = false;
    void Start()
    {
        _animatorChest = transform.Find("SM_Prop_Chest/SM_Prop_Chest_List").GetComponent<Animator>();
        _animatorLoot = transform.Find("Loot").GetComponent<Animator>();
        if (_animatorChest != null) {
            _animatorChest.enabled = false;
        }
        if (_animatorLoot != null) {
            _animatorLoot.enabled = false;
        }
        gameInfos = GameObject.Find("GameInfos").GetComponent<GameInfos>();
        _effect = transform.Find("FX_Flame_Booster").gameObject;
        _effect.SetActive(false);
        //SetDelayAnimation();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetDelayAnimation()
    {
        if (delay == 0.0f) {
            delay = Random.Range(0.0F, 1.5F);
        }
        Invoke("ReabledAnimation", delay);
    }

    public void ReabledAnimation()
    {
        if (_activated)
            return;
        _activated = true;
        _animatorChest.enabled = true;
        _animatorLoot.enabled = true;
        _effect.SetActive(true);
        Invoke("DesactiveEffect", 2.3F);
    }

    void DesactiveEffect()
    {
        transform.Find("Loot").gameObject.SetActive(false);
        _effect.SetActive(false);
    }
}
