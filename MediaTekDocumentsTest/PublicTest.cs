using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class PublicTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs
        [TestMethod]
        public void Public_Constructeur_AssigneValeursCorrectement()
        {
            Public publicCible = new Public("P001", "Adulte");
            Assert.AreEqual("P001", publicCible.Id);
            Assert.AreEqual("Adulte", publicCible.Libelle);
        }

        // Vérifie la modification des propriétés après l'initialisation
        [TestMethod]
        public void Public_Modification_Proprietes()
        {
            Public publicCible = new Public("P002", "Enfant");
            publicCible.Libelle = "Jeunesse";
            Assert.AreEqual("Jeunesse", publicCible.Libelle);
        }
    }
}
