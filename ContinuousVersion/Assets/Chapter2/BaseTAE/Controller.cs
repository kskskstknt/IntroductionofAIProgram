using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter2
{
    public class Controller : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            // 出現場所を乱数で設定できるようする
            System.Random random = new System.Random();

            // predatorの出現場所を設定
            int predatorPosition = random.Next(0, spawnArea.Length);
            // predatorを出現させる
            predator = Instantiate(predator, spawnArea[predatorPosition].transform.position, Quaternion.identity);

            // playerの出現場所を設定
            int playerPosition;
            // predatorと被らないようにする
            while (predatorPosition == (playerPosition = random.Next(0, spawnArea.Length))) ;
            // playerを出現させる
            player = Instantiate(player, spawnArea[playerPosition].transform.position, Quaternion.identity);

            Debug.Log("predator position = " + predator.transform.position);
            Debug.Log("player position = " + player.transform.position);

            // 押されたキーに対応した移動量（ベクトル）を登録
            moveVector = new Dictionary<KeyCode, Vector3>
        {
            {
                KeyCode.RightArrow,
                new Vector3(moveDelta, 0.0f, 0.0f)
            },
            {
                KeyCode.LeftArrow,
                new Vector3(-moveDelta, 0.0f, 0.0f)
            },
            {
                KeyCode.UpArrow,
                new Vector3(0.0f, 0.0f, moveDelta)
            },
            {
                KeyCode.DownArrow,
                new Vector3(0.0f, 0.0f, -moveDelta)
            }
        };

            basedTracing = new BasedTracing(0.1f, predator, player);
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayer();
            UpdateCamera();
            basedTracing.Trace();
        }

        /// <summary>
        /// playerCameraの更新
        /// </summary>
        void UpdateCamera()
        {
            // playerの場所とplayerCameraの場所を一致させる
            playerCamera.transform.position = player.transform.position;
            // predatorを見るようにする
            playerCamera.transform.LookAt(predator.transform);
        }

        /// <summary>
        /// playerの移動を反映
        /// </summary>
        void UpdatePlayer()
        {
            // 横方向の移動
            Vector3 verticalVector = new Vector3();
            if (Input.GetKey(KeyCode.RightArrow))
            {
                verticalVector = moveVector[KeyCode.RightArrow];
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                verticalVector = moveVector[KeyCode.LeftArrow];
            }

            player.transform.Translate(verticalVector, Space.World);

            // 縦方向の移動
            Vector3 horizontalVector = new Vector3();
            if (Input.GetKey(KeyCode.UpArrow))
            {
                horizontalVector = moveVector[KeyCode.UpArrow];
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                horizontalVector = moveVector[KeyCode.DownArrow];
            }

            player.transform.Translate(horizontalVector, Space.World);
        }

        /// <summary>
        /// predatorとplayerの出現位置
        /// </summary>
        [SerializeField]
        public GameObject[] spawnArea = new GameObject[4];

        /// <summary>
        /// playerを追跡するオブジェクト
        /// </summary>
        [SerializeField]
        public GameObject predator;

        /// <summary>
        /// predatorから逃げるオブジェクト
        /// </summary>
        [SerializeField]
        public GameObject player;

        /// <summary>
        /// player視点でpredatorを撮影するカメラ
        /// </summary>
        [SerializeField]
        public Camera playerCamera;

        /// <summary>
        /// playerの移動量
        /// </summary>
        [SerializeField]
        public float moveDelta;

        private IDictionary<KeyCode, Vector3> moveVector;
        private BasedTracing basedTracing;
    }
}
