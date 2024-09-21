using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class Variables : MonoBehaviour
    {
        public enum Suits
        {
            None,
            Spade,
            Club,
            Heart,
            Diamond
        }
        public enum CardColor
        { 
            None,
            Red,
            Black
        }

        public enum CardTypes
        {
            None,
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }
        public static readonly string Loading = "Loading";
    }
}