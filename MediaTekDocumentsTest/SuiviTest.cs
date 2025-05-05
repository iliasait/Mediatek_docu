using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class SuiviTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs
        [TestMethod]
        public void Suivi_Constructeur_AssigneValeursCorrectement()
        {
            Suivi suivi = new Suivi(1, "En cours");
            Assert.AreEqual(1, suivi.Id);
            Assert.AreEqual("En cours", suivi.Etat);
        }

        // Vérifie que la méthode ToString() retourne bien l'état
        [TestMethod]
        public void Suivi_ToString_RetourneEtat()
        {
            Suivi suivi = new Suivi(2, "Terminé");
            Assert.AreEqual("Terminé", suivi.ToString());
        }
    }
}
