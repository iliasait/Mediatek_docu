using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MediaTekDocuments.manager
{
    /// <summary>
    /// Classe indépendante d'accès à une api rest avec éventuellement une "basic authorization"
    /// </summary>
    class ApiRest
    {
        /// <summary>
        /// unique instance de la classe
        /// </summary>
        private static ApiRest instance = null;
        /// <summary>
        /// Objet de connexion à l'api
        /// </summary>
        private readonly HttpClient httpClient;
        /// <summary>
        /// Canal http pour l'envoi du message et la récupération de la réponse
        /// </summary>
        private HttpResponseMessage httpResponse;

        /// <summary>
        /// Constructeur privé pour préparer la connexion (éventuellement sécurisée)
        /// </summary>
        /// <param name="uriApi">adresse de l'api</param>
        /// <param name="username">chaîne d'authentification</param>
        /// <param name="password">chaîne d'authentification</param>

        private ApiRest(String uriApi, String username, String password)
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(uriApi) };

            // Ajout des en-têtes X-Auth-User et X-Auth-Pass pour l'authentification
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                httpClient.DefaultRequestHeaders.Add("X-Auth-User", username);
                httpClient.DefaultRequestHeaders.Add("X-Auth-Pass", password);
            }
        }


        /// <summary>
        /// Crée une instance unique de la classe
        /// </summary>
        /// <param name="uriApi">adresse de l'api</param>
        /// <param name="username">chaîne d'authentificatio (login)</param>
        /// <param name="password">chaîne d'authentificatio (pwd)</param>
        /// <returns></returns>
        public static ApiRest GetInstance(String uriApi, String username, String password)
        {
            if (instance == null)
            {
                instance = new ApiRest(uriApi, username, password);
            }
            return instance;
        }


        /// <summary>
        /// Envoi une demande à l'API et récupère la réponse
        /// </summary>
        /// <param name="methode">verbe http (GET, POST, PUT, DELETE)</param>
        /// <param name="message">message à envoyer dans l'URL</param>
        /// <param name="parametres">contenu de variables à mettre dans body</param>
        /// <returns>liste d'objets (select) ou liste vide (ok) ou null si erreur</returns>
        public JObject RecupDistant(string methode, string message, String parametres)
        {
            // transformation des paramètres pour les mettre dans le body
            StringContent content = null;
            if(!(parametres is null))
            {
                content = new StringContent(parametres, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            }
            // envoi du message et attente de la réponse
            switch (methode)
            {
                case "GET":
                    httpResponse = httpClient.GetAsync(message).Result;
                    break;
                case "POST":
                    httpResponse = httpClient.PostAsync(message, content).Result;
                    break;
                case "PUT":
                    httpResponse = httpClient.PutAsync(message, content).Result;
                    break;
                case "DELETE":
                    httpResponse = httpClient.DeleteAsync(message).Result;
                    break;
                // methode incorrecte
                default:
                    return new JObject();
            }
            // récupération de l'information retournée par l'api
            return httpResponse.Content.ReadAsAsync<JObject>().Result;
        }

        /// <summary>
        /// Envoi une demande à l'API et récupère la réponse
        /// </summary>
        /// <param name="methode">verbe http (GET, POST, PUT, DELETE)</param>
        /// <param name="message">message à envoyer dans l'URL</param>
        /// <returns>liste d'objets (select) ou liste vide (ok) ou null si erreur</returns>
        public JObject RecupDistant(string methode, string message)
        {
            // envoi du message et attente de la réponse
            switch (methode)
            {
                case "GET":
                    httpResponse = httpClient.GetAsync(message).Result;
                    break;
                case "POST":
                    httpResponse = httpClient.PostAsync(message, null).Result;
                    break;
                case "PUT":
                    httpResponse = httpClient.PutAsync(message, null).Result;
                    break;
                case "DELETE":
                    httpResponse = httpClient.DeleteAsync(message).Result;
                    break;
                // methode incorrecte
                default:
                    return new JObject();
            }
            // récupération de l'information retournée par l'api
            return httpResponse.Content.ReadAsAsync<JObject>().Result;
        }
    }
}
