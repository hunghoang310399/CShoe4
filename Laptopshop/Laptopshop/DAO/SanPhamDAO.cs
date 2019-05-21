
using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laptopshop.DAO
{
    public class SanPhamDAO
    {
        CShoeEntities db;
        public SanPhamDAO()
        {
            db = new CShoeEntities();
        }

        public IQueryable<Product> ListSP()
        {
            var res = (from sp in db.Products select sp);
            return res;
        }

        //public IQueryable<Product> ListSP(int? dm)
        //{
        //    var res = (from sp in db.Products where sp.CategoryId==dm select sp);
        //    return res;
        //}
        //public IQueryable<Product> ListSPTL(int? tl)
        //{
        //    var res = (from sp in db.Products where sp.CategoryId == tl select sp);
        //    return res;
        //}
        
    }
}