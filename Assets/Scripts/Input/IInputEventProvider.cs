using UniRx;
using UnityEngine;

namespace Inputs
{
    public interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<bool> A_Button { get; }
        IReadOnlyReactiveProperty<bool> B_Button { get; }
        IReadOnlyReactiveProperty<bool> X_Button { get; }
        IReadOnlyReactiveProperty<bool> Y_Button { get; }
        IReadOnlyReactiveProperty<bool> L_Button { get; }
        IReadOnlyReactiveProperty<bool> R_Button { get; }

        IReadOnlyReactiveProperty<Vector3> L_Stick { get; }
        IReadOnlyReactiveProperty<Vector3> R_Stick { get; }
    }
}
