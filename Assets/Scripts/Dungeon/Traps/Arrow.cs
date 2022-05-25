using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Trap
{
    float _speed = 1.0f;
    bool _onFire = false;
    [SerializeField] SpriteRenderer _renderer;

    void Start()
    {
        Invoke("DestroyItself", 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * _speed;
    }

    void DestroyItself()
    {
        Destroy(gameObject);
    }

    protected override void InflicteDamage(EntityData player)
    {
        player.entityHealthManager.TakeDamage(_damage);

        if (_onFire) player.entityStateManager.ActiveHarmfullEffects(States.FIRE);
        Destroy(gameObject);
    }

    public void SetOnFire()
    {
        _onFire = true;
        _renderer.color = Color.red;
    }


    protected override void TriggerEnterNotPlayer(Collider2D collider)
    {
        if (collider.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }

    }
}
