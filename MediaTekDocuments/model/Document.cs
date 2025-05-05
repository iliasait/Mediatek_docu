namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les informations communes à tous les documents : Livre, Revue, DVD).
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Identifiant unique du document.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Titre du document.
        /// </summary>
        public string Titre { get; }

        /// <summary>
        /// Chemin ou URL de l'image associée au document.
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// Identifiant du genre associé au document.
        /// </summary>
        public string IdGenre { get; }

        /// <summary>
        /// Libellé du genre associé au document.
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// Identifiant du public cible associé au document.
        /// </summary>
        public string IdPublic { get; }

        /// <summary>
        /// Libellé du public cible associé au document.
        /// </summary>
        public string Public { get; }

        /// <summary>
        /// Identifiant du rayon associé au document.
        /// </summary>
        public string IdRayon { get; }

        /// <summary>
        /// Libellé du rayon associé au document.
        /// </summary>
        public string Rayon { get; }

        /// <summary>
        /// Constructeur de la classe Document.
        /// </summary>
        /// <param name="id">Identifiant unique du document.</param>
        /// <param name="titre">Titre du document.</param>
        /// <param name="image">Chemin ou URL de l'image associée au document.</param>
        /// <param name="idGenre">Identifiant du genre associé au document.</param>
        /// <param name="genre">Libellé du genre associé au document.</param>
        /// <param name="idPublic">Identifiant du public cible associé au document.</param>
        /// <param name="lePublic">Libellé du public cible associé au document.</param>
        /// <param name="idRayon">Identifiant du rayon associé au document.</param>
        /// <param name="rayon">Libellé du rayon associé au document.</param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}