namespace SmallRPGLibrary.Entities.Impl.UnitFeatures
{
    public class AttackParameters
    {
        public string Name { get; set; }
        public double Damage { get; set; }

        public AttackParameters(string name, double damage)
        {
            Name = name;
            Damage = damage;
        }
    }
}