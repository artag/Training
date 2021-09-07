using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("ExpenseDetails")]
    public class ExpenseLine
    {
        public int ExpenseLineId { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        [Column("UnitPrice", TypeName = "decimal(16, 2)")]
        public decimal UnitCost { get; set; }
    }
}