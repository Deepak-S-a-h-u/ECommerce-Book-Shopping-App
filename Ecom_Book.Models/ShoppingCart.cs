using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            count = 1;
        }
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [Range(1,1000,ErrorMessage ="please enter a number between 1 to 1000")]
        public int count { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
