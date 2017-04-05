namespace ToyCardGameLibrary.ActionCards
{
    public class InstantDamage : ActionCard
    {       
        public int SplashDamage { get; set; }
        public int PrimaryTargetDamage { get; set; }
        /// <summary>
        /// How many many minions are affected in the splash area to each side of the primary target.
        /// For example 2 would mean that the 2 toys on the left of the primary target and the 2 toys on the right of the primary target would be affected.
        /// </summary>
        public int SplashRadius { get; set; }
    }

}
