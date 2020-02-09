using System;
using System.Collections.Generic;
using System.Text;

namespace Um32Core.Tests
{
    public static class PlatterHelper
    {
        public static Platter SetOperation(this Platter platter, uint mu32operator)
        {
            var shiftedOperator = mu32operator << 28;
            var maskedValue = platter.Value & 0x0FFFFFFF;
            return new Platter(maskedValue + shiftedOperator);
        }

        public static Platter SetRegisterA(this Platter platter, uint value)
        {
            var shiftedValue = value << 6;
            var maskedValue = platter.Value & 0xFFFFFE3F;
            return new Platter(maskedValue + shiftedValue);
        }

        public static Platter SetRegisterB(this Platter platter, uint value)
        {
            var shiftedValue = value << 3;
            var maskedValue = platter.Value & 0xFFFFFFC7;
            return new Platter(maskedValue + shiftedValue);
        }

        public static Platter SetRegisterC(this Platter platter, uint value)
        {
            var shiftedValue = value ;
            var maskedValue = platter.Value & 0xFFFFFFF8;
            return new Platter(maskedValue + shiftedValue);
        }
    }
}
