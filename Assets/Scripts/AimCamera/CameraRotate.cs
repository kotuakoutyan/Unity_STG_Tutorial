//http://isemito.hatenablog.com/entry/2016/12/25/084821　を参考に作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyCharacter;
using Inputs;
using UniRx;
using UniRx.Triggers;

namespace Camerawork
{
    public class CameraRotate : BaseCamerawork
    {
        public float sensitivity = 1.0f;
        public bool reverseX = true;
        public bool reverseY= true;
        public float clampAngle = 60;
        private GameObject followTarget;

        private bool IsOnGround;

        void Awake()
        {
            followTarget = GameObject.FindGameObjectWithTag("Player").transform.Find("FollowTarget").gameObject;
        }

        override protected void Start()
        {
            base.Start();
            GameObject.FindGameObjectWithTag("Player").GetComponent<FlyCharacterCore>().IsOnGround.Subscribe(x => IsOnGround = x);
            this.FixedUpdateAsObservable().Select(_ => InputEventProvider.R_Stick.Value).Subscribe(x => RotateCamera(x));
        }

        private void RotateCamera(Vector3 input)
        {
            if (IsOnGround)
            {
                var newX = transform.eulerAngles.x + input.y * sensitivity * (reverseX ? -1 : 1);
                newX -= newX > 180 ? 360 : 0;
                newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;

                var newY = transform.eulerAngles.y + input.x * -sensitivity * (reverseY ? -1 : 1);
                newY -= newY > 180 ? 360 : 0;

                transform.eulerAngles = new Vector3(newX, newY, 0);
            }
            else
            {
                var newX = transform.eulerAngles.x + input.y * sensitivity * (reverseX ? -1 : 1);
                newX -= newX > 180 ? 360 : 0;
                newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;

                transform.eulerAngles = new Vector3(newX, followTarget.transform.eulerAngles.y, 0);

            }
        }
        
    }
}