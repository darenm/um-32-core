using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Um32Core.Tests
{
    [TestClass]
    public class PlatterTests
    {
        [TestMethod]
        public void Platter_Initialize()
        {
            Platter p = new Platter(32376);
            Assert.AreEqual((uint)32376, p.Value);
        }

        [TestMethod]
        public void Platter_Initialize_Four_Bytes()
        {
            Platter p = new Platter(new byte[] { 255, 255, 255, 255 });
            uint magnificent = 4278190080;
            uint lovely = 16711680;
            uint mediocre = 65280;
            uint shoddy = 255;
            Assert.AreEqual(magnificent + lovely + mediocre + shoddy, p.Value);

            p = new Platter(new byte[] { 255, 0, 0, 0 });
            Assert.AreEqual(magnificent, p.Value);

            p = new Platter(new byte[] { 0, 255, 0, 0 });
            Assert.AreEqual(lovely, p.Value);

            p = new Platter(new byte[] { 0, 0, 255, 0 });
            Assert.AreEqual(mediocre, p.Value);

            p = new Platter(new byte[] { 0, 0, 0, 255 });
            Assert.AreEqual(shoddy, p.Value);

            p = new Platter(new byte[] { 0, 0, 0, 0 });
            Assert.AreEqual((uint)0, p.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Platter_Initialize_3_Bytes()
        {
            Platter p = new Platter(new byte[] { 255, 255, 255 });
        }

        [TestMethod]
        public void Platter_OperatorNumber()
        {
            Platter p = new Platter(new byte[] { 255, 255, 255, 255 });
            Assert.AreEqual((uint)15, p.OperatorNumber);

            p = new Platter(new byte[] { 0xF0, 255, 255, 255 });
            Assert.AreEqual((uint)15, p.OperatorNumber);

            p = new Platter(new byte[] { 0x90, 255, 255, 255 });
            Assert.AreEqual((uint)0x9, p.OperatorNumber);
        }


        [TestMethod]
        public void Platter_RegisterA()
        {
            Platter p = new Platter(new byte[] { 0, 0, 255, 255 });
            Assert.AreEqual((uint)7, p.RegisterA);

            p = new Platter(new byte[] { 0, 0, 1, 0 });
            Assert.AreEqual((uint)4, p.RegisterA);
        }

        [TestMethod]
        public void Platter_RegisterB()
        {
            Platter p = new Platter(new byte[] { 0, 0, 0, 255 });
            Assert.AreEqual((uint)7, p.RegisterB); 
            
            p = new Platter(new byte[] { 0, 0, 0, 40 });
            Assert.AreEqual((uint)5, p.RegisterB);
        }

        [TestMethod]
        public void Platter_RegisterC()
        {
            Platter p = new Platter(new byte[] { 0, 0, 0, 255 });
            Assert.AreEqual((uint)7, p.RegisterC);

            p = new Platter(new byte[] { 0, 0, 0, 4 });
            Assert.AreEqual((uint)4, p.RegisterC);
        }
    }
}
