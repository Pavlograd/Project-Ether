using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobIncome : MonoBehaviour
{
    [SerializeField] Animator animator;

    public Incomes incomes;

    float life = 100.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        if (life > 0.0f)
        {
            life -= 25.0f;

            if (life <= 0.0f)
            {
                animator.SetTrigger("Dead");
                incomes.UpdateIncomes();
                Invoke("DestroySelf", 0.9f);
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
