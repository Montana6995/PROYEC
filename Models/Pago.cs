namespace MVC.Models
{
    public class Pago
    {
        public int IdPago { get; set; }  // Clave primaria
        public int IdPuesto { get; set; }
        public int IdPersonal { get; set; }
        public int IdTipoServicio { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        // Relaciones
        public Puesto Puesto { get; set; }
        public Personal Personal { get; set; }
        public TipoServicio TipoServicio { get; set; }
    }

}
