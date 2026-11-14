using System.Globalization;
using PetCare;

namespace PetCareWeb.Utilidades
{
    /// <summary>
    /// Formatos de pantalla: pesos colombianos, fechas y nombres legibles de los enumerados.
    /// </summary>
    public static class Formato
    {
        private static readonly CultureInfo Colombia = new("es-CO");

        /// <summary>Ejemplo: 82500 → "$82.500".</summary>
        public static string Moneda(decimal valor)
        {
            return valor.ToString("$#,##0", Colombia);
        }

        /// <summary>Ejemplo: "24/11/2026".</summary>
        public static string Fecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy", Colombia);
        }

        /// <summary>Ejemplo: "24/11/2026".</summary>
        public static string Fecha(DateOnly fecha)
        {
            return fecha.ToString("dd/MM/yyyy", Colombia);
        }

        /// <summary>Ejemplo: "24/11/2026 10:00".</summary>
        public static string FechaHora(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm", Colombia);
        }

        /// <summary>Ejemplo: "martes 24 de noviembre de 2026", con la primera letra en mayúscula.</summary>
        public static string FechaLarga(DateTime fecha)
        {
            string texto = fecha.ToString("dddd d 'de' MMMM 'de' yyyy", Colombia);
            return char.ToUpper(texto[0], Colombia) + texto[1..];
        }

        /// <summary>
        /// Asegura que el mensaje termine en punto, para poder agregarle otra frase.
        /// Ejemplo: "La factura está anulada" → "La factura está anulada.".
        /// </summary>
        public static string Oracion(string mensaje)
        {
            string texto = mensaje.Trim();

            if (texto.Length == 0 || texto.EndsWith('.') || texto.EndsWith('!') || texto.EndsWith('?'))
            {
                return texto;
            }

            return texto + ".";
        }

        /// <summary>Iniciales del nombre para el círculo del encabezado. Ejemplo: "Diana Gomez" → "DG".</summary>
        public static string Iniciales(string nombre)
        {
            string[] partes = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string iniciales = string.Concat(partes.Take(2).Select(p => char.ToUpper(p[0], Colombia)));
            return iniciales.Length == 0 ? "?" : iniciales;
        }

        /// <summary>Tamaño de la jaula como se dice en la clínica.</summary>
        public static string TextoTamano(TamanoJaula tamano)
        {
            return tamano switch
            {
                TamanoJaula.Pequeno => "Pequeña",
                TamanoJaula.Mediano => "Mediana",
                _ => "Grande"
            };
        }

        /// <summary>Tipo de medicamento en singular y con tilde.</summary>
        public static string TextoTipoMedicamento(TipoMedicamento tipo)
        {
            return tipo switch
            {
                TipoMedicamento.Antiparasitarios => "Antiparasitario",
                TipoMedicamento.Antibioticos => "Antibiótico",
                TipoMedicamento.Analgesicos => "Analgésico",
                _ => "Vacuna"
            };
        }

        /// <summary>Color de la etiqueta de estado de una cita.</summary>
        public static string TonoCita(EstadoCita estado)
        {
            return estado switch
            {
                EstadoCita.Atendida => "ok",
                EstadoCita.Cancelada => "neutro",
                _ => "alerta"
            };
        }

        /// <summary>Rol como se muestra en el encabezado.</summary>
        public static string TextoRol(string? rol)
        {
            return rol == nameof(RolUsuario.Administrador) ? "Administradora" : "Recepcionista";
        }
    }
}
