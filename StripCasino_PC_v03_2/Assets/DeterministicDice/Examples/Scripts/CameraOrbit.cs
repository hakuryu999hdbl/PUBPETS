//  A simple Unity C# script for orbital movement around a target gameobject
//  Author: Ashkan Ashtiani
//  Gist on Github: https://gist.github.com/3dln/c16d000b174f7ccf6df9a1cb0cef7f80

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TDLN.CameraControllers
{
    public class CameraOrbit : MonoBehaviour
    {
        public GameObject target;
        public float height = 1.0f;
        public float distance = 10.0f;

        public float xSpeed = 250.0f;
        public float ySpeed = 120.0f;

        private float prevDistance;

        float x = 0.0f;
        float y = 0.0f;

        void Start()
        {
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            transform.position += Vector3.up * height;
            transform.LookAt(target.transform);
        }

        void LateUpdate()
        {
            if (distance < 2)
                distance = 2;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
            if (target && Input.GetMouseButton(0))
            {
                x += Input.GetAxisRaw("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxisRaw("Mouse Y") * ySpeed * 0.02f;

                var rotation = Quaternion.Euler(y, x, 0);
                var position =
                    rotation * new Vector3(0f, height, -distance) + target.transform.position;
                transform.rotation = rotation;
                transform.position = position;
            }

            if (Math.Abs(prevDistance - distance) > 0.001f)
            {
                prevDistance = distance;
                var rot = Quaternion.Euler(y, x, 0);
                var po = rot * new Vector3(0f, height, -distance) + target.transform.position;
                transform.rotation = rot;
                transform.position = po;
            }

            transform.LookAt(target.transform);
        }
    }
}
