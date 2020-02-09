using System;
using System.Collections.Generic;
using System.Text;

namespace Um32Core
{
    public class Platter
    {
        public uint Value { get; private set; }

        public Platter(uint value)
        {
            Value = value;
        }

        public Platter(byte a, byte b, byte c, byte d) : this(new byte[] { a, b, c, d})
        { }

        public Platter(byte[] values)
        {
            if (values.Length != 4)
            {
                throw new ArgumentOutOfRangeException("values", "must be 4 bytes");
            }

            Value = (uint)(values[0] << 24) +
                    (uint)(values[1] << 16) +
                    (uint)(values[2] << 8) +
                    (uint)(values[3]);
        }

        public uint OperatorNumber => (Value & 0xF0000000) >> 28;

        public uint RegisterA => (Value & 0x1C0) >> 6;
        public uint RegisterB => (Value & 0x38) >> 3;
        public uint RegisterC => Value & 0x07;

        public uint OrthograhyRegister => (Value & 0x0E000000) >> 25;
        public uint OrthograhyValue => Value & 0x01FFFFFF;

        public override string ToString()
        {
            if (OperatorNumber == OperatorConstants.Orthography)
            {
                return $"Op: {OperatorNumber}, OReg: {OrthograhyRegister}, OVal: {OrthograhyValue}";
            }

            return $"Op: {OperatorNumber}, A: {RegisterA}, B: {RegisterB}, C: {RegisterC}";
        }
    }
}
