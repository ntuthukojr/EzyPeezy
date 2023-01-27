using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzyPeezy.Models
{
    [Serializable]
    public class Cart
    {
        public List<ProductDictionary> Products { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total Cost")]
        public decimal TotalCost
        {
            get
            {
                decimal total = Products.Sum(x => x.Key.Price * x.Value);
                return Decimal.Parse(total.ToString("0.00"));
            }
        }


        public Cart()
        {
            this.Products = new List<ProductDictionary>();
        }

        public Boolean Add(Product product)
        {
            if (product.InStock)
            {
                ProductDictionary existing = Products.Find(p => p.Key.Name == product.Name);

                if (existing != null && product.Quantity - 1 >= 0)
                {
                    existing.Value++;
                }
                else
                {
                    Products.Add(new ProductDictionary { Key = product, Value = 1 });
                }

                return true;
            }

            return false;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            foreach (ProductDictionary p in Products)
            {
                sb.AppendFormat("{0} x {1} @ R{2} each\n", p.Value, p.Key.Name, p.Key.Price);
            }

            return sb.ToString();
        }
    }
}