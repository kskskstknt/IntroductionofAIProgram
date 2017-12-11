using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter3
{
    public class EntityMotion : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.FreezeRotationX;

            InitializePatrolPattern();
            InitializeZigzagPattern();

            InitializePatternTracking(0);

            patternData = pattern == Pattern.Patrol ? patrolPattern : zigzagPattern;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void FixedUpdate()
        {
            if (!DoPattern(patternData))
            {
                InitializePatternTracking(0);
            }
        }

        private bool DoPattern(ControlData[] patterns)
        {
            int i = patternTracking.CurrentControlID;

            // 回転しきったか確認
            bool hasHeaded = patterns[i].CheckHeadingChange && patternTracking.DeltaHeading >= patterns[i].HeadingAngleLimit;
            // 移動しきったか確認
            bool hasMoved = patterns[i].CheckPositionChande && patternTracking.DeltaPosition >= patterns[i].PositionLimit;

            Debug.Log("pattern finished? => " + (hasHeaded || hasMoved));

            // 現在のパターン行動を終えた場合、次のパターンを取り出す
            if (hasHeaded || hasMoved)
            {
                InitializePatternTracking(++i);
                if (patternTracking.CurrentControlID >= patterns.Length)
                {
                    return false;
                }
            }

            Vector3 direction = rb.velocity.normalized;

            // entityの方向ベクトルと最初のentityの方向ベクトルとの内積を求める
            // dot  = direction * InitialHeading = |direction| * |InitialHeading| * cosθ
            // cosθ = dot / (|direction| * |InitialHeading|) = dot (directionもInitialHeadingも正規化済みなため)
            // θ    = Acos(dot) [rad]
            // 以上の手順で何度向きが変わったかを求められる
            float dot = Vector3.Dot(direction, patternTracking.InitialHeading);
            patternTracking.DeltaHeading = Mathf.Abs(Mathf.Acos(dot) * 180 / Mathf.PI);

            // 移動距離を求める
            patternTracking.DeltaPosition = (rb.transform.position - patternTracking.InitialPosition).magnitude;

            // 実際にパターン行動を行う
            if (patterns[i].DoTurn)
            {
                Debug.Log("Turn!!");
                rb.velocity = RotateVector(rb.velocity) * speed * Time.deltaTime;
            }
            else
            {
                Debug.Log("Forward!!");
                rb.velocity = patterns[i].DirectionVector * speed * Time.deltaTime;
            }

            Debug.Log("velocity = " + rb.velocity);

            return true;
        }


        private Vector3 RotateVector(Vector3 origin)
        {
            float radius = deltaAngle * Mathf.PI / 180f;
            return new Vector3(
                    Mathf.Cos(radius) * origin.x + Mathf.Sin(radius) * origin.z,
                    origin.y,
                    -Mathf.Sin(radius) * origin.x + Mathf.Cos(radius) * origin.z
                ).normalized;
        }

        private void InitializePatrolPattern()
        {
            patrolPattern[0].DoTurn = false;
            patrolPattern[0].CheckHeadingChange = false;
            patrolPattern[0].CheckPositionChande = true;
            patrolPattern[0].PositionLimit = patrolForwardDistance;
            patrolPattern[0].HeadingAngleLimit = 0;
            patrolPattern[0].DirectionVector = Vector3.forward;

            patrolPattern[1].DoTurn = true;
            patrolPattern[1].CheckHeadingChange = true;
            patrolPattern[1].CheckPositionChande = false;
            patrolPattern[1].PositionLimit = 0;
            patrolPattern[1].HeadingAngleLimit = 90;
            patrolPattern[1].DirectionVector = Vector3.forward;

            patrolPattern[2].DoTurn = false;
            patrolPattern[2].CheckHeadingChange = false;
            patrolPattern[2].CheckPositionChande = true;
            patrolPattern[2].PositionLimit = patrolForwardDistance;
            patrolPattern[2].HeadingAngleLimit = 0;
            patrolPattern[2].DirectionVector = Vector3.right;

            patrolPattern[3].DoTurn = true;
            patrolPattern[3].CheckHeadingChange = true;
            patrolPattern[3].CheckPositionChande = false;
            patrolPattern[3].PositionLimit = 0;
            patrolPattern[3].HeadingAngleLimit = 90;
            patrolPattern[3].DirectionVector = Vector3.right;

            patrolPattern[4].DoTurn = false;
            patrolPattern[4].CheckHeadingChange = false;
            patrolPattern[4].CheckPositionChande = true;
            patrolPattern[4].PositionLimit = patrolForwardDistance;
            patrolPattern[4].HeadingAngleLimit = 0;
            patrolPattern[4].DirectionVector = Vector3.back;

            patrolPattern[5].DoTurn = true;
            patrolPattern[5].CheckHeadingChange = true;
            patrolPattern[5].CheckPositionChande = false;
            patrolPattern[5].PositionLimit = 0;
            patrolPattern[5].HeadingAngleLimit = 90;
            patrolPattern[5].DirectionVector = Vector3.back;

            patrolPattern[6].DoTurn = false;
            patrolPattern[6].CheckHeadingChange = false;
            patrolPattern[6].CheckPositionChande = true;
            patrolPattern[6].PositionLimit = patrolForwardDistance;
            patrolPattern[6].HeadingAngleLimit = 0;
            patrolPattern[6].DirectionVector = Vector3.left;

            patrolPattern[7].DoTurn = true;
            patrolPattern[7].CheckHeadingChange = true;
            patrolPattern[7].CheckPositionChande = false;
            patrolPattern[7].PositionLimit = 0;
            patrolPattern[7].HeadingAngleLimit = 90;
            patrolPattern[7].DirectionVector = Vector3.left;
        }

        private void InitializeZigzagPattern()
        {
            zigzagPattern[0].DoTurn = false;
            zigzagPattern[0].CheckPositionChande = true;
            zigzagPattern[0].CheckHeadingChange = false;
            zigzagPattern[0].PositionLimit = zigzagForwardDistance;
            zigzagPattern[0].HeadingAngleLimit = 0;
            zigzagPattern[0].DirectionVector = Vector3.forward;

            zigzagPattern[1].DoTurn = true;
            zigzagPattern[1].CheckPositionChande = false;
            zigzagPattern[1].CheckHeadingChange = true;
            zigzagPattern[1].PositionLimit = 0;
            zigzagPattern[1].HeadingAngleLimit = zigzagTurningAngle;
            zigzagPattern[1].DirectionVector = Vector3.forward;

            zigzagPattern[2].DoTurn = false;
            zigzagPattern[2].CheckPositionChande = true;
            zigzagPattern[2].CheckHeadingChange = false;
            zigzagPattern[2].PositionLimit = zigzagForwardDistance;
            zigzagPattern[2].HeadingAngleLimit = 0;
            zigzagPattern[2].DirectionVector = Vector3.forward;

            zigzagPattern[3].DoTurn = true;
            zigzagPattern[3].CheckPositionChande = false;
            zigzagPattern[3].CheckHeadingChange = true;
            zigzagPattern[3].PositionLimit = 0;
            zigzagPattern[3].HeadingAngleLimit = zigzagTurningAngle;
            zigzagPattern[3].DirectionVector = Vector3.forward;
        }

        private void InitializePatternTracking(int id)
        {
            patternTracking = new StateChangeData
            {
                CurrentControlID = id,
                InitialPosition = rb.transform.position,
                InitialHeading = rb.transform.rotation.eulerAngles.normalized,
                DeltaHeading = 0,
                DeltaPosition = 0
            };
        }

        public enum Pattern { Patrol, Zigzag}

        private const int _Patrol_Array_Size = 8;
        private const int _Zigzag_Array_Size = 4;

        /// <summary>
        /// 巡回（ラウンド正方形）パターン
        /// </summary>
        private ControlData[] patrolPattern = new ControlData[_Patrol_Array_Size];

        /// <summary>
        /// ジグザグパターン
        /// </summary>
        private ControlData[] zigzagPattern = new ControlData[_Zigzag_Array_Size];

        /// <summary>
        /// パターン制御構造体
        /// </summary>
        private StateChangeData patternTracking;

        private Rigidbody rb;

        private ControlData[] patternData;

        [SerializeField]
        public float speed;
        [SerializeField]
        public float deltaAngle;

        [SerializeField]
        public Pattern pattern = Pattern.Patrol;

        [SerializeField]
        public float patrolForwardDistance = 50;

        [SerializeField]
        public float zigzagForwardDistance = 30;

        [SerializeField]
        public float zigzagTurningAngle = 60;

        //[SerializeField]
        //public bool doTurn;
    }
}
