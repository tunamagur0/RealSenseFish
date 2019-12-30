
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Inputs.InputImpls
{
    public class MouseInputsProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly ReactiveProperty<Vector3> _onCreate = new ReactiveProperty<Vector3>();
        private readonly ReactiveProperty<Vector3> _onTouch = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<Vector3> OnCreate => _onCreate;
        public IReadOnlyReactiveProperty<Vector3> OnTouch => _onTouch;
        protected void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => Input.GetMouseButton(0))
                .DistinctUntilChanged()
                .Where(x => x)
                .Subscribe(_ =>
                {
                    var position = Input.mousePosition;
                    position.z = 10f;
                    var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

                    _onCreate.SetValueAndForceNotify(screenToWorldPointPosition);
                });

            this.UpdateAsObservable()
                .Select(_ => Input.GetMouseButton(1))
                .DistinctUntilChanged()
                .Where(x => x)
                .Subscribe(_ =>
                {
                    var position = Input.mousePosition;
                    position.z = 10f;
                    var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

                    _onTouch.SetValueAndForceNotify(Input.mousePosition);
                });
        }
    }
}