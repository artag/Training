using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public decimal UsdExchangeRate { get; set; }

        // Navigation property.
        // One-to-many. Один ExpenseHeader содержит ссылки на множество ExpenseLine.
        public List<ExpenseLine> ExpenseLines { get; set; }

        // Id пользователя, который будет запрашивать этот expense.
        [ForeignKey("Requester")]
        public int RequesterId { get; set; }

        // Navigation property к пользователю, который будет запрашивать этот expense.
        // Inverse property указывает на свойство User.RequesterExpenseHeaders для связывания.
        [InverseProperty("RequesterExpenseHeaders")]
        public User Requester { get; set; }

        // Id пользователя, который будет approve (подтверждать) этот expense.
        [ForeignKey("Approver")]
        public int ApproverId { get; set; }

        // Navigation property к пользователю, который будет approve этот expense.
        // Inverse property указывает на свойство User.ApproverExpenseHeaders для связывания.
        [InverseProperty("ApproverExpenseHeaders")]
        public User Approver { get; set; }
    }
}
