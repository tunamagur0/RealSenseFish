using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Animals;
using Inputs;
using UniRx;

namespace Managers
{
    public class AnimalGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _fishPrefab = default;
        void Start()
        {
            var inputEvent = GetComponent<IInputEventProvider>();
            var texture = Resources.Load("Textures/fish_texture") as Texture2D;

            inputEvent.OnCreate.Subscribe(pos => { CreateAnimal(AnimalType.Fish, texture, pos); });
        }

        private void CreateAnimal(AnimalType type, Texture2D texture, Vector3 posiiton)
        {
            switch (type)
            {
                case AnimalType.Fish:
                    {
                        var go = Instantiate(_fishPrefab, posiiton, Quaternion.identity);
                        var script = go.GetComponent<Animal>();
                        script.Init(texture);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}