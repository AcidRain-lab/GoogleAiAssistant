using System;

namespace WebObjectsBLL.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public double Quantity { get; set; }
        public int ActionType { get; set; }
        public double Balance { get; set; }
    }
}
