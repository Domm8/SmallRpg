namespace SmallRPGLibrary.Entities.Impl.UnitFeatures
{
    public class Characteristics
    {
        public double AttackPower { get; set; }
        public double Stamina { get; set; }
        public double Strength { get; set; }
        public double Speed { get; set; }
        public double MagicPower { get; set; }
        public double ReceivedHealthMultiplier { get; set; }

        public static Characteristics operator +(Characteristics left, Characteristics right)
        {
            return new Characteristics
            {
                AttackPower = left.AttackPower + right.AttackPower,
                MagicPower = left.MagicPower + right.MagicPower,
                Speed = left.Speed + right.Speed,
                Stamina = left.Stamina + right.Stamina,
                Strength = left.Strength + right.Strength,
                ReceivedHealthMultiplier = left.ReceivedHealthMultiplier + right.ReceivedHealthMultiplier,
            };
        }
        
        public static Characteristics operator *(Characteristics left, Characteristics right)
        {
            return new Characteristics
            {
                AttackPower = ZeroProtectedMultiplier(left.AttackPower,right.AttackPower),
                MagicPower = ZeroProtectedMultiplier(left.MagicPower,right.MagicPower),
                Speed = ZeroProtectedMultiplier(left.Speed, right.Speed),
                Stamina = ZeroProtectedMultiplier(left.Stamina, right.Stamina),
                Strength = ZeroProtectedMultiplier(left.Strength, right.Strength),
                ReceivedHealthMultiplier = ZeroProtectedMultiplier(left.ReceivedHealthMultiplier, right.ReceivedHealthMultiplier),
            };
        }

        private static double ZeroProtectedMultiplier(double left, double right)
        {
            if (left <= 0)
            {
                return right;
            }
            if (right <= 0)
            {
                return left;
            }
            return left * right;
        }

        public double GetRecievedHealthWithMultipliers(double health)
        {
            return health + ReceivedHealthMultiplier*health;
        }
    }
}