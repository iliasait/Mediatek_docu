using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe représentant une commande de document (livre ou DVD).
    /// Hérite de la classe <see cref="Commande"/>.
    /// </summary>
    public class CommandeDocument : Commande
    {
        /// <summary>
        /// Nombre d'exemplaires commandés.
        /// </summary>
        public int NbExemplaire { get; set; }

        /// <summary>
        /// Identifiant du livre ou du DVD associé à la commande.
        /// </summary>
        public string IdLivreDvd { get; set; }

        /// <summary>
        /// Identifiant du suivi de la commande.
        /// </summary>
        public int IdSuivi { get; set; }

        /// <summary>
        /// État actuel de la commande (exemple : "En cours", "Livrée", etc.).
        /// </summary>
        public string Etat { get; set; }

        /// <summary>
        /// Constructeur de la classe CommandeDocument.
        /// </summary>
        /// <param name="id">Identifiant de la commande (peut être null si la commande n'est pas encore enregistrée en base de données).</param>
        /// <param name="dateCommande">Date de la commande.</param>
        /// <param name="montant">Montant total de la commande.</param>
        /// <param name="nbExemplaire">Nombre d'exemplaires commandés.</param>
        /// <param name="idLivreDvd">Identifiant du livre ou du DVD associé à la commande.</param>
        /// <param name="idSuivi">Identifiant du suivi de la commande.</param>
        /// <param name="etat">État actuel de la commande.</param>
        public CommandeDocument(int? id, DateTime dateCommande, double montant, int nbExemplaire,
            string idLivreDvd, int idSuivi, string etat)
            : base(id, dateCommande, montant)
        {
            this.NbExemplaire = nbExemplaire;
            this.IdLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
            this.Etat = etat;
        }
    }
}