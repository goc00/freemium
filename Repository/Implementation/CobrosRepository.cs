using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PlanesCobros
    {
        public string Pais { get; set; }
        public string IdCobro { get; set; }
        public string Monto { get; set; }
        public string MedioPago { get; set; }
        public int IdPlan { get; set; }
        public string Moneda { get; set; }
        public int Principal { get; set; }
    }

    public class CobrosRepository
    {
        public List<PlanesCobros> GetCobros(int idAplicacion)
        {
            Wach.WachHelper server = new Wach.WachHelper("http://54.94.143.29/");

            List<PlanesCobros> cobros = new List<PlanesCobros>();

            try
            {
                //string subscripcion = server.Get<string>("subscriptions/Hhd7s56ag$/4");

                cobros = server.Get<List<PlanesCobros>>("servicio.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                cobros = null;
            }

            return cobros;
        }
    }
}
