using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Inputs
{
    interface ITouchEventProvider
    {
        IReadOnlyReactiveProperty<Vector3> OnTouch { get; }
    }
}