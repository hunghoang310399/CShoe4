using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laptopshop.DAO
{
    public class OrderDetaillsDao
    {
        CShoeEntities db;
        public OrderDetaillsDao()
        {
            db = new CShoeEntities();
        }
        public IQueryable<OrderDetail> ChiTietGH(int Id)
        {
            var res = (from sp in db.OrderDetails where sp.Id == Id select sp);
            return res;
        }
    }
}