namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un état de suivi (exemple : "En cours", "Livré", etc.).
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Identifiant unique du suivi.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// État du suivi (exemple : "En cours", "Livré", etc.).
        /// </summary>
        public string Etat { get; }

        /// <summary>
        /// Constructeur de la classe Suivi.
        /// </summary>
        /// <param name="id">Identifiant unique du suivi.</param>
        /// <param name="etat">État du suivi.</param>
        public Suivi(int id, string etat)
        {
            this.Id = id;
            this.Etat = etat;
        }

        /// <summary>
        /// Récupère le libellé de l'état pour l'affichage.
        /// </summary>
        /// <returns>Le libellé de l'état.</returns>
        public override string ToString()
        {
            return this.Etat;
        }
    }
}