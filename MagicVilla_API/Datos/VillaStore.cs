using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
    public static class VillaStore

    {
        public static List<villaDto> villaList = new List<villaDto>
        {
             new villaDto{ Id=1, Nombre="Vista a la piscina", Ocupantes=3, MetrosCuadrados=50},
             new villaDto{ Id=2, Nombre="Vista a la playa", Ocupantes=4, MetrosCuadrados=80}
        };
    }
    
}
    
