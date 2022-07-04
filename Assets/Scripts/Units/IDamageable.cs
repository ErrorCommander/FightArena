public interface IDamageable
{
    public float Health { get; }

    /// <summary>
    /// Applies damage taken.
    /// </summary>
    /// <param name="damage">Damage taken.</param>
    /// <returns>Whether death of destroy was caused by the given damage.</returns>
    public bool ApplyDamage(float damage);

}
