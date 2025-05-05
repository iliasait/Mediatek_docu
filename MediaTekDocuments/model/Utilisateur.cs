using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un utilisateur du système.
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de l'utilisateur.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur.
        /// </summary>
        public string MotDePasse { get; set; }

        /// <summary>
        /// Identifiant du service auquel l'utilisateur est associé.
        /// </summary>
        public int IdService { get; set; }

        /// <summary>
        /// Constructeur de la classe Utilisateur.
        /// </summary>
        /// <param name="idUser">Identifiant unique de l'utilisateur.</param>
        /// <param name="nom">Nom de l'utilisateur.</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur.</param>
        /// <param name="idService">Identifiant du service auquel l'utilisateur est associé.</param>
        public Utilisateur(int idUser, string nom, string motDePasse, int idService)
        {
            this.Id = idUser;
            this.Nom = nom;
            this.MotDePasse = motDePasse;
            this.IdService = idService;
        }
    }
}