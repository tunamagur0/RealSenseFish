using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Inputs
{
    interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<Vector3> OnCreate { get; }
        IReadOnlyReactiveProperty<Vector3> OnTouch { get; }
    }
}