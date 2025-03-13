﻿using System;
using System.Collections.Generic;

namespace PRN222.Lab2.ProductStore.Repository.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
