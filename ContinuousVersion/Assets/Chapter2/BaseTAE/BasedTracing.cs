using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単純な追跡を行う
/// </summary>
public class BasedTracing : MonoBehaviour
{
    /// <param name="delta">追跡する幅、スピードと等しい</param>
    /// <param name="predator">追跡者</param>
    /// <param name="player">追跡するターゲット</param>
    public BasedTracing(float delta, GameObject predator, GameObject player)
    {
        this.predator = predator;
        this.player = player;
        this.delta = delta;
    }

    /// <summary>
    /// playerのx, z座標に対してdeltaの幅でpredatorが接近する
    /// </summary>
    public void Trace()
    {
        // x座標に対する追跡
        if (player.transform.position.x < predator.transform.position.x)
        {
            predator.transform.Translate(-delta, 0, 0, Space.World);
        }
        else if (player.transform.position.x > predator.transform.position.x)
        {
            predator.transform.Translate(delta, 0, 0, Space.World);
        }

        // y座標に対する追跡
        if (player.transform.position.z < predator.transform.position.z)
        {
            predator.transform.Translate(0, 0, -delta, Space.World);
        }
        else if (player.transform.position.z > predator.transform.position.z)
        {
            predator.transform.Translate(0, 0, delta, Space.World);
        }
    }

    private readonly GameObject predator;
    private readonly GameObject player;
    private float delta;
}
