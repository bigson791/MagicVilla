using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    //<T> where T : class hace referencia a una interfaz generica
    public interface IRepositorio<T> where T : class
    {
        //Contrato donde unicamente se declaran los metodos
        //Crear recibe la entidad tipo T
        Task Crear(T entidad);

        //retorna una lista segun la entidad enviada
        Task<List<T>> ObtenerTodos(Expression<Func<T,bool>>? filtro = null);

        Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true);

        Task<T> Remover(T entidad);

        Task<T> Grabar();
    }
}
