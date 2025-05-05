using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CategorieTest
    {
        // Teste le constructeur et vérifie que toutes les propriétés sont bien assignées
        [TestMethod]
        public void Categorie_Constructeur_AssigneValeursCorrectement()
        {
            string id = "1";
            string libelle = "Public";

            Categorie categorie = new Categorie(id, libelle);

            Assert.AreEqual(id, categorie.Id);
            Assert.AreEqual(libelle, categorie.Libelle);
        }

        // Teste la propriété Id
        [TestMethod]
        public void Categorie_Id_AssigneCorrectement()
        {
            string id = "2";
            Categorie categorie = new Categorie(id, "Genre");
            Assert.AreEqual(id, categorie.Id);
        }

        // Teste la propriété Libelle
        [TestMethod]
        public void Categorie_Libelle_AssigneCorrectement()
        {
            string libelle = "Rayon";
            Categorie categorie = new Categorie("3", libelle);
            Assert.AreEqual(libelle, categorie.Libelle);
        }

        // Teste la méthode ToString() pour la représentation du libellé
        [TestMethod]
        public void Categorie_ToString_RetourneLibelle()
        {
            string libelle = "Genre";
            Categorie categorie = new Categorie("4", libelle);
            Assert.AreEqual(libelle, categorie.ToString());
        }
    }
}
