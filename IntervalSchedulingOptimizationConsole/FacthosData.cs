using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalSchedulingOptimization
{
    public class FacthosData
    {
        [JsonProperty("tipo_Rec_Ag")]
        public string TipoRecAg { get; set; }

        [JsonProperty("recurso_Agendable_ID")]
        public int RecursoAgendableID { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("servicio_ID")]
        public int ServicioID { get; set; }

        [JsonProperty("descServicio")]
        public string DescServicio { get; set; }

        [JsonProperty("dia")]
        public string Dia { get; set; }

        [JsonProperty("diaSemanaID")]
        public int DiaSemanaID { get; set; }

        [JsonProperty("fecha_Vig_Desde")]
        public DateTime FechaVigDesde { get; set; }

        [JsonProperty("fecha_Hasta")]
        public DateTime? FechaHasta { get; set; }

        [JsonProperty("hora_Desde")]
        public string HoraDesde { get; set; }

        [JsonProperty("hora_Hasta")]
        public string HoraHasta { get; set; }

        [JsonProperty("minutos")]
        public int Minutos { get; set; }

        [JsonProperty("cant_Sobre_Turnos")]
        public int CantSobreTurnos { get; set; }

        [JsonProperty("consultorio_ID")]
        public int ConsultorioID { get; set; }

        [JsonProperty("nombre_Consultorio")]
        public string NombreConsultorio { get; set; }

        [JsonProperty("sitio")]
        public string Sitio { get; set; }

        [JsonProperty("tilde_Aviso")]
        public string TildeAviso { get; set; }

        [JsonProperty("observaciones")]
        public object Observaciones { get; set; }

        [JsonProperty("en_Portal")]
        public object EnPortal { get; set; }

        [JsonProperty("atiende1Era_Vez")]
        public object Atiende1EraVez { get; set; }

        [JsonProperty("column20")]
        public object Column20 { get; set; }
    }

    public static class FacthosDataExtensions
    {
        public static Interval ToInterval(this FacthosData facthosData, int slots) =>
            new(
                TimeSpan.ParseExact(facthosData.HoraDesde, @"h\:m\:s", null).Hours * slots / 24,
                TimeSpan.ParseExact(facthosData.HoraHasta, @"h\:m\:s", null).Hours * slots / 24
            );
    }
}
