using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Inputs;

namespace Effects
{
    class Effecter : MonoBehaviour
    {
        [SerializeField] private GameObject _notEnterPrefab = default;
        [SerializeField] private float _areaRadius = 1.0f;
        private float _liveTime = 0.1f;

        void Awake()
        {
            if (_notEnterPrefab == default)
                _notEnterPrefab = Resources.Load("Prefabs/notEnterArea") as GameObject;
        }

        void Start()
        {
            var inputEvent = GetComponent<ITouchEventProvider>();

            inputEvent.OnTouch.Subscribe(pos => CreateNotEnterArea(pos));
        }

        private void CreateNotEnterArea(Vector3 position)
        {
            var go = Instantiate(_notEnterPrefab, position, Quaternion.identity);
            go.transform.localScale *= _areaRadius * 2;
            Destroy(go, _liveTime);
        }
    }
}
