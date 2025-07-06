using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace E.m.a.r.t.Helpers
{
    /// <summary>
    /// Métodos de extensão para facilitar o armazenamento e recuperação de objetos JSON na sessão.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Serializa um objeto para JSON e guarda na sessão com a chave especificada.
        /// </summary>
        /// <param name="session">Sessão HTTP onde será guardado o objeto.</param>
        /// <param name="key">Chave para identificar o objeto na sessão.</param>
        /// <param name="value">Objeto a ser serializado e guardado.</param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Recupera e desserializa um objeto JSON da sessão usando a chave especificada.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a desserializar.</typeparam>
        /// <param name="session">Sessão HTTP onde o objeto está guardado.</param>
        /// <param name="key">Chave que identifica o objeto na sessão.</param>
        /// <returns>Objeto desserializado do tipo especificado, ou valor padrão se não existir.</returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
