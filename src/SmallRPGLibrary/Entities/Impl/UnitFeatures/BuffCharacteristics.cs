namespace SmallRPGLibrary.Entities.Impl.UnitFeatures
{
    public class BuffCharacteristics : Characteristics
    {
        public BuffCharacteristics()
        {
            DamageMultiplier = 1;
            HealMultiplier = 1;
        }

        public BuffCharacteristics(Characteristics characteristics) : this()
        {
            AttackPower = characteristics.AttackPower;
            MagicPower = characteristics.MagicPower;
            Speed = characteristics.Speed;
            Stamina = characteristics.Stamina;
            Strength = characteristics.Strength;
            ReceivedHealthMultiplier = characteristics.ReceivedHealthMultiplier;
        }
        public double DamageMultiplier { get; set; }
        public double HealMultiplier { get; set; }

        public static BuffCharacteristics operator +(BuffCharacteristics left, BuffCharacteristics right)
        {
            return new BuffCharacteristics((Characteristics)left + (Characteristics)right)
            {
                DamageMultiplier = left.DamageMultiplier * right.DamageMultiplier,
                HealMultiplier = left.HealMultiplier * right.HealMultiplier
            };
        }

        public static BuffCharacteristics operator +(BuffCharacteristics left, Characteristics right)
        {
            return new BuffCharacteristics((Characteristics)left + right)
            {
                DamageMultiplier = left.DamageMultiplier,
                HealMultiplier = left.HealMultiplier
            };
        }
    }
}