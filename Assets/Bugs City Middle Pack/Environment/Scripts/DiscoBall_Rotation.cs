using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall_Rotation : MonoBehaviour
{
    // Start is called before the first frame update

    float speed = 50.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
