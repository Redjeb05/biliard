using System.ComponentModel.DataAnnotations.Schema;

namespace biliard.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int TableId { get; set; }
        [ForeignKey("TableId")]
        public virtual Table? Table { get; set; }
        public DateTime DateAndHour { get; set; }

        public string Description { get; set; }
    }
}
