using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinsAnimation : MonoBehaviour
{
    private Queue<GameObject> _queue = new Queue<GameObject>();
    [SerializeField] Transform _endPoint;
    private Vector3 _startPoint;
    [SerializeField] private int _coinNumber = 50;
    [SerializeField] private GameObject _coinPrefab;
    private Vector3[] path;
    [HideInInspector] public float timeToTravel = .75f;

    private void Awake() {
        _startPoint = transform.position;
        for (int i = 0; i < _coinNumber; i++) {
            GameObject coin;
            coin = Instantiate(_coinPrefab, transform);
            coin.SetActive(false);
            _queue.Enqueue(coin);
        }
    }

    public IEnumerator Animate()
    {
        if (_endPoint == null) {
            Debug.Log("Can't perform coins animation because the endPoint is missing.");
            yield return null;
        }
        while(_queue.Count > 0) {
            yield return new WaitForSeconds(0.03f);
            GameObject coin = _queue.Dequeue();
            path = new Vector3[2] {
                new Vector3(_startPoint.x + + Random.Range(-100, 100), _startPoint.y + Random.Range(-100, 100) , 0),
                _endPoint.position
            };
            coin.gameObject.SetActive(true);
            coin.transform.DOPath(path, timeToTravel, PathType.CatmullRom)
                .SetEase(Ease.InSine)
                .OnComplete(() => {
                    AudioManager.instance?.PlaySoundEffect(SFX.COINS);
                    Destroy(coin);
                });
        }
    }

    public int GetNumberOfCoins()
    {
        return _coinNumber;
    }
}
