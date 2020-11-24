using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Framework
{
    public class NumberEventArgs:EventArgs
    {
        private int _number;
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }
        public NumberEventArgs(int num)
        {
            _number = num;
        }
    }
}
