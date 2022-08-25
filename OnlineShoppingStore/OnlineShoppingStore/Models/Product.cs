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
    
    public partial class Product
    {
        public Product()
        {
            this.Order_Details = new HashSet<Order_Details>();
        }
    
        public int Product_id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Price { get; set; }
        public string Picture { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public Nullable<int> Available_Stock { get; set; }
        public Nullable<int> Category_ID { get; set; }
        public PagedList.IPagedList<Product> productList { get; set; }

        public virtual Category Category1 { get; set; }
        public virtual ICollection<Order_Details> Order_Details { get; set; }
     
    }
}