﻿using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Villa> Villas { get; set; }
    }
}
