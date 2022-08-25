using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore.Models
{
    public class Product_with_Filter
    {
        public PagedList.IPagedList<Product> productList { get; set; }
    }


    public class Productlist
    {
        public PagedList.IPagedList<Product> ProductList { get; set; }
    }
}