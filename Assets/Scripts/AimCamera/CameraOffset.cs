//http://isemito.hatenablog.com/entry/2016/12/25/084821　を参考に作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camerawork
{
    public class CameraOffset : MonoBehaviour
    {
        public float moveSpeed = 2;
        private GameObject playerObj;
        private Vector3 offset;

        void Awake()
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }

        // Use this for initialization
        void Start()
        {
            offset = transform.position - playerObj.transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var targetPosition = playerObj.transform.position + offset;
            var newPos = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}