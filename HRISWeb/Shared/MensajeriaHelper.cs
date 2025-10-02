using System;
using System.Web.UI;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Define los tipos de mensaje.
    /// </summary>
    public enum TipoMensaje : byte
    {
        /// <summary>
        /// Define un mensaje de tipo Informativo.
        /// </summary>
        Informacion = 1,

        /// <summary>
        /// Define un mensaje de tipo Advertencia.
        /// </summary>
        Advertencia = 2,

        /// <summary>
        /// Define un mensaje de tipo Validación.
        /// </summary>
        Validacion = 3,

        /// <summary>
        /// Define un mensaje de tipo Error.
        /// </summary>
        Error = 4
    }

    /// <summary>
    /// Clase utilitaria para el despliegue de mansajería.
    /// </summary>
    public class MensajeriaHelper
    {
        /// <summary>
        /// Muestra un mensaje emergente del tipo indicado..
        /// </summary>
        /// <param name="control">El control en el cual se registra el bloque de script.</param>
        /// <param name="tipo">El tipo de mensaje.</param>
        /// <param name="mensaje">El mensaje.</param>
        public static void MostrarMensaje(Control control, TipoMensaje tipo, string mensaje)
        {
            MostrarMensaje(control, tipo, mensaje, string.Empty);
        }

        /// <summary>
        /// Muestra un mensaje emergente del tipo indicado, con la posibilidad de ejecutar un callback Javascript al cerrar el mensaje.
        /// </summary>
        /// <param name="control">El control en el cual se registra el bloque de script.</param>
        /// <param name="tipo">El tipo de mensaje.</param>
        /// <param name="mensaje">El mensaje.</param>
        /// <param name="callbackJS">El callback de Javascript o nulo si no se desea.</param>
        public static void MostrarMensaje(Control control, TipoMensaje tipo, string mensaje, string callbackJS)
        {
            // Nota técnica: cuando la invocación de la mensajería viene desde el server side (es decir, por este método), se detectó
            // que hay un pequeño issue donde al presionar TAB con la pantalla de mensaje abierta se puede alcanzar controles que se
            // encuentran atrás. Por eso se 1) Muestra el backdrop como primer paso y 2) después de una pequeña espera de 100ms se
            // muestra la mensajería, esto para dar un tiempo de que todos los controles estén en estado consistente.
            ScriptManager.RegisterStartupScript(control, control.GetType(), 
                string.Format("Mensajeria-{0}", new Guid().ToString()), 
                string.Format("$(document).ready(function(){{ DesplegarBackdrop(); setTimeout(function() {{ MostrarMensaje({0}, '{1}', {2}); }}, 200); }});", Convert.ToByte(tipo), mensaje, string.IsNullOrEmpty(callbackJS) ? "null" : callbackJS), 
                true);
        }

        /// <summary>
        /// Muestra un mensaje de confirmación, con la posibilidad de ejecutar un callback Javascript al cerrar el mensaje.
        /// </summary>
        /// <param name="control">El control en el cual se registra el bloque de script.</param>
        /// <param name="mensajeConfirmacion">El mensaje de confirmación para mostrar al usuario.</param>
        /// <param name="mensajeAfirmacion">El mensaje de afirmación.</param>
        /// <param name="accionConfirmacion">La acción de confirmación.</param>
        /// <param name="mensajeNegacion">El mensaje de negación.</param>
        /// <param name="accionNegacion">La acción de negación.</param>
        public static void MostrarMensajeConfirmacion(Control control, string mensajeConfirmacion, string mensajeAfirmacion, string accionConfirmacion, string mensajeNegacion, string accionNegacion)
        {
            // Nota técnica: cuando la invocación de la mensajería viene desde el server side (es decir, por este método), se detectó
            // que hay un pequeño issue donde al presionar TAB con la pantalla de mensaje abierta se puede alcanzar controles que se
            // encuentran atrás. Por eso se 1) Muestra el backdrop como primer paso y 2) después de una pequeña espera de 100ms se
            // muestra la mensajería, esto para dar un tiempo de que todos los controles estén en estado consistente.
            ScriptManager.RegisterStartupScript(
                control, control.GetType(), string.Format("Mensajeria-{0}", new Guid().ToString()), 
                string.Format("$(document).ready(function(){{ DesplegarBackdrop(); setTimeout(function() {{ MostrarConfirmacion('{0}', '{1}', {2}, '{3}', {4}); }}, 100); }});", mensajeConfirmacion, mensajeAfirmacion, string.IsNullOrEmpty(accionConfirmacion) ? "null" : accionConfirmacion, mensajeNegacion, string.IsNullOrEmpty(accionNegacion) ? "null" : accionNegacion), 
                true);
        }
    }
}