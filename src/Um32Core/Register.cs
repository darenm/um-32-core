using System;

namespace Um32Core
{
    public struct Register : IComparable
    {
        private byte _value;

        public static readonly Register MinValue = new Register { _value = 0 };
        public static readonly Register MaxValue = new Register { _value = 7 };
        public static readonly Register Mask = 0x7;

        private Register(byte r)
        {
            if (r < Register.MinValue)
                throw new ArgumentOutOfRangeException("r", "Value cannot be less then Register.MinValue");
            if (r > Register.MaxValue)
                throw new ArgumentOutOfRangeException("r", "Value cannot be more then Register.MaxValue");

            _value = r;
        }

        public static implicit operator Register(byte r) => new Register(r);

        public static implicit operator byte(Register r) => r._value;

        public int CompareTo(object obj) => _value.CompareTo(obj);

        public static Register operator +(Register a) => a;
        public static Register operator -(Register a) => a;

        public static Register operator +(Register a, Register b)
            => new Register((a+b) & Mask);

        public static Register operator &(Register a, Register b) => (byte)(a._value & b._value);

        //public static Fraction operator -(Fraction a, Fraction b)
        //    => a + (-b);

        //public static Fraction operator *(Fraction a, Fraction b)
        //    => new Fraction(a.num * b.num, a.den * b.den);

        //public static Fraction operator /(Fraction a, Fraction b)
        //{
        //    if (b.num == 0)
        //    {
        //        throw new DivideByZeroException();
        //    }
        //    return new Fraction(a.num * b.den, a.den * b.num);
        //}

        // operators for other numeric types...

        public override string ToString() => _value.ToString();

        // override Equals, HashCode, etc...

        public override bool Equals(object obj) => _value.Equals(obj);

        public override int GetHashCode() => _value.GetHashCode();  
    }

}