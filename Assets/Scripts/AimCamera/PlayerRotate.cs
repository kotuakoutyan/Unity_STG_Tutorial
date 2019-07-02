//http://isemito.hatenablog.com/entry/2016/12/25/084821　を参考に作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyCharacter;
using UniRx;
using UniRx.Triggers;

namespace Camerawork
{
    public class PlayerRotate :BaseCamerawork
    {
        [SerializeField] private GameObject mainAimObj = null;
        public float rotateSpeed = 1;
        public Constrain constrainX;
        public Constrain constrainY;
        public Constrain constrainZ;

        override protected void Start()
        {
            base.Start();
            constrainX.value = constrainX.active ? 0 : 1;
            constrainY.value = constrainY.active ? 0 : 1;
            constrainZ.value = constrainZ.active ? 0 : 1;

        }

        void FixedUpdate()
        {
            RotatePlayerInAir();
        }

        private void RotatePlayerInAir()
        {
            if (mainAimObj != null)
            {
                var targetRotate = Quaternion.LookRotation(mainAimObj.transform.position - transform.position);
                var newRotate = Quaternion.Lerp(transform.rotation, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;
                transform.eulerAngles = new Vector3(
                    newRotate.x * constrainX.value,
                    newRotate.y * constrainY.value,
                    newRotate.z * constrainZ.value
                );
            }
        }

        private void RotatePlayerOnGround(Vector3 direction)
        {
            if (direction == Vector3.zero) return;
            var targetRotate = Quaternion.LookRotation(direction);
            var newRotate = Quaternion.Lerp(transform.rotation, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;
            transform.eulerAngles = new Vector3(
                newRotate.x * constrainX.value,
                newRotate.y * constrainY.value,
                newRotate.z * constrainZ.value
            );
        }
    }

    [System.Serializable]
    public class Constrain
    {
        public bool active;
        [HideInInspector]
        public float value;
    }
}
