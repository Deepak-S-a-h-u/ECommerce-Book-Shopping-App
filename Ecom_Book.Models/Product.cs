using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.Models
{
    public class Product
    {
        public int ID { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Discription { get; set; }
        
        [Required]
        public string ISBN { get; set; }
        
        public string Author { get; set; }
        
        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }
        
        [Required]
        [Range(1,10000)]
        public double Price50 { get; set; }
        
        [Required]
        [Range(1,10000)]
        public double Price100 { get; set; }
        
        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }
        
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        
        [Display(Name ="Category")]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [Display(Name ="CoverType")]
        public int CoverTypeID { get; set; }
        [ForeignKey("CoverTypeID")]
        public CoverType CoverType { get; set; }
    }
}
