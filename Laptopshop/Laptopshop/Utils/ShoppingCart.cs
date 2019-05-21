using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ShoppingCart
{
    /// <summary>
    /// Truy xuất giỏ hàng từ Session
    /// </summary>
    public static ShoppingCart Cart
    {
        get
        {
            var cart = HttpContext.Current.Session["Cart"] as ShoppingCart;
            if (cart == null)
            {
                cart = new ShoppingCart();
                HttpContext.Current.Session["Cart"] = cart;
            }
            return cart;
        }
    }

    public List<Product> Items = new List<Product>();

    /// <summary>
    /// Lấy số lượng hàng hóa trong giỏ
    /// </summary>
    public int Count
    {
        get
        {
            return Items.Sum(p => p.Quantity);
        }
    }

    /// <summary>
    /// Tính tổng số tiền của giỏ hàng
    /// </summary>
    public double Amount
    {
        get
        {
            return Items.Sum(p => p.Quantity * p.UnitPrice * (1 - p.Discount));
        }
    }

    /// <summary>
    /// Chọn hàng (bỏ hàng vào giỏ)
    /// </summary>
    /// <param name="Id">Mã mặt hàng được chọn</param>
    public void Add(int Id)
    {
        try
        {
            var p = Items.Single(i => i.Id == Id);
            p.Quantity++;
        }
        catch // Chưa có trong giỏ -> Lấy từ DB
        {
            using (var db = new CShoeEntities())
            {
                var p = db.Products.Find(Id);
                p.Quantity = 1;
                Items.Add(p);
            }
        }
    }

    /// <summary>
    /// Xóa hàng khỏi giỏ
    /// </summary>
    /// <param name="Id">Mã mặt hàng bị xóa</param>
    public void Remove(int Id)
    {
        var p = Items.Single(i => i.Id == Id);
        Items.Remove(p);
    }

    /// <summary>
    /// Cap nhat so luong
    /// </summary>
    /// <param name="Id">Ma mat hang can cap nhat</param>
    /// <param name="newQty">So luong moi</param>
    public void Update(int Id, int newQty)
    {
        var p = Items.Single(i => i.Id == Id);
        p.Quantity = newQty;
    }

    /// <summary>
    /// Xóa sạch giỏ hàng
    /// </summary>
    public void Clear()
    {
        Items.Clear();
    }
}