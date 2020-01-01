using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Inputs
{
    interface ICreateEventProvider
    {
        IReadOnlyReactiveProperty<Vector3> OnCreate { get; }
    }
}