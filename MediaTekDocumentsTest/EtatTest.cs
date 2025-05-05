using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class EtatTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs à chaque propriété
        [TestMethod]
        public void Etat_Constructeur_AssigneValeursCorrectement()
        {
            string id = "E001";
            string libelle = "Bon état";
            Etat etat = new Etat(id, libelle);
            Assert.AreEqual(id, etat.Id);
            Assert.AreEqual(libelle, etat.Libelle);
        }

        // Vérifie que la propriété Id est correctement assignée
        [TestMethod]
        public void Etat_Id_AssigneCorrectement()
        {
            string id = "E002";
            Etat etat = new Etat(id, "Mauvais état");
            Assert.AreEqual(id, etat.Id);
        }

        // Vérifie que la propriété Libelle est correctement assignée
        [TestMethod]
        public void Etat_Libelle_AssigneCorrectement()
        {
            string libelle = "Neuf";
            Etat etat = new Etat("E003", libelle);
            Assert.AreEqual(libelle, etat.Libelle);
        }
    }
}
