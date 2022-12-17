using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncome : MonoBehaviour
{
    [SerializeField] private Animator _staffAnimator;
    [SerializeField] public Animator bodyAnimator;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Attack", 1.0f, 2.0f);
    }

    void Attack()
    {
        bodyAnimator.SetTrigger("Auto_attack");
        _staffAnimator.SetTrigger("Auto_attack");
    }
}
