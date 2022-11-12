using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 directionToCam = (transform.position - Camera.main.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToCam);
    }
}
