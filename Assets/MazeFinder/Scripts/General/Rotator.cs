using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.right);
    }
}
