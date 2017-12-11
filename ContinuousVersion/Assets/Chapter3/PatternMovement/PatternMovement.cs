using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chapter3
{
    public class PatternMovement : MonoBehaviour
    {
        void Start()
        {
            entity = Instantiate(entity, startPosition.transform.position, Quaternion.identity);
            entityBody = entity.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = string.Format("velocity = {0}", entityBody.velocity);
        }


        [SerializeField]
        public GameObject entity;

        private Rigidbody entityBody;

        [SerializeField]
        public GameObject startPosition;

        [SerializeField]
        public Text text;
    }
}