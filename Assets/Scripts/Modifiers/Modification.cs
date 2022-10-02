namespace Modifiers
{
    public abstract class Modification
    {
        public abstract void Apply(Stats stats);
        
        public abstract void Unapply(Stats stats);
    }
}