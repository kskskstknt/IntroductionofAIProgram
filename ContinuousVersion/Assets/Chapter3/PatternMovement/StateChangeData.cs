using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter3
{
    /// <summary>
    /// 状態の変化を追跡するための構造体
    /// </summary>
    public struct StateChangeData
    { 
        /// <summary>
        /// 初期角度
        /// </summary>
        public Vector3 InitialHeading { get; set; }

        /// <summary>
        /// 初期位置
        /// </summary>
        public Vector3 InitialPosition { get; set; }

        /// <summary>
        /// 移動角度
        /// </summary>
        public float DeltaHeading { get; set; }

        /// <summary>
        /// 移動距離
        /// </summary>
        public float DeltaPosition { get; set; }

        /// <summary>
        /// 現在適用しているパターンデータ
        /// </summary>
        public int CurrentControlID { get; set; }
    }
}
