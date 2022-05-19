using System;

namespace CharacterStatus
{
    public static class Character
    {
        public enum PoseStatus
        {
            Idle = 0,
            Walking = 1,
            Turn = 2
        }

        public enum FacingDirection
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3
        }
    }



}