using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BowArrow
{

    public class RotatingCube : MonoBehaviour
    {

        [SerializeField] private float rotationRate = 20;

        void Update()
        {
            transform.Rotate(new Vector3(0, rotationRate, 0) * Time.deltaTime);
        }

    }
}