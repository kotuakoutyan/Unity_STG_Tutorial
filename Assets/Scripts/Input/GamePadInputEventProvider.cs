using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Inputs
{
    public class GamePadInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly ReactiveProperty<bool> a_Button = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> b_Button = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> x_Button = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> y_Button = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> l_Button = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> r_Button = new ReactiveProperty<bool>(false);

        private ReactiveProperty<Vector3> l_Stick = new ReactiveProperty<Vector3>();
        private ReactiveProperty<Vector3> r_Stick = new ReactiveProperty<Vector3>();

        public IReadOnlyReactiveProperty<bool> A_Button => a_Button;
        public IReadOnlyReactiveProperty<bool> B_Button => b_Button;
        public IReadOnlyReactiveProperty<bool> X_Button => x_Button;
        public IReadOnlyReactiveProperty<bool> Y_Button => y_Button;
        public IReadOnlyReactiveProperty<bool> L_Button => l_Button;
        public IReadOnlyReactiveProperty<bool> R_Button => r_Button;

        public IReadOnlyReactiveProperty<Vector3> L_Stick => l_Stick;
        public IReadOnlyReactiveProperty<Vector3> R_Stick => r_Stick;

        protected void Start()
        {
            this.UpdateAsObservable().Select(_ => Input.GetButton("Fire3")).DistinctUntilChanged().Subscribe(x => a_Button.Value = x);
            this.UpdateAsObservable().Select(_ => Input.GetButton("Fire4")).DistinctUntilChanged().Subscribe(x => b_Button.Value = x);
            this.UpdateAsObservable().Select(_ => Input.GetButton("Fire1")).DistinctUntilChanged().Subscribe(x => x_Button.Value = x);
            this.UpdateAsObservable().Select(_ => Input.GetButton("Fire2")).DistinctUntilChanged().Subscribe(x => y_Button.Value = x);
            this.UpdateAsObservable().Select(_ => Input.GetButton("LB")).DistinctUntilChanged().Subscribe(x => l_Button.Value = x);
            this.UpdateAsObservable().Select(_ => Input.GetButton("RB")).DistinctUntilChanged().Subscribe(x => r_Button.Value = x);
            this.UpdateAsObservable().Select(_ => new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)).DistinctUntilChanged().Subscribe(x => l_Stick.SetValueAndForceNotify(x));
            this.UpdateAsObservable().Select(_ => new Vector3(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"), 0)).DistinctUntilChanged().Subscribe(x => r_Stick.SetValueAndForceNotify(x));
        }
    }
}
