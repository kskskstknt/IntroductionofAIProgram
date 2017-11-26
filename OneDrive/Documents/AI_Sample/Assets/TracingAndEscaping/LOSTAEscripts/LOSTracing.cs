using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSTracing : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        System.Random random = new System.Random();

        int predatorPosition = random.Next(0, spawnArea.Length);
        predator = Instantiate(predator, spawnArea[predatorPosition].transform.position, Quaternion.identity);

        int playerPosition;
        while ((playerPosition = random.Next(0, spawnArea.Length)) == predatorPosition) ;
        player = Instantiate(player, spawnArea[playerPosition].transform.position, Quaternion.identity);

        UpdateCamera();

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
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePlayer();
        UpdateCamera();
        LOSTrase();
	}

    void UpdateCamera()
    {
        mainCamera.transform.position = new Vector3(player.transform.position.x, lookAtDistance, player.transform.position.z);
        mainCamera.transform.LookAt(player.transform);
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

    void LOSTrase()
    {
        // uはpredatorがplayerを指す方向を表すベクトル
        Vector3 u = (player.transform.position - predator.transform.position).normalized;
        //Vector3 predatorX = new Vector3(predator.transform.position.x, predator.transform.position.y, 0).normalized;
        //float theta = Mathf.Acos(Vector3.Dot(predatorX, u));
        //predator.transform.rotation = new Quaternion(0, theta, 0, 0);
        predator.transform.TransformDirection(u);
        predator.transform.Translate(u * (moveDelta + 0.1f));
    }

    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject predator;
    [SerializeField]
    public GameObject[] spawnArea;
    [SerializeField]
    public Camera mainCamera;
    [SerializeField]
    public float lookAtDistance;

    /// <summary>
    /// playerの移動量
    /// </summary>
    [SerializeField]
    public float moveDelta;

    private IDictionary<KeyCode, Vector3> moveVector;
}
