using UnityEngine;
using Barrage.Shot;
using UnityEngine.Events;
using Barrage.Bullet;
using UniRx;
using Barrage.Iterators;
using Barrage;

namespace FlyCharacter
{
    [RequireComponent(typeof(Iterator))]
    public class FlyCharacterAction : BaseFlyCharacter
    {
        [SerializeField] private GameObject Aim = null;
        private FlyCharacterStatus Status;
        private Iterator Iterator;

        override protected void Start()
        {
            base.Start();
            InputEventProvider.X_Button.DistinctUntilChanged().Subscribe(x => { if (x) Attack1(); });
            InputEventProvider.Y_Button.DistinctUntilChanged().Subscribe(x => { if (x) Attack2(); });
            InputEventProvider.L_Button.DistinctUntilChanged().Subscribe(x => { if (x) Attack3(); });
            InputEventProvider.R_Button.DistinctUntilChanged().Subscribe(x => { if (x) Attack4(); });

            Status = GetComponent<FlyCharacterStatus>();
            Iterator = GetComponent<Iterator>();
        }

        public void Attack1() => Iterator.SetUnityAction(Status.AttackerType, Status.BarrageData1, Aim, Status.Target);
        public void Attack2() => Iterator.SetUnityAction(Status.AttackerType, Status.BarrageData2, Aim, Status.Target);
        public void Attack3() => Iterator.SetUnityAction(Status.AttackerType, Status.BarrageData3, Aim, Status.Target);
        public void Attack4() => Iterator.SetUnityAction(Status.AttackerType, Status.BarrageData4, Aim, Status.Target);
    }
}