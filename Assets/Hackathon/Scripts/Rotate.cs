using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
  
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up,50 * Time.deltaTime);
    }
}
