using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Framework
{
    public class Enums
    {
        public enum ToeknStatus
        {
            Base,
            Home,
            Safe,
            Risk
        }

        public enum PlayerResult
        {
            InProgress  = -1,
            First = 1,
            Second = 2,
            Third =3,
            Forth = 4
        }

        public enum PlayerStatus
        {
            Playing = 1,
            LeftInBetween = 2,
            LeftAtTheEnd =3
        }

    }
}
