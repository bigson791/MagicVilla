﻿using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {   
            //inyectando dependencias
            _db = db;
            this.dbSet = _db.Set<T>();
            
        }
        public async Task Crear(T entidad)
        {
            await dbSet.addAsync(entidad);
            await Grabar();
        }

        public async Task<T> Grabar()
        {
            await _db.SaveChangesAsync();   
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();

        }

        public async Task<T> Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }
    }
}
