using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe représentant un abonnement à une revue.
    /// </summary>
    public class Abonnement
    {
        /// <summary>
        /// Identifiant de l'abonnement (peut être null si l'abonnement n'est pas encore enregistré en base de données).
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identifiant de la revue associée à l'abonnement.
        /// </summary>
        public string IdRevue { get; }

        /// <summary>
        /// Date de commande de l'abonnement.
        /// </summary>
        public DateTime DateCommande { get; }

        /// <summary>
        /// Montant de l'abonnement.
        /// </summary>
        public double Montant { get; }

        /// <summary>
        /// Date de fin de l'abonnement (peut être null si l'abonnement n'a pas de date de fin définie).
        /// </summary>
        public DateTime? DateFinAbonnement { get; set; }

        /// <summary>
        /// Constructeur de la classe Abonnement.
        /// </summary>
        /// <param name="idCommande">Identifiant de l'abonnement (peut être null).</param>
        /// <param name="dateCommande">Date de commande de l'abonnement.</param>
        /// <param name="montant">Montant de l'abonnement.</param>
        /// <param name="dateFinAbonnement">Date de fin de l'abonnement (peut être null).</param>
        /// <param name="idRevue">Identifiant de la revue associée à l'abonnement.</param>
        public Abonnement(int? idCommande, DateTime dateCommande, double montant, DateTime? dateFinAbonnement, string idRevue)
        {
            this.Id = idCommande;
            this.IdRevue = idRevue;
            this.DateCommande = dateCommande;
            this.Montant = montant;
            this.DateFinAbonnement = dateFinAbonnement;
        }

        /// <summary>
        /// Vérifie si une date de parution est comprise dans la période de l'abonnement.
        /// </summary>
        /// <param name="dateCommande">Date de commande de l'abonnement.</param>
        /// <param name="dateFinAbonnement">Date de fin de l'abonnement.</param>
        /// <param name="dateParution">Date de parution à vérifier.</param>
        /// <returns>True si la date de parution est comprise dans l'abonnement, sinon False.</returns>
        public static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }
    }
}