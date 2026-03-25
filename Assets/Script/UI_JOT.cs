using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JOT : MonoBehaviour
{
    Vector3 offset;
    Quaternion fixed_UI;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - transform.parent.position;
        fixed_UI = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + offset;
        transform.rotation = fixed_UI;
    }
}
