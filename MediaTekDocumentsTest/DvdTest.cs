using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class DvdTest
    {
        // Teste le constructeur et vérifie que les propriétés sont bien assignées
        [TestMethod]
        public void Dvd_Constructeur_AssigneValeursCorrectement()
        {
            string id = "D001";
            string titre = "Le Film";
            string image = "image.jpg";
            int duree = 120;
            string realisateur = "Réalisateur";
            string synopsis = "Synopsis du film";
            string idGenre = "G001";
            string genre = "Action";
            string idPublic = "P001";
            string lePublic = "Tout Public";
            string idRayon = "R001";
            string rayon = "Films";

            Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis,
                              idGenre, genre, idPublic, lePublic, idRayon, rayon);

            Assert.AreEqual(id, dvd.Id);
            Assert.AreEqual(titre, dvd.Titre);
            Assert.AreEqual(image, dvd.Image);
            Assert.AreEqual(duree, dvd.Duree);
            Assert.AreEqual(realisateur, dvd.Realisateur);
            Assert.AreEqual(synopsis, dvd.Synopsis);
            Assert.AreEqual(idGenre, dvd.IdGenre);
            Assert.AreEqual(genre, dvd.Genre);
            Assert.AreEqual(idPublic, dvd.IdPublic);
            Assert.AreEqual(lePublic, dvd.Public);
            Assert.AreEqual(idRayon, dvd.IdRayon);
            Assert.AreEqual(rayon, dvd.Rayon);
        }

        // Teste la propriété Duree
        [TestMethod]
        public void Dvd_Duree_AssigneCorrectement()
        {
            int duree = 150;
            Dvd dvd = new Dvd("D002", "Film 2", "image2.jpg", duree, "Réalisateur 2", "Synopsis 2",
                              "G002", "Drame", "P002", "Adulte", "R002", "Drame");
            Assert.AreEqual(duree, dvd.Duree);
        }

        // Teste la propriété Realisateur
        [TestMethod]
        public void Dvd_Realisateur_AssigneCorrectement()
        {
            string realisateur = "Réalisateur X";
            Dvd dvd = new Dvd("D003", "Film 3", "image3.jpg", 130, realisateur, "Synopsis 3",
                              "G003", "Science-fiction", "P003", "Tous publics", "R003", "Science-fiction");
            Assert.AreEqual(realisateur, dvd.Realisateur);
        }

        // Teste la propriété Synopsis
        [TestMethod]
        public void Dvd_Synopsis_AssigneCorrectement()
        {
            string synopsis = "Synopsis détaillé";
            Dvd dvd = new Dvd("D004", "Film 4", "image4.jpg", 140, "Réalisateur Y", synopsis,
                              "G004", "Comédie", "P004", "Enfants", "R004", "Comédie");
            Assert.AreEqual(synopsis, dvd.Synopsis);
        }
    }
}
