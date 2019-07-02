//http://isemito.hatenablog.com/entry/2016/12/25/084821　を参考に作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camerawork
{
    public class CameraFollow : MonoBehaviour
    {

        public float moveSpeed = 3;
        public float rotateSpeed = 3;
        [SerializeField] private GameObject lookTarget = null;
        [SerializeField] private GameObject cameraPositionTarget = null;

        void Awake()
        {
            if (tag == "Player")
            {
                lookTarget = GameObject.Find("CameraRotateOrigin").gameObject;
                cameraPositionTarget = lookTarget.transform.Find("CameraPositionTarget").gameObject;
            }
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, cameraPositionTarget.transform.position, moveSpeed * Time.deltaTime);

            var vectorToTarget = lookTarget.transform.position - transform.position;
            var targetRotate = Quaternion.LookRotation(vectorToTarget);
            transform.eulerAngles = Quaternion.Lerp(transform.rotation, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;
        }
    }
}