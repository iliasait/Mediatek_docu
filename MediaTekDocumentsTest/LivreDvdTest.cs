using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class LivreDvdTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs
        [TestMethod]
        public void LivreDvd_Constructeur_AssigneValeursCorrectement()
        {
            LivreDvd livreDvdCible = new LivreDvdConcret("LD001", "Harry Potter", "hp.jpg", "G001", "Fantasy", "P001", "Adulte", "R001", "Fiction");

            Assert.AreEqual("LD001", livreDvdCible.Id);
            Assert.AreEqual("Harry Potter", livreDvdCible.Titre);
            Assert.AreEqual("hp.jpg", livreDvdCible.Image);
            Assert.AreEqual("G001", livreDvdCible.IdGenre);
            Assert.AreEqual("Fantasy", livreDvdCible.Genre);
            Assert.AreEqual("P001", livreDvdCible.IdPublic);
            Assert.AreEqual("Adulte", livreDvdCible.Public);
            Assert.AreEqual("R001", livreDvdCible.IdRayon);
            Assert.AreEqual("Fiction", livreDvdCible.Rayon);
        }

        // Test de la classe pour instancier LivreDvd car LivreDvd est abstraite
        private class LivreDvdConcret : LivreDvd
        {
            public LivreDvdConcret(string id, string titre, string image, string idGenre, string genre,
                string idPublic, string lePublic, string idRayon, string rayon)
                : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
            {
            }
        }
    }
}
