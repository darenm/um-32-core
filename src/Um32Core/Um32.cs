using System;
using System.Collections.Generic;

namespace Um32Core
{
    public class Um32Core
    {
        public uint[] Registers { get; private set; } = new uint[8];
        public Dictionary<uint, Platter[]> Platters { get; private set; } = new Dictionary<uint, Platter[]>();
        public bool IsExecuting { get; private set; } = false;

        public ExecutionFinger _executionFinger;

        public Um32Core(Platter[] program)
        {
            LoadPlatterArray(0, program);
            AssignRegister(0, 0);
            AssignRegister(1, 0);
            AssignRegister(2, 0);
            AssignRegister(3, 0);
            AssignRegister(4, 0);
            AssignRegister(5, 0);
            AssignRegister(6, 0);
            AssignRegister(7, 0);
            _executionFinger = new ExecutionFinger { ArrayId = 0, PlatterIndex = 0 };
            SpinCycle();
        }

        public void SpinCycle()
        {
            IsExecuting = true;

            while(IsExecuting)
            {
                var mu32Operator = ReadNextPlatter();
                AdvanceExecutionFinger();
                DischargeOperator(mu32Operator);
            }
        }

        public void DischargeOperator(Platter mu32Operator)
        {
            switch(mu32Operator.OperatorNumber)
            {
                case OperatorConstants.ConditionalMove:
                    break;
                default:
                    Fail($"Unrecognized operator {mu32Operator.OperatorNumber}");
                    break;
            }
        }

        public void AdvanceExecutionFinger()
        {
            if (Platters.ContainsKey(_executionFinger.ArrayId) && _executionFinger.PlatterIndex < Platters[_executionFinger.ArrayId].Length)
            {
                _executionFinger.PlatterIndex++;
            }
            else
            {
                Fail("Unable to advance the ExecutionFinger");
            }
        }

        public Platter ReadNextPlatter()
        {
            if (Platters.ContainsKey(_executionFinger.ArrayId) && _executionFinger.PlatterIndex < Platters[_executionFinger.ArrayId].Length)
            {
                return Platters[_executionFinger.ArrayId][_executionFinger.PlatterIndex];
            }
            else
            {
                Fail("Unable to advance the ExecutionFinger");
                return null;
            }
        }

        public void LoadPlatterArray(int arrayId, Platter[] program)
        {
            throw new NotImplementedException();
        }

        public void AbandonArray(uint arrayId)
        {
            if (arrayId == 0)
            {
                Fail("Abandoning '0' array");
            }

            if (Platters.ContainsKey(arrayId))
            {
                Platters.Remove(arrayId);
            }
            else
            {
                Fail($"Abandoning missing array {arrayId}");
            }
        }

        public void Fail(string message)
        {
            Console.WriteLine($"The machine has Failed - {message}");
            IsExecuting = false;
        }

        public void AssignRegister(int registerId, uint value)
        {
            if (registerId < 0)
            {
                throw new ArgumentOutOfRangeException("registerId", "value cannot be less than zero");
            }

            if (registerId > 7)
            {
                throw new ArgumentOutOfRangeException("registerId", "value cannot exceed 7");
            }

            Registers[registerId] = value;
        }
    }
}