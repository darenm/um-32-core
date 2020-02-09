using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Um32Core.Tests
{
    [TestClass]
    public class PlatterHelperTests
    {
        [TestMethod]
        public void Platter_RegisterA()
        {
            Platter p = new Platter(new byte[] { 0, 0, 255, 255 });
            Assert.AreEqual((uint)7, p.RegisterA);
            p = new Platter(new byte[] { 0, 0, 1, 0 });
            Assert.AreEqual((uint)4, p.RegisterA);

            var newP = p.SetRegisterA(5);
            Assert.AreEqual((uint)5, newP.RegisterA);

        }

        [TestMethod]
        public void Platter_RegisterB()
        {
            Platter p = new Platter(new byte[] { 0, 0, 0, 255 });
            Assert.AreEqual((uint)7, p.RegisterB);
            p = new Platter(new byte[] { 0, 0, 0, 40 });
            Assert.AreEqual((uint)5, p.RegisterB);

            var newP = p.SetRegisterB(6);
            Assert.AreEqual((uint)6, newP.RegisterB);

        }

        [TestMethod]
        public void Platter_RegisterC()
        {
            Platter p = new Platter(new byte[] { 0, 0, 0, 255 });
            Assert.AreEqual((uint)7, p.RegisterC);
            p = new Platter(new byte[] { 0, 0, 0, 4 });
            Assert.AreEqual((uint)4, p.RegisterC);

            var newP = p.SetRegisterC(3);
            Assert.AreEqual((uint)3, newP.RegisterC);
        }


        [TestMethod]
        public void Platter_Operation()
        {
            var p = new Platter(new byte[] { 0xF0, 255, 255, 255 });
            Assert.AreEqual((uint)15, p.OperatorNumber);

            var newP = p.SetRegisterA(3).SetRegisterB(6).SetRegisterC(2);
            Assert.AreEqual((uint)3, newP.RegisterA);
            Assert.AreEqual((uint)6, newP.RegisterB);
            Assert.AreEqual((uint)2, newP.RegisterC);
            Assert.AreEqual((uint)15, newP.OperatorNumber);

            newP = p.SetRegisterA(3).SetRegisterB(6).SetRegisterC(2).SetOperation(11);
            Assert.AreEqual((uint)3, newP.RegisterA);
            Assert.AreEqual((uint)6, newP.RegisterB);
            Assert.AreEqual((uint)2, newP.RegisterC);
            Assert.AreEqual((uint)11, newP.OperatorNumber);
        }
    }
}
