//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShoppingStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.Order_Details = new HashSet<Order_Details>();
        }
    
        public int Order_ID { get; set; }
        public Nullable<int> Total_Amount { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> Order_Date { get; set; }
        public Nullable<int> Payment_Made { get; set; }
        public string Order_Status { get; set; }
    
        public virtual ICollection<Order_Details> Order_Details { get; set; }
        public virtual User User { get; set; }
    }
}
