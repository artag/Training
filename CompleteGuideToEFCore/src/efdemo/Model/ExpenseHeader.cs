using System;

namespace Model
{
    public class ExpenseHeader
    {
        // Имя Id - соглашение об наименовании Primary key в таблице.
        // EF - автоматически распознает это свойство как Primary key.
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? ExpenseDate { get; set; }
    }
}
