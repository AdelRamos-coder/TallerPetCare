namespace PetCareWeb.Utilidades
{
    /// <summary>
    /// Datos de contacto de la clínica que muestra la página pública.
    /// Se leen de la sección "Clinica" de appsettings.json.
    /// </summary>
    public sealed class OpcionesClinica
    {
        /// <summary>Nombre de la sección en appsettings.json.</summary>
        public const string Seccion = "Clinica";

        public string Nombre { get; set; } = "PetCare";

        /// <summary>Número de WhatsApp con indicativo de país, por ejemplo 573001234567.</summary>
        public string WhatsApp { get; set; } = string.Empty;

        /// <summary>Número como se le muestra al público, por ejemplo +57 300 123 4567.</summary>
        public string TelefonoVisible { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string Horario { get; set; } = string.Empty;

        /// <summary>Enlace a Google Maps (o similar) para el botón "Cómo llegar".</summary>
        public string EnlaceMapa { get; set; } = string.Empty;

        /// <summary>Se activa cuando la clínica tenga testimonios reales que publicar.</summary>
        public bool MostrarOpiniones { get; set; }

        /// <summary>
        /// Enlace que abre un chat de WhatsApp con la clínica y el mensaje ya escrito.
        /// Ignora espacios, signos y cualquier carácter que no sea un dígito del número.
        /// </summary>
        public string EnlaceWhatsApp(string mensaje)
        {
            string digitos = new(WhatsApp.Where(char.IsAsciiDigit).ToArray());
            return $"https://wa.me/{digitos}?text={Uri.EscapeDataString(mensaje)}";
        }
    }
}
