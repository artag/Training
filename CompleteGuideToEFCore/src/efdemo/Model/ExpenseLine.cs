using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class ExpenseLine
    {
        public int ExpenseLineId { get; set; }

        public string Description { get; set; }

        [Range(1, 10, ErrorMessage = "{0} must be between 1 and 10")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(16, 2)")]
        [Range(0.01, 100.0, ErrorMessage = "Unit Cost must be between 0.01 and 100.00")]
        public decimal UnitCost { get; set; }

        [NotMapped]
        public string Secret { get; set; }

        // Foreign key to ExpenseHeader Id
        public int ExpenseHeaderId { get; set; }

        // Navigation property.
        // One-to-many. Один ExpenseHeader содержит ссылки на множество ExpenseLine.
        public ExpenseHeader ExpenseHeader { get; set; }
    }
}
