using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Inputs.InputImpls
{
    public class MouseTouchProvider : MonoBehaviour, ITouchEventProvider
    {
        private static MouseTouchProvider _instance;
        private readonly ReactiveProperty<Vector3> _onTouch = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<Vector3> OnTouch => _onTouch;

        void Start()
        {
            if (_instance == null)
            {
                this.UpdateAsObservable()
                    .Select(_ => Input.GetMouseButton(1))
                    .DistinctUntilChanged()
                    .Where(x => x)
                    .Subscribe(_ =>
                    {
                        var position = Input.mousePosition;
                        position.z = 10f;
                        var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

                        _onTouch.SetValueAndForceNotify(screenToWorldPointPosition);
                    });
                _instance = this;
            }
        }
    }
}