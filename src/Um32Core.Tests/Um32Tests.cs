using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Um32Core.Tests
{
    [TestClass]
    public class Um32Tests
    {
        [TestMethod]
        public void Um32_Initialize()
        {
            Um32 um32 = BuildUm32();

            Assert.AreEqual(1, um32.Platters.Count);
            Assert.AreEqual(7, um32.Platters[0].Length);
            Assert.AreEqual((uint)40, um32.Platters[0][3].Value);
        }

        private static Um32 BuildUm32()
        {
            var platters = new Platter[]
                {
                    new Platter(10),
                    new Platter(20),
                    new Platter(30),
                    new Platter(40),
                    new Platter(50),
                    new Platter(60),
                    new Platter(70),
                };
            var um32 = new Um32(platters);
            return um32;
        }

        [TestMethod]
        public void Um32_AssignRegister()
        {
            Um32 um32 = BuildUm32();

            um32.AssignRegister(0, 23);
            Assert.AreEqual((uint)23, um32.Registers[0]);

            um32.AssignRegister(5, 48);
            Assert.AreEqual((uint)48, um32.Registers[5]);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Um32_AssignRegister_OutOfRange()
        {
            Um32 um32 = BuildUm32();

            um32.AssignRegister(8, 23);
            Assert.AreEqual((uint)23, um32.Registers[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Um32_ReadRegister_OutOfRange()
        {
            Um32 um32 = BuildUm32();

            um32.AssignRegister(0, 23);
            Assert.AreEqual(um32.ReadRegister(8), um32.Registers[0]);
        }


        [TestMethod]
        public void Um32_ReadRegister()
        {
            Um32 um32 = BuildUm32();

            um32.AssignRegister(0, 23);
            Assert.AreEqual(um32.ReadRegister(0), um32.Registers[0]);

            um32.AssignRegister(5, 48);
            Assert.AreEqual(um32.ReadRegister(5), um32.Registers[5]);
        }

        [TestMethod]
        public void Um32_ReadNextPlatter()
        {
            Um32 um32 = BuildUm32();

            var platter = um32.ReadNextPlatter();
            Assert.AreEqual((uint)10, platter.Value);

            platter = um32.ReadNextPlatter();
            Assert.AreEqual((uint)10, platter.Value);
        }

        [TestMethod]
        public void Um32_AdvanceExecutionFinger()
        {
            Um32 um32 = BuildUm32();

            var platter = um32.ReadNextPlatter();
            Assert.AreEqual((uint)10, platter.Value);


            um32.AdvanceExecutionFinger();

            platter = um32.ReadNextPlatter();
            Assert.AreEqual((uint)20, platter.Value);
        }

        [TestMethod]
        public void Um32_ConditionalMove_True()
        {
            Um32 um32 = BuildUm32();
            uint aIndex = 0;
            uint bIndex = 1;
            uint cIndex = 7;
            um32.AssignRegister(aIndex, 0);  // A
            um32.AssignRegister(bIndex, 23); // B
            um32.AssignRegister(cIndex, 1);  // C
            var platter = new Platter(0)
                .SetOperation(OperatorConstants.ConditionalMove)
                .SetRegisterA(aIndex)
                .SetRegisterB(bIndex)
                .SetRegisterC(cIndex);

            um32.DischargeOperator(platter);

            Assert.AreEqual((uint)23, um32.ReadRegister(aIndex));
        }

        [TestMethod]
        public void Um32_ConditionalMove_False()
        {
            Um32 um32 = BuildUm32();
            uint aIndex = 0;
            uint bIndex = 1;
            uint cIndex = 7;
            um32.AssignRegister(aIndex, 0);  // A
            um32.AssignRegister(bIndex, 23); // B
            um32.AssignRegister(cIndex, 0);  // C
            var platter = new Platter(0)
                .SetOperation(OperatorConstants.ConditionalMove)
                .SetRegisterA(aIndex)
                .SetRegisterB(bIndex)
                .SetRegisterC(cIndex);

            um32.DischargeOperator(platter);

            Assert.AreEqual((uint)0, um32.ReadRegister(aIndex));
        }


        [TestMethod]
        public void Um32_Nand()
        {
            Um32 um32 = BuildUm32();
            uint aIndex = 0;
            uint bIndex = 1;
            uint cIndex = 7;
            um32.AssignRegister(aIndex, 0);  // A
            um32.AssignRegister(bIndex, 6); // B
            um32.AssignRegister(cIndex, 5);  // C
            var platter = new Platter(0)
                .SetOperation(OperatorConstants.NotAnd)
                .SetRegisterA(aIndex)
                .SetRegisterB(bIndex)
                .SetRegisterC(cIndex);

            um32.DischargeOperator(platter);

            Assert.AreEqual((uint)7, um32.ReadRegister(aIndex));
        }

    }
}
