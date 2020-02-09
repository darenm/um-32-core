using System;
using System.Collections.Generic;
using System.Linq;

namespace Um32Core
{
    public class Um32
    {
        public uint[] Registers { get; private set; } = new uint[8];
        public Dictionary<uint, Platter[]> Platters { get; private set; } = new Dictionary<uint, Platter[]>();
        public bool IsExecuting { get; private set; } = false;

        public ExecutionFinger ExecutionFinger { get; private set; }

        public Um32(Platter[] program)
        {
            AllocatePlatterArray(0, program);
            AssignRegister(0, 0);
            AssignRegister(1, 0);
            AssignRegister(2, 0);
            AssignRegister(3, 0);
            AssignRegister(4, 0);
            AssignRegister(5, 0);
            AssignRegister(6, 0);
            AssignRegister(7, 0);
            ExecutionFinger = new ExecutionFinger { ArrayId = 0, PlatterIndex = 0 };
        }

        public void SpinCycle()
        {
            IsExecuting = true;

            while(IsExecuting)
            {
                var um32Operator = ReadNextPlatter();
                AdvanceExecutionFinger();
                DischargeOperator(um32Operator);
            }
        }

        public void DischargeOperator(Platter um32Operator)
        {
            switch(um32Operator.OperatorNumber)
            {
                case OperatorConstants.ConditionalMove:
                    Logger.WriteInfo($"ConditionalMove: if {ReadRegister(um32Operator.RegisterC)} then {um32Operator.RegisterA} = {ReadRegister(um32Operator.RegisterB)}");

                    if (ReadRegister(um32Operator.RegisterC) != 0)
                    {
                        AssignRegister(
                            um32Operator.RegisterA, 
                            ReadRegister(um32Operator.RegisterB));
                    }
                    break;

                case OperatorConstants.ArrayIndex:
                    Logger.WriteInfo("ArrayIndex");

                    AssignRegister(
                        um32Operator.RegisterA, 
                        ReadArrayIndex(
                            ReadRegister(um32Operator.RegisterB), 
                            ReadRegister(um32Operator.RegisterC)));
                    break;

                case OperatorConstants.ArrayAmendment:
                    Logger.WriteInfo("ArrayAmendment");

                    WriteArrayIndex(
                        ReadRegister(um32Operator.RegisterA),
                        ReadRegister(um32Operator.RegisterB),
                        ReadRegister(um32Operator.RegisterC));
                    break;

                case OperatorConstants.Addition:
                    Logger.WriteInfo("Addition");

                    AssignRegister(um32Operator.RegisterA, ReadRegister(um32Operator.RegisterB) + ReadRegister(um32Operator.RegisterC));
                    break;

                case OperatorConstants.Multiplication:
                    Logger.WriteInfo("Multiplication");

                    AssignRegister(um32Operator.RegisterA, ReadRegister(um32Operator.RegisterB) * ReadRegister(um32Operator.RegisterC));
                    break;

                case OperatorConstants.Division:
                    Logger.WriteInfo("Division");

                    AssignRegister(um32Operator.RegisterA, ReadRegister(um32Operator.RegisterB) / ReadRegister(um32Operator.RegisterC));

                    break;

                case OperatorConstants.NotAnd:
                    Logger.WriteInfo("NotAnd");

                    AssignRegister(um32Operator.RegisterA, ~(ReadRegister(um32Operator.RegisterB) & ReadRegister(um32Operator.RegisterC)));

                    break;

                case OperatorConstants.Halt:
                    Logger.WriteInfo("Halt");

                    IsExecuting = false;

                    break;

                case OperatorConstants.Allocation:
                    Logger.WriteInfo("Allocation");

                    var numberOfPlatters = ReadRegister(um32Operator.RegisterC);
                    var newPlatters = new List<Platter>();
                    for (int i = 0; i < numberOfPlatters; i++)
                    {
                        newPlatters.Add(new Platter(0));
                    }

                    var newArrayId = Platters.Keys.Max() + 1;
                    AllocatePlatterArray(newArrayId, newPlatters.ToArray());
                    AssignRegister(um32Operator.RegisterB, newArrayId);

                    break;

                case OperatorConstants.Abandonment:
                    Logger.WriteInfo("Abandonment");

                    AbandonPlatterArray(ReadRegister(um32Operator.RegisterC));

                    break;

                case OperatorConstants.Output:

                    char c = (char)ReadRegister(um32Operator.RegisterC);
                    Console.Write(c);

                    break;

                case OperatorConstants.Input:
                    Logger.WriteInfo("Input");

                    var x = Console.Read();
                    if (x == -1)
                    {
                        AssignRegister(um32Operator.RegisterC, 0xFFFFFFFF);
                    }
                    else
                    {
                        AssignRegister(um32Operator.RegisterC, (uint)x);
                    }

                    break;

                case OperatorConstants.LoadProgram:
                    Logger.WriteInfo("LoadProgram");

                    var sourceArrayId = ReadRegister(um32Operator.RegisterB);
                    var newExecutionOffset = ReadRegister(um32Operator.RegisterC);

                    var duplicatePlatters = new List<Platter>();
                    foreach(var platter in Platters[sourceArrayId])
                    {
                        duplicatePlatters.Add(new Platter(platter.Value));
                    }

                    ExecutionFinger = new ExecutionFinger { ArrayId = 0, PlatterIndex = (int)newExecutionOffset };

                    Platters[0] = duplicatePlatters.ToArray();

                    break;


                case OperatorConstants.Orthography:
                    Logger.WriteInfo("Orthography");

                    AssignRegister(um32Operator.OrthograhyRegister, um32Operator.OrthograhyValue);

                    break;


                default:
                    Fail($"Unrecognized operator {um32Operator.OperatorNumber}");
                    break;
            }
        }

        public void AdvanceExecutionFinger()
        {
            if (Platters.ContainsKey(ExecutionFinger.ArrayId) && ExecutionFinger.PlatterIndex < Platters[ExecutionFinger.ArrayId].Length)
            {
                ExecutionFinger.PlatterIndex++;
            }
            else
            {
                Fail("Unable to advance the ExecutionFinger");
            }
        }

        public Platter ReadNextPlatter()
        {
            if (Platters.ContainsKey(ExecutionFinger.ArrayId) && ExecutionFinger.PlatterIndex < Platters[ExecutionFinger.ArrayId].Length)
            {
                return Platters[ExecutionFinger.ArrayId][ExecutionFinger.PlatterIndex];
            }
            else
            {
                Fail("Unable to advance the ExecutionFinger");
                return null;
            }
        }

        public void AllocatePlatterArray(uint arrayId, Platter[] array)
        {
            Platters.Add(arrayId, array);
        }

        public void AbandonPlatterArray(uint arrayId)
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

        public void AssignRegister(uint registerId, uint value)
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

        public uint ReadRegister(uint registerId)
        {
            if (registerId < 0)
            {
                throw new ArgumentOutOfRangeException("registerId", "value cannot be less than zero");
            }

            if (registerId > 7)
            {
                throw new ArgumentOutOfRangeException("registerId", "value cannot exceed 7");
            }

            return Registers[registerId];
        }

        public uint ReadArrayIndex(uint arrayId, uint index)
        {
            if (!Platters.ContainsKey(arrayId))
            {
                Fail($"Attempting to read from abdandoned array {arrayId}");
                return 0;
            }

            if (index >= Platters[arrayId].Length)
            {
                Fail($"Attempting to read out of bounds {arrayId} : {index}");
                return 0;
            }

            return Platters[arrayId][index].Value;
        }

        public void WriteArrayIndex(uint arrayId, uint index, uint value)
        {
            if (!Platters.ContainsKey(arrayId))
            {
                Fail($"Attempting to read from abandoned array {arrayId}");
            }

            if (index >= Platters[arrayId].Length)
            {
                Fail($"Attempting to read out of bounds {arrayId} : {index}");
            }

            Platters[arrayId][index] = new Platter(value);
        }
    }
}