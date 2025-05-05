using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CommandeDocumentTest
    {
        // Teste le constructeur et vérifie que toutes les propriétés sont bien assignées
        [TestMethod]
        public void CommandeDocument_Constructeur_AssigneValeursCorrectement()
        {
            int? id = 1;
            DateTime dateCommande = new DateTime(2025, 1, 25);
            double montant = 100.50;
            int nbExemplaire = 3;
            string idLivreDvd = "L001";
            int idSuivi = 2;
            string etat = "En cours";

            CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, etat);

            Assert.AreEqual(id, commandeDocument.Id);
            Assert.AreEqual(dateCommande, commandeDocument.DateCommande);
            Assert.AreEqual(montant, commandeDocument.Montant);
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire);
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd);
            Assert.AreEqual(idSuivi, commandeDocument.IdSuivi);
            Assert.AreEqual(etat, commandeDocument.Etat);
        }

        // Teste la propriété NbExemplaire
        [TestMethod]
        public void CommandeDocument_NbExemplaire_AssigneCorrectement()
        {
            int nbExemplaire = 5;
            CommandeDocument commandeDocument = new CommandeDocument(1, DateTime.Now, 50.00, nbExemplaire, "L002", 1, "Livré");
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire);
        }

        // Teste la propriété IdLivreDvd
        [TestMethod]
        public void CommandeDocument_IdLivreDvd_AssigneCorrectement()
        {
            string idLivreDvd = "L003";
            CommandeDocument commandeDocument = new CommandeDocument(2, DateTime.Now, 75.50, 2, idLivreDvd, 1, "En cours");
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd);
        }

        // Teste la propriété IdSuivi
        [TestMethod]
        public void CommandeDocument_IdSuivi_AssigneCorrectement()
        {
            int idSuivi = 3;
            CommandeDocument commandeDocument = new CommandeDocument(3, DateTime.Now, 100.00, 1, "L004", idSuivi, "Livré");
            Assert.AreEqual(idSuivi, commandeDocument.IdSuivi);
        }

        // Teste la propriété Etat
        [TestMethod]
        public void CommandeDocument_Etat_AssigneCorrectement()
        {
            string etat = "Annulé";
            CommandeDocument commandeDocument = new CommandeDocument(4, DateTime.Now, 120.00, 2, "L005", 1, etat);
            Assert.AreEqual(etat, commandeDocument.Etat);
        }
    }
}
