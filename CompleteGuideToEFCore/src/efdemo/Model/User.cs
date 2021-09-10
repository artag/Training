using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Navigation property. Для одного/нескольких ExpenseHeader.
        public List<ExpenseHeader> RequesterExpenseHeaders { get; set; }

        // Navigation property. Для одного/нескольких ExpenseHeader.
        public List<ExpenseHeader> ApproverExpenseHeaders { get; set; }
    }
}
