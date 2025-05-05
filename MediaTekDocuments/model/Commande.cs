using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe représentant une commande.
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Identifiant de la commande (peut être null si la commande n'est pas encore enregistrée en base de données).
        /// </summary>
        public int? Id { get; set; } // ID nullable

        /// <summary>
        /// Date de la commande.
        /// </summary>
        public DateTime DateCommande { get; set; }

        /// <summary>
        /// Montant total de la commande.
        /// </summary>
        public double Montant { get; set; }

        /// <summary>
        /// Constructeur de la classe Commande.
        /// </summary>
        /// <param name="id">Identifiant de la commande (peut être null).</param>
        /// <param name="dateCommande">Date de la commande.</param>
        /// <param name="montant">Montant total de la commande.</param>
        public Commande(int? id, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}