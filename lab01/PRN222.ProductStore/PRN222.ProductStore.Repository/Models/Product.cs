﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PRN222.ProductStore.Repository.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int CategoryId { get; set; }

    public short? UnitsInStock { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Category Category { get; set; }
}