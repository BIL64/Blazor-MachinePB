using System.ComponentModel.DataAnnotations;

namespace ServerAPI.Entities
{
    public class Machine
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        public int Number {  get; set; }

        public DateTime DateTimeCreate { get; set; }

        [StringLength(40, ErrorMessage = "Max 40 characters...")]
        public string Location { get; set; } = string.Empty;

        [StringLength(12, ErrorMessage = "Max 12 characters...")]
        public string Date { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Max 20 characters")]
        public string Type { get; set; } = string.Empty;

        public bool Status { get; set; }

        public string DataCode { get; set; } = string.Empty;

        public string Temp { get; set; } = string.Empty;
    }
}