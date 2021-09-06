using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ExpenseHeader
    {
        // Имя Id - соглашение об наименовании Primary key в таблице.
        // EF - автоматически распознает это свойство как Primary key.
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        public string Description { get; set; }

        public DateTime? ExpenseDate { get; set; }
    }
}
