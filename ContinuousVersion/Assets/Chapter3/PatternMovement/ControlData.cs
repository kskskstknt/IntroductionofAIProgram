using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter3
{
    /// <summary>
    /// パターンムーブメント制御データ構造体
    /// </summary>
    public struct ControlData
    {
        /// <summary>
        /// 回転するかどうかのフラグ
        /// </summary>
        public bool DoTurn { get; set; }

        /// <summary>
        /// 最大回転角
        /// </summary>
        public float HeadingAngleLimit { get; set; }

        /// <summary>
        /// 最大移動距離
        /// </summary>
        public float PositionLimit { get; set; }

        /// <summary>
        /// 回転制御のフラグ
        /// </summary>
        public bool CheckHeadingChange { get; set; }

        /// <summary>
        /// 移動制御のフラグ
        /// </summary>
        public bool CheckPositionChande { get; set; }

        /// <summary>
        /// 移動の方向ベクトル
        /// </summary>
        public Vector3 DirectionVector { get; set; }
    }

}