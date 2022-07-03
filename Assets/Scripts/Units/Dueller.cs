using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dueller : Fighter
{
    [SerializeField] private Transform _target;

    private void Start()
    {
        FollowTo(_target);
    }

    protected override void Attack()
    {

    }
}
