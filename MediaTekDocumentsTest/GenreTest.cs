using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class GenreTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs pour Genre
        [TestMethod]
        public void Genre_Constructeur_AssigneValeursCorrectement()
        {
            Genre genreCible = new Genre("G001", "Fantasy");
            Assert.AreEqual("G001", genreCible.Id);
            Assert.AreEqual("Fantasy", genreCible.Libelle);
        }
    }
}
