namespace InfiniteFusionHairStudio.Core
{
    public enum HairColor
    {
        Brown,
        Blonde,
        Black,
        Purple,
        Red,
        Pink,
        Blue,
        Green,
        White,
        Silver,
        Orange
    }

    public enum HairShade
    {
        Light,
        Normal,
        Dark
    }

    public class HairPalette
    {
        public HairColor Color { get; set; }

        public HairShade Shade { get; set; }

        public override string ToString()
        {
            return $"{Shade} {Color}";
        }
    }
}