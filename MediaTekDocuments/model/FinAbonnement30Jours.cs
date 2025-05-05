using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un abonnement qui se termine dans les 30 jours.
    /// </summary>
    public class FinAbonnement30Jours
    {
        /// <summary>
        /// Date de fin de l'abonnement.
        /// </summary>
        public DateTime DateFinAbonnement { get; }

        /// <summary>
        /// Titre de la revue associée à l'abonnement.
        /// </summary>
        public string Titre { get; }

        /// <summary>
        /// Constructeur de la classe FinAbonnement30Jours.
        /// </summary>
        /// <param name="titre">Titre de la revue associée à l'abonnement.</param>
        /// <param name="dateFinAbonnement">Date de fin de l'abonnement sous forme de chaîne de caractères.</param>
        public FinAbonnement30Jours(string titre, string dateFinAbonnement)
        {
            this.Titre = titre;
            this.DateFinAbonnement = DateTime.Parse(dateFinAbonnement, CultureInfo.InvariantCulture);
        }
    }
}