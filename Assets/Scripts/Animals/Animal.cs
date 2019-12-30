using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Animals
{
    abstract class Animal : MonoBehaviour
    {
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private float _liveTime = 10.0f;   //[s]
        public void Init(Texture2D texture)
        {
            // textureを反映する
            this.GetComponentInChildren<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texture);

            // 壁にぶつかったら反転する
            var OnTriggerEnterWall = this.OnTriggerEnterAsObservable()
                                         .Select(collision => collision.tag)
                                         .Where(tag => tag == "Wall")
                                         .Subscribe(_ => transform.forward *= -1);

            // 時間が経ったら消す
            Destroy(this.gameObject, _liveTime);

        }

        void Update()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

    }
}