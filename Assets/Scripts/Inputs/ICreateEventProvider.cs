using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Inputs
{
    interface ICreateEventProvider
    {
        Texture2D texture { get; }

        IReadOnlyReactiveProperty<Vector3> OnCreate { get; }
    }
}