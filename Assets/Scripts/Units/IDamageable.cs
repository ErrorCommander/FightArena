public interface IDamageable
{
    public float Health { get; }

    /// <summary>Applies damage taken.</summary>
    /// <param name="damage"> Damage taken.</param>
    public void ApplyDamage(float damage);
}
