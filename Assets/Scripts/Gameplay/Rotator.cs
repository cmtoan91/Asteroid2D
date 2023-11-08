using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    Vector3 _rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime);
    }
}
