using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BowArrow
{

    public class ArrowController : MonoBehaviour
    {

        [SerializeField] private float length = 0.7f;
        [SerializeField] private float cd = 1.8f;

        private Rigidbody rB;
        private float tempDragCalc;
        private float dragOffset;

        public Rigidbody RB { get => rB; }
        public float Length { get => length; }


        void Start()
        {
            const float csa = 0.00015f;
            rB = GetComponent<Rigidbody>();
            tempDragCalc = 1.292f * csa * 0.5f * cd;
            // distance behind the CoG for the application of drag, 40% of length works well
            dragOffset = length * 0.4f;
        }

        void FixedUpdate()
        {
            float vel = rB.velocity.magnitude;
            float drag = tempDragCalc * vel * vel;
            rB.AddForceAtPosition(rB.velocity.normalized * -drag, rB.transform.TransformPoint(rB.centerOfMass) - transform.forward * dragOffset);
        }

        public void Launch(float velocity)
        {
            transform.parent = null;
            rB.isKinematic = false;
            rB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rB.velocity = transform.forward * velocity;
        }


        void OnCollisionEnter(Collision collision)
        {
            Collider col = GetComponent<CapsuleCollider>();
            col.enabled = false;
            transform.parent = collision.gameObject.transform;
            rB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rB.isKinematic = true;
        }

    }
}