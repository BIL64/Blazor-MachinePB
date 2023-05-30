using ServerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempFuncMachine.Entities;

namespace TempFuncMachine.Extensions
{
    public static class Mapper
    {
        public static MachineTable ToTableEntity(this Machine machine)
        {
            return new MachineTable
            {
                Number = machine.Number,
                DateTimeCreate = machine.DateTimeCreate,
                Location = machine.Location,
                Date = machine.Date,
                Type = machine.Type,
                Status = machine.Status,
                DataCode = machine.DataCode,
                Temp = machine.Temp,
                RowKey = machine.Id
            };
        }

        public static Machine ToMachine(this MachineTable machineTable)
        {
            return new Machine
            {
                Id = machineTable.RowKey,
                Number = machineTable.Number,
                DateTimeCreate = machineTable.DateTimeCreate,
                Location = machineTable.Location,
                Date = machineTable.Date,
                Type = machineTable.Type,
                Status = machineTable.Status,
                DataCode = machineTable.DataCode,
                Temp = machineTable.Temp,
            };
        }
    }
}
