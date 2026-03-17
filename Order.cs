using System;

namespace ABC_Car_Traders
{
    internal class Order
    {
        public int CustomerID { get; internal set; }
        public int OrderID { get; internal set; }
        public DateTime OrderDate { get; internal set; }
        public string Status { get; internal set; }
        public decimal TotalAmount { get; internal set; }
    }
}