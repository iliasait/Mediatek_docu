using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using Serilog;
namespace MediaTekDocuments.dal
{

    /// <summary>
    /// Espace de noms contenant les classes d'accès aux données pour l'application MediaTekDocuments.
    /// </summary>
    internal class NamespaceDoc
    {
    }
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi =  ConfigurationManager.AppSettings["ApiUri"];
        /// <summary>
        /// constante pour la chaîne 'champs='
        /// </summary>
        private const string ChampsQueryParam = "champs=";

        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour SELECT
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour INSERT
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour UPDATE
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour DELETE
        private const string DELETE = "PUT";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            try
            {
                string username = ConfigurationManager.AppSettings["ApiUsername"];
                string password = ConfigurationManager.AppSettings["ApiPassword"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(uriApi))
                {
                    throw new ConfigurationErrorsException("Le mot de passe de l'API est manquant dans le fichier de configuration.");
                }

                api = ApiRest.GetInstance(uriApi, username, password);


            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Constructeur static de la classe Access qui initialise les logs une seule fois
        /// </summary>
        ///
        static Access()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs/errorlog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.EventLog("MediaTekDocuments", manageEventSource: true, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal)
                .CreateLogger();

            Log.Information("Initialisation de Serilog terminée.");
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {

            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }
        /// <summary>
        /// Retourne tous états de la table suivi
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> allsuivi = TraitementRecup<Suivi>(GET, "allsuivi", null);
            return allsuivi;
        }
        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne tous les etats à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            IEnumerable<Etat> lesEtats = TraitementRecup<Etat>(GET, "etat", null);
            return new List<Etat>(lesEtats);
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", ChampsQueryParam + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message, String parametres)
        {
            List<T> liste = new List<T>();
            try
            {
                // Remplacer l'interpolation de chaîne par des paramètres
                Log.Information("PREPARATION API : Méthode={Methode}, Message={Message}, Paramètres={Parametres}", methode, message, parametres);
                JObject retour = api.RecupDistant(methode, message, parametres);

                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        Log.Information("RETOUR API : {ResultString}", resultString);
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }

                }
                else
                {
                    Log.Error("Code erreur : {Code} Message : {Message}", code, (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Log.Error("Erreur lors de l'accès à l'API : {ErrorMessage}", e.Message);
                Environment.Exit(0);
            }
            return liste;
        }


        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        /// <summary>
        /// Retourne les commandes d'un livre
        /// </summary>
        /// <param name="idLivre"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesLivres(string idLivre)
        {
            Log.Information("Tentative de récupération des commandes pour le livre avec l'ID : {IdLivre}", idLivre);

            String jsonIdDocument = convertToJson("idLivreDvd", idLivre);
            List<CommandeDocument> lesCommandesLivres = TraitementRecup<CommandeDocument>(GET, "infocommandedocument/" + jsonIdDocument, null);

            if (lesCommandesLivres.Count > 0)
            {
                Log.Information("Nombre de commandes récupérées pour le livre ID {IdLivre}: {CommandeCount}", idLivre, lesCommandesLivres.Count);
            }
            else
            {
                Log.Warning("Aucune commande trouvée pour le livre avec l'ID {IdLivre}", idLivre);
            }

            return lesCommandesLivres;
        }

        /// <summary>
        /// Créer une commande de Livre / Dvd 
        /// </summary>
        /// <param name="insertCommande"></param>
        /// <returns></returns>
        public bool CreerCommandeDocument(CommandeDocument insertCommande)
        {
            string jsonDetailCommande = JsonConvert.SerializeObject(insertCommande, new CustomDateTimeConverter());
            Console.WriteLine(jsonDetailCommande);

            Log.Information("Tentative de création de la commande document.");

            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandeDocAjout", ChampsQueryParam + jsonDetailCommande);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Erreur lors de la création de la commande document : {Message} ", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Modifie une commande de Livre / Dvd 
        /// </summary>
        /// <param name="updateCommande"></param>
        /// <returns></returns>
        public bool ModifierCommandeDocument(CommandeDocument updateCommande)
        {
            string jsonDetailCommande = JsonConvert.SerializeObject(updateCommande, new CustomDateTimeConverter());
            Console.WriteLine(uriApi + "commandeDocModifier?champs=" + jsonDetailCommande);

            Log.Information("Tentative de modification de la commande document.");

            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, "commandeDocModifier", ChampsQueryParam + jsonDetailCommande);
                return true;

            }
            catch (Exception ex)
            {
                Log.Error("Erreur lors de la modification de la commande document : {Message} ", ex.Message);
                return false;

            }
        }

        /// <summary>
        /// Supprime une commande de Livre / Dvd 
        /// </summary>
        /// <param name="deleteCommande"></param>
        /// <returns></returns>
        public bool SupprimerCommandeDocument(CommandeDocument deleteCommande)
        {
            string jsonDetailCommande = JsonConvert.SerializeObject(deleteCommande, new CustomDateTimeConverter());
            Console.WriteLine(uriApi + "commandeDocSupprimer?champs=" + jsonDetailCommande);

            Log.Information("Tentative de suppression de la commande document.");

            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(DELETE, "commandeDocSupprimer", ChampsQueryParam + jsonDetailCommande);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Erreur lors de la suppression de la commande document : {Message} ", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Récupère la liste de commande d'abonnements présents dans la BDD 
        /// </summary>
        /// <param name="idRevue"></param>
        /// <returns></returns>
        public List<Abonnement> GetCommandesAbonnement(string idRevue)
        {
            Log.Information("Tentative de récupération des commandes pour l'abonnement avec l'ID revue : {IdRevue} ", idRevue);

            String jsonIdDocument = convertToJson("idRevue", idRevue);
            List<Abonnement> lesCommandesAbonnement = TraitementRecup<Abonnement>(GET, "commandeabonnementinfo/" + jsonIdDocument, null);

            if (lesCommandesAbonnement != null && lesCommandesAbonnement.Count > 0)
            {
                Log.Information("Commandes d'abonnement récupérées pour l'ID revue : {IdRevue} ", idRevue);
            }
            else
            {
                Log.Warning("Aucune commande trouvée pour l'abonnement avec l'ID revue : {IdRevue} ", idRevue);
            }

            return lesCommandesAbonnement;
        }
        /// <summary>
        /// Créer une commande d'abonnement (revues)
        /// </summary>
        /// <param name="insertAbonnementCommande"></param>
        /// <returns></returns>
        public bool CreerCommandeAbonnement(Abonnement insertAbonnementCommande)
        {
            string jsonDetailCommande = JsonConvert.SerializeObject(insertAbonnementCommande, new CustomDateTimeConverter());
            Console.WriteLine(jsonDetailCommande);

            Log.Information("Tentative de création de la commande d'abonnement.");

            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "commandeAbonnementAjout", ChampsQueryParam + jsonDetailCommande);
                // Si l'exécution atteint cette ligne sans exceptions, la commande a été créée avec succès
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Erreur lors de la création de la commande d'abonnement : {Message}", ex.Message);
                return false; // Retourner false si une exception se produit
            }
        }
        /// <summary>
        /// Supprime une commande d'abonnement (revues)
        /// </summary>
        /// <param name="deleteAbonnementCommande"></param>
        /// <returns></returns>
        public bool SupprimerCommandeAbonnement(Abonnement deleteAbonnementCommande)
        {
            string jsonDetailCommande = JsonConvert.SerializeObject(deleteAbonnementCommande, new CustomDateTimeConverter());
            Console.WriteLine(uriApi + "commandeAbonnementSupprimer?champs=" + jsonDetailCommande);

            Log.Information("Tentative de suppression de la commande d'abonnement.");

            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(DELETE, "commandeAbonnementSupprimer", ChampsQueryParam + jsonDetailCommande);
                return true;

            }
            catch (Exception ex)
            {
                Log.Error("Erreur lors de la suppression de la commande d'abonnement : {Message}", ex.Message);
                return false;

            }
        }
        /// <summary>
        /// Modifie une commande d'abonnement (revues)
        /// </summary>
        /// <param name="updateCommande"></param>
        /// <returns></returns>
        public bool ModifierCommandeAbonnement(Abonnement updateCommande)
        {
            string jsonDetailCommande = JsonConvert.SerializeObject(updateCommande, new CustomDateTimeConverter());
            Console.WriteLine(uriApi + "commandeAbonnementModifier?champs=" + jsonDetailCommande);

            Log.Information("Tentative de modification de la commande d'abonnement.");

            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(PUT, "commandeAbonnementModifier", ChampsQueryParam + jsonDetailCommande);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Erreur lors de la modification de la commande d'abonnement : {Message}", ex.Message);
                return false;

            }
        }
        /// <summary>
        /// Récupère la liste des abonnements dont la fin arrive bientôt 
        /// </summary>
        /// <param name="idRevue"></param>
        /// <returns></returns>
        public List<FinAbonnement30Jours> GetListeFinAbonnement(string idAbonnement)
        {
            String jsonIdDocument = convertToJson("idAbo", idAbonnement);

            Log.Information("Tentative de récupération de la liste des fins d'abonnement pour l'ID d'abonnement : {IdAbonnement}", idAbonnement);

            List<FinAbonnement30Jours> laListeFinAbonnement = TraitementRecup<FinAbonnement30Jours>(GET, "infolistefinabonnement/" + jsonIdDocument, null);

            if (laListeFinAbonnement != null && laListeFinAbonnement.Count > 0)
            {
                Log.Information("Liste des fins d'abonnement récupérée avec succès. Nombre d'éléments : {NombreElements}", laListeFinAbonnement.Count);
            }
            else
            {
                Log.Warning("Aucune fin d'abonnement trouvée pour l'ID d'abonnement : {IdAbonnement}", idAbonnement);
            }

            return laListeFinAbonnement;
        }
        /// <summary>
        /// Récupère la liste des utilisateurs dans la base de données
        /// </summary>
        /// <param name="idRevue"></param>
        /// <returns></returns>
        public List<Utilisateur> GetUserInfo(Utilisateur utilisateur)
        {
            string jsonUtilisateur = JsonConvert.SerializeObject(utilisateur);

            Log.Information("Tentative de récupération des informations pour l'utilisateur : {Utilisateur}", jsonUtilisateur);

            List<Utilisateur> liste = TraitementRecup<Utilisateur>(GET, "infoUser/" + jsonUtilisateur, null);

            if (liste != null && liste.Count > 0)
            {
                Log.Information("Informations utilisateur récupérées avec succès. Nombre d'éléments : {NombreElements}", liste.Count);
            }
            else
            {
                Log.Warning("Aucune information utilisateur trouvée pour : {Utilisateur}", jsonUtilisateur);
            }

            return liste;
        }

    }
}
