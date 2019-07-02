//http://isemito.hatenablog.com/entry/2016/12/25/084821　を参考に作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Camerawork
{
    public class AimController : BaseCamerawork
    {
        private GameObject mainAimObj;
        public GameObject subAimObj;
        public float subAimSpeed = 1.0f;
        public float sensitivity = 1.0f;
        public bool reverseX = false;
        public bool reverseY = false;
        public float clampAngle = 60;

        private GameObject followTarget;
        private Vector3 offset;

        // Use this for initialization
        void Awake()
        {
            //サブ照準が存在するときだけメイン照準情報を取得
            mainAimObj = subAimObj != null ? transform.Find("MainAim").gameObject : null;
            followTarget = GameObject.FindGameObjectWithTag("Player").transform.Find("FollowTarget").gameObject;
        }

        override protected void Start()
        {
            base.Start();
            offset = transform.position - followTarget.transform.position;
        }

        void FixedUpdate()
        {
            AimRotate(new Vector3(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"), 0));
            AimMove();
            if (subAimObj != null) SubAimMove();
        }

        private void AimRotate(Vector3 input)
        {
            var newX = transform.eulerAngles.x + input.y * sensitivity * (reverseY ? -1: 1);
            newX -= newX > 180 ? 360 : 0;
            newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;
            var newY = transform.eulerAngles.y + input.x * sensitivity * (reverseX ? -1 : 1);
            transform.localEulerAngles = new Vector3(newX, newY, 0);
        }

        private void AimMove()
        {
            transform.position = followTarget.transform.position + offset;
        }

        private void SubAimMove()
        {
            var thisPos = subAimObj.transform.position;
            var targetPos = mainAimObj.transform.position;
            subAimObj.transform.position = Vector3.Lerp(thisPos, targetPos, subAimSpeed * Time.deltaTime);
        }
    }
}