using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ExpenseHeader
    {
        // Имя Id - соглашение об наименовании Primary key в таблице.
        // EF - автоматически распознает это свойство как Primary key.
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(100, ErrorMessage = "{0} can not be more than 100 characters")]
        [MinLength(10)]
        public string Description { get; set; }

        public DateTime? ExpenseDate { get; set; }

        // Navigation property.
        // One-to-many. Один ExpenseHeader содержит ссылки на множество ExpenseLine.
        public List<ExpenseLine> ExpenseLines { get; set; }
    }
}
