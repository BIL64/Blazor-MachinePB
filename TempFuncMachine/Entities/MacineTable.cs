using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TempFuncMachine.Helpers;

namespace TempFuncMachine.Entities
{
    public class MachineTable : BaseTable
    {
        public int Number { get; set; }

        public DateTime DateTimeCreate { get; set; }

        public string Location { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool Status { get; set; }

        public string DataCode { get; set; } = string.Empty;

        public string Temp { get; set; } = string.Empty;
    }

    public class BaseTable : ITableEntity
    {
        public string PartitionKey { get; set; } = TableNames.PartionKey;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
