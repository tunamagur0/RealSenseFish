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

            // ランダムに左右に移動する
            transform.forward *= (Random.Range(0, 2) == 0 ? -1 : 1);

            // 壁にぶつかったら反転する
            var OnTriggerEnterWall = this.OnTriggerEnterAsObservable()
                                         .Select(collision => collision.tag)
                                         .Where(tag => tag == "Wall")
                                         .Subscribe(_ => transform.forward *= -1);

            // タッチされた時に逃げる（unirxを使った意味がない）
            var OnTriggerEnterNotEnterArea = this.OnTriggerEnterAsObservable()
                                                 .Subscribe(collision =>
                                                 {
                                                     if (collision.tag == "NotEnterArea")
                                                     {
                                                         var go = collision.gameObject;
                                                         transform.forward = transform.position - go.transform.position;
                                                     }
                                                 });

            // 時間が経ったら消す
            Destroy(this.gameObject, _liveTime);

        }

        void Update()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

    }
}