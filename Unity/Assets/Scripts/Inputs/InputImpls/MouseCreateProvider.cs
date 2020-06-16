using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Inputs.InputImpls
{
    public class MouseCreateProvider : MonoBehaviour, ICreateEventProvider
    {
        private Texture2D _texture;
        public Texture2D texture => _texture;
        private static MouseCreateProvider _instance;
        private readonly ReactiveProperty<Vector3> _onCreate = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<Vector3> OnCreate => _onCreate;
        [SerializeField] private float sigma = 2.0f;

        void Start()
        {
            if (_instance == null)
            {
                _texture = Resources.Load("Textures/fish_texture") as Texture2D;

                _instance = this;
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
            }
        }
    }
}