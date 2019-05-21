using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laptopshop.DAO
{
    public class UserDao
    {
        CShoeEntities db = null;
        public UserDao()
        {
            db = new CShoeEntities();
        }
        public string InsertForFacebook(Customer entity)
        {
           
            var user = db.Customers.SingleOrDefault(x => x.Fullname == entity.Fullname);
            if (user == null)
            {
                db.Customers.Add(entity);
                db.SaveChanges();
                return entity.Id;
            }
            else
            {
                return user.Id;
            }

        }
    }

}