using UnityEngine;

public interface IMovable
{
    public float MoveSpeed { get; }

    /// <summary>Send the unit to move to a given point in space.</summary>
    /// <param name="pointTarget">Coordinates of a point in space.</param>
    public void MoveTo(Vector3 pointTarget);

    /// <summary>Assign the unit a target to follow.</summary>
    /// <param name="target"> Target to follow.</param>
    public void FollowTo(Transform target);

    /// <summary>Cancels following the current target.</summary>
    public void StopFollow();
}
