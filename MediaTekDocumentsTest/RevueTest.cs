using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class RevueTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs
        [TestMethod]
        public void Revue_Constructeur_AssigneValeursCorrectement()
        {
            Revue revue = new Revue(
                "R001", "Science & Vie", "image.jpg", "G001", "Science",
                "P001", "Adulte", "R001", "Bibliothèque Centrale",
                "Mensuelle", 15
            );

            Assert.AreEqual("R001", revue.Id);
            Assert.AreEqual("Science & Vie", revue.Titre);
            Assert.AreEqual("image.jpg", revue.Image);
            Assert.AreEqual("G001", revue.IdGenre);
            Assert.AreEqual("Science", revue.Genre);
            Assert.AreEqual("P001", revue.IdPublic);
            Assert.AreEqual("Adulte", revue.Public);
            Assert.AreEqual("R001", revue.IdRayon);
            Assert.AreEqual("Bibliothèque Centrale", revue.Rayon);

            Assert.AreEqual("Mensuelle", revue.Periodicite);
            Assert.AreEqual(15, revue.DelaiMiseADispo);
        }
    }
}
