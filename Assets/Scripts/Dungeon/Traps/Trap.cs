using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    public int id = 0;
    [SerializeField] protected float _damage = 10.0f;

    // function to send elementary or normal damages
    protected abstract void InflicteDamage(EntityData player);
    protected abstract void TriggerEnterNotPlayer(Collider2D collider);

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("player");
            EntityData player = collider.GetComponent<EntityData>();

            InflicteDamage(player);
        }
        else
        {
            TriggerEnterNotPlayer(collider);
        }
    }

    public float getDamage()
    {
        return _damage;
    }

    // Will be called when instantiate trap
    public void InitDamage(int dungeonLevel)
    {
        _damage = _damage * (Mathf.Exp(dungeonLevel * 0.2f));
    }
}
