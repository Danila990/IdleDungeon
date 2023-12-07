using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Offset;

    private void Awake()
    {
        if (Target != null) transform.position = Target.position + Offset;
    }

    private void Update()
    {
        if(Target!=null)transform.position = Target.position + Offset;
    }
}
