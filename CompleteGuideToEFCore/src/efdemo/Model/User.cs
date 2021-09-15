using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace Model
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName { get; set; }

        // Поля для tracking changes in the database.
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }

        // Navigation property. Для одного/нескольких ExpenseHeader.
        public List<ExpenseHeader> RequesterExpenseHeaders { get; set; }

        // Navigation property. Для одного/нескольких ExpenseHeader.
        public List<ExpenseHeader> ApproverExpenseHeaders { get; set; }
    }
}
