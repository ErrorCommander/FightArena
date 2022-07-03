using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dueller : Fighter
{
    [SerializeField] private Transform _target;

    private void Start()
    {
        if (_target != null)
            FollowTo(_target);
    }

    protected override void Attack()
    {

    }
}
