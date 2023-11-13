using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointOffset : MonoBehaviour
{
    private Vector3 _offset;
    void Start()
    {
        _offset = transform.position - transform.parent.position;
    }

    void Update()
    {
        transform.position = transform.parent.position + _offset;
    }
}
