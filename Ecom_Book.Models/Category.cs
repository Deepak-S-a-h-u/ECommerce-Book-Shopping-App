﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.Models
{
   public class Category
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
