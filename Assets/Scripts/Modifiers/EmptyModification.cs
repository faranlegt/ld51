namespace Modifiers
{
    public class EmptyModification : Modification
    {
        public override void Apply(Stats stats) { }

        public override void Unapply(Stats stats) { }
    }
}