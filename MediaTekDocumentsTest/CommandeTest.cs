using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CommandeTest
    {
        // Teste le constructeur et vérifie que toutes les propriétés sont bien assignées
        [TestMethod]
        public void Commande_Constructeur_AssigneValeursCorrectement()
        {
            int? id = 1;
            DateTime dateCommande = new DateTime(2025, 1, 25);
            double montant = 100.50;

            Commande commande = new Commande(id, dateCommande, montant);

            Assert.AreEqual(id, commande.Id);
            Assert.AreEqual(dateCommande, commande.DateCommande);
            Assert.AreEqual(montant, commande.Montant);
        }

        // Teste la propriété Id
        [TestMethod]
        public void Commande_Id_AssigneCorrectement()
        {
            int? id = 2;
            Commande commande = new Commande(id, DateTime.Now, 50.00);
            Assert.AreEqual(id, commande.Id);
        }

        // Teste la propriété DateCommande
        [TestMethod]
        public void Commande_DateCommande_AssigneCorrectement()
        {
            DateTime dateCommande = new DateTime(2025, 2, 15);
            Commande commande = new Commande(3, dateCommande, 75.00);
            Assert.AreEqual(dateCommande, commande.DateCommande);
        }

        // Teste la propriété Montant
        [TestMethod]
        public void Commande_Montant_AssigneCorrectement()
        {
            double montant = 150.75;
            Commande commande = new Commande(4, DateTime.Now, montant);
            Assert.AreEqual(montant, commande.Montant);
        }
    }
}
