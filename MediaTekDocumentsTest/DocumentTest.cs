using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class DocumentTest
    {
        // Teste le constructeur et vérifie que toutes les propriétés sont bien assignées
        [TestMethod]
        public void Document_Constructeur_AssigneValeursCorrectement()
        {
            string id = "D001";
            string titre = "Le Document";
            string image = "document.jpg";
            string idGenre = "G001";
            string genre = "Action";
            string idPublic = "P001";
            string lePublic = "Tous publics";
            string idRayon = "R001";
            string rayon = "Films";

            Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            Assert.AreEqual(id, document.Id);
            Assert.AreEqual(titre, document.Titre);
            Assert.AreEqual(image, document.Image);
            Assert.AreEqual(idGenre, document.IdGenre);
            Assert.AreEqual(genre, document.Genre);
            Assert.AreEqual(idPublic, document.IdPublic);
            Assert.AreEqual(lePublic, document.Public);
            Assert.AreEqual(idRayon, document.IdRayon);
            Assert.AreEqual(rayon, document.Rayon);
        }

        // Teste la propriété Id
        [TestMethod]
        public void Document_Id_AssigneCorrectement()
        {
            string id = "D002";
            Document document = new Document(id, "Document 2", "image2.jpg", "G002", "Drame", "P002", "Adultes", "R002", "Drame");
            Assert.AreEqual(id, document.Id);
        }

        // Teste la propriété Titre
        [TestMethod]
        public void Document_Titre_AssigneCorrectement()
        {
            string titre = "Document 3";
            Document document = new Document("D003", titre, "image3.jpg", "G003", "Science-fiction", "P003", "Tous publics", "R003", "Science-fiction");
            Assert.AreEqual(titre, document.Titre);
        }

        // Teste la propriété Genre
        [TestMethod]
        public void Document_Genre_AssigneCorrectement()
        {
            string genre = "Aventure";
            Document document = new Document("D004", "Document 4", "image4.jpg", "G004", genre, "P004", "Jeunes", "R004", "Aventure");
            Assert.AreEqual(genre, document.Genre);
        }

        // Teste la propriété Rayon
        [TestMethod]
        public void Document_Rayon_AssigneCorrectement()
        {
            string rayon = "Histoire";
            Document document = new Document("D005", "Document 5", "image5.jpg", "G005", "Biographie", "P005", "Adultes", "R005", rayon);
            Assert.AreEqual(rayon, document.Rayon);
        }
    }
}
