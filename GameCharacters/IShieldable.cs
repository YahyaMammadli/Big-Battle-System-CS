namespace GameCharacters
{
    public interface IShieldable
    {
        bool HasShield { get; }
        int ShieldReduction { get; }
        void ActivateShield();
        void DeactivateShield();
    }
}