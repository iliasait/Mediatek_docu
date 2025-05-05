using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class AbonnementTest
    {
        // Teste le constructeur et vérifie que toutes les propriétés sont bien assignées
        [TestMethod]
        public void Abonnement_Constructeur_AssigneValeursCorrectement()
        {
            int? idCommande = 1;
            string idRevue = "Revue001";
            DateTime dateCommande = new DateTime(2025, 1, 25);
            double montant = 19.99;
            DateTime? dateFinAbonnement = new DateTime(2026, 1, 25);

            Abonnement abonnement = new Abonnement(idCommande, dateCommande, montant, dateFinAbonnement, idRevue);

            // Vérifications des valeurs
            Assert.AreEqual(idCommande, abonnement.Id);
            Assert.AreEqual(idRevue, abonnement.IdRevue);
            Assert.AreEqual(dateCommande, abonnement.DateCommande);
            Assert.AreEqual(montant, abonnement.Montant);
            Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement);
        }

        // Teste la propriété Id
        [TestMethod]
        public void Abonnement_Id_AssigneCorrectement()
        {
            int? idCommande = 2;
            Abonnement abonnement = new Abonnement(idCommande, new DateTime(2025, 2, 25), 29.99, null, "Revue002");
            Assert.AreEqual(idCommande, abonnement.Id);
        }

        // Teste la propriété IdRevue
        [TestMethod]
        public void Abonnement_IdRevue_AssigneCorrectement()
        {
            string idRevue = "Revue003";
            Abonnement abonnement = new Abonnement(3, new DateTime(2025, 3, 25), 39.99, new DateTime(2026, 3, 25), idRevue);
            Assert.AreEqual(idRevue, abonnement.IdRevue);
        }

        // Teste la propriété DateCommande
        [TestMethod]
        public void Abonnement_DateCommande_AssigneCorrectement()
        {
            DateTime dateCommande = new DateTime(2025, 4, 25);
            Abonnement abonnement = new Abonnement(4, dateCommande, 49.99, null, "Revue004");
            Assert.AreEqual(dateCommande, abonnement.DateCommande);
        }

        // Teste la propriété Montant
        [TestMethod]
        public void Abonnement_Montant_AssigneCorrectement()
        {
            double montant = 59.99;
            Abonnement abonnement = new Abonnement(5, new DateTime(2025, 5, 25), montant, null, "Revue005");
            Assert.AreEqual(montant, abonnement.Montant);
        }

        // Teste la propriété DateFinAbonnement avec valeur null
        [TestMethod]
        public void Abonnement_DateFinAbonnement_Null()
        {
            DateTime? dateFinAbonnement = null;
            Abonnement abonnement = new Abonnement(6, new DateTime(2025, 6, 25), 69.99, dateFinAbonnement, "Revue006");
            Assert.IsNull(abonnement.DateFinAbonnement);
        }

        // Teste la propriété DateFinAbonnement avec valeur non null
        [TestMethod]
        public void Abonnement_DateFinAbonnement_NonNull()
        {
            DateTime? dateFinAbonnement = new DateTime(2026, 6, 25);
            Abonnement abonnement = new Abonnement(7, new DateTime(2025, 7, 25), 79.99, dateFinAbonnement, "Revue007");
            Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement);
        }
    }
}
