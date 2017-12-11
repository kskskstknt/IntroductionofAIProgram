using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter3
{
    public class StandardAlgoritm : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            predator = Instantiate(predator, startPosition.transform.position, Quaternion.identity);
            predator.AddComponent<Rigidbody>();
            predatorBody = predator.GetComponent<Rigidbody>();

            Vector3 stepVector = predator.transform.forward;

            pattern = new PatternData[]
            {
            new PatternData(Movement.Step, stepVector * speed, Time.deltaTime * 45),
            new PatternData(Movement.Turn, Quaternion.Euler(0, 90f, 0), Time.deltaTime * 45),
            new PatternData(Movement.Step, transform.right * speed, Time.deltaTime * 45),
            new PatternData(Movement.Turn, Quaternion.Euler(0, -90f, 0), Time.deltaTime * 45)
            };

            currentStep = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (pattern[currentStep].movement == Movement.Step)
            {
                //Debug.Log("Step");
                predatorBody.angularVelocity = Vector3.zero;
                predatorBody.velocity = pattern[currentStep].vector;
            }
            else
            {
                //Debug.Log("Turn");
                predatorBody.velocity = Vector3.zero;
                float step = rotateSpeed;
                predatorBody.transform.rotation = Quaternion.RotateTowards(
                    predatorBody.transform.rotation, pattern[currentStep].quaternion, step
                    );
            }

            pattern[currentStep].timeToMove -= Time.deltaTime;

            if (pattern[currentStep].timeToMove <= 0)
            {
                pattern[currentStep].Reset();
                currentStep = (currentStep + 1) % pattern.Length;
            }
        }

        private class PatternData
        {
            public Movement movement;
            public Vector3 vector;
            public float timeToMove;
            public Quaternion quaternion;
            private float lifeTime;

            public PatternData(Movement movement, Vector3 vector, float timeToMove)
            {
                this.movement = movement;
                this.vector = vector;
                this.timeToMove = timeToMove;
                lifeTime = timeToMove;
            }

            public PatternData(Movement movement, Quaternion quaternion, float timeToMove)
            {
                this.movement = movement;
                this.quaternion = quaternion;
                this.timeToMove = timeToMove;
                lifeTime = timeToMove;
            }

            public void Reset()
            {
                timeToMove = lifeTime;
            }
        }

        public enum Movement { Step, Turn }


        public GameObject predator;
        public float speed;
        public float rotateSpeed;
        public GameObject startPosition;

        private Rigidbody predatorBody;
        private PatternData[] pattern;
        private int currentStep;
    }

}