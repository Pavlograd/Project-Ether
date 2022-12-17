using System;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    private Transform _target;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _landingPos;
    private bool _hasLanded = false;
    private float _animation;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        _target = player.transform.Find("Ground");
    }

    public void ActiveObject(Vector2 landingPos)
    {
        gameObject.SetActive(true);
        _landingPos = landingPos;
    }

    private void Update() {
        if (this.gameObject.activeSelf && !_hasLanded) {
            _animation += Time.deltaTime;
            transform.position = Parabola(transform.position, _landingPos, .50f, _animation * 1.25f);
            if (transform.position.y <= _landingPos.y) {
                _hasLanded = true;
            }
        }
        if (_hasLanded && _target) {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _velocity, Time.deltaTime * UnityEngine.Random.Range(7, 11));
        }
    }

    Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
        var mid = Vector2.Lerp(start, end, t);
        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (AudioManager.instance) {
                AudioManager.instance.PlaySoundEffect(SFX.COINS);
            }
            Destroy(gameObject);
        }
    }
}
