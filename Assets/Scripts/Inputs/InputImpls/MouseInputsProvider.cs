
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

        [SerializeField] private float sigma = 2.0f;
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

                    //奥にランダムにずらす
                    screenToWorldPointPosition += new Vector3(Random.value * sigma, 0, 0);

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

                    _onTouch.SetValueAndForceNotify(screenToWorldPointPosition);
                });
        }
    }
}