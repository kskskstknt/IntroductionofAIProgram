using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter2
{
    public class Interception : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            player = Instantiate(player, startPosition.transform.position, Quaternion.identity);
            player.transform.LookAt(targetPosition.transform.position - player.transform.position);
            player.AddComponent<Rigidbody>();
            playerBody = player.GetComponent<Rigidbody>();

            predator = Instantiate(predator, predatorPosition.transform.position, Quaternion.identity);
            predator.transform.LookAt(playerBody.position - predator.transform.position);
            predator.AddComponent<Rigidbody>();
            predatorBody = predator.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 playerDirection = (targetPosition.transform.position - playerBody.transform.position).normalized;
            if (playerDirection.magnitude > 0.01f)
            {
                Quaternion playerQ = Quaternion.LookRotation(playerDirection);
                playerBody.transform.rotation = Quaternion.Lerp(transform.rotation, playerQ, playerVelocity * Time.deltaTime);
            }
            playerBody.velocity = playerDirection * Time.deltaTime * playerVelocity;

            Intercept();
        }

        void Intercept()
        {
            // 相対速度
            Vector3 relativeV = playerBody.velocity - predatorBody.velocity;
            //Debug.Log("playerV = " + playerBody.transform.forward * playerVelocity + ", predatorV = " + predatorBody.transform.forward * predatorVelocity);
            //Debug.Log("relativeV = " + relativeV);
            // 相対距離
            Vector3 relativeS = playerBody.transform.position - predatorBody.transform.position;
            //Debug.Log("playerS = " + playerBody.position + ", predatorS = " + predatorBody.position);
            //Debug.Log("relativeS = " + relativeS);
            // 接近に要する時間
            float closingT = relativeS.magnitude / relativeV.magnitude;
            //Debug.Log("closingT = " + closingT);
            // playerの予想位置
            Vector3 predictedSAfterClosingT = playerBody.position + playerBody.velocity * closingT;
            Vector3 predatorDirection = (predictedSAfterClosingT - predatorBody.transform.position).normalized;

            Debug.Log("predicted = " + predictedSAfterClosingT);

            PutMarker(predictedSAfterClosingT);

            if (predatorDirection.magnitude > 0.01f)
            {
                Quaternion predatorQ = Quaternion.LookRotation(predatorDirection);
                predatorBody.transform.rotation = Quaternion.Lerp(transform.rotation, predatorQ, predatorVelocity * Time.deltaTime);
            }
            predatorBody.velocity = predatorDirection * Time.deltaTime * predatorVelocity;

            predatorBody.angularVelocity = predatorDirection * predatorVelocity;
        }

        void PutMarker(Vector3 position)
        {
            if (putMarker)
            {
                if (_marker)
                {
                    Destroy(_marker);
                }
                _marker = Instantiate(marker, position, Quaternion.identity);
            }
        }

        [SerializeField]
        public GameObject player;
        [SerializeField]
        public GameObject predator;
        [SerializeField]
        public GameObject marker;
        [SerializeField]
        public GameObject startPosition;
        [SerializeField]
        public GameObject targetPosition;
        [SerializeField]
        public GameObject predatorPosition;

        [SerializeField]
        public float playerVelocity;
        [SerializeField]
        public float predatorVelocity;

        [SerializeField]
        public bool putMarker = false;

        private Rigidbody playerBody;
        private Rigidbody predatorBody;
        private GameObject _marker;
    }

}