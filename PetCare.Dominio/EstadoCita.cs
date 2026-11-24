namespace PetCare
{
    /// <summary>
    /// Estados por los que pasa una cita de consulta.
    /// </summary>
    public enum EstadoCita
    {
        /// <summary>La cita está agendada y todavía no se ha atendido.</summary>
        Programada,
        /// <summary>La consulta ya se hizo.</summary>
        Atendida,
        /// <summary>La cita se canceló y no se puede facturar.</summary>
        Cancelada
    }
}
