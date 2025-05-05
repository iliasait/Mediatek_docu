using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class ParutionDansAbonnementTest
    {
        [TestMethod]
        // REUSSI : la date de parution est entre dateCommande et DateFinAbonnement
        public void ParutionDansAbonnement_DateDansPlage_RetourneVrai()
        {
            // préparation des dates avec DateTimeKind.Utc
            DateTime dateCommande = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2025, 6, 15, 12, 0, 0, DateTimeKind.Utc);

            // appel de la méthode à tester
            bool result = Abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // vérifie que le résultat est correct
            Assert.IsTrue(result);
        }

        [TestMethod]
        // ECHEC : la date de parution est inférieur à dateCommande 
        public void ParutionDansAbonnement_DateAvantCommande_RetourneFaux()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc);

            bool result = Abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsFalse(result);
        }

        [TestMethod]
        // ECHEC : la date de parution est supérieur à DateFinAbonnement 
        public void ParutionDansAbonnement_DateApresFinAbonnement_RetourneFaux()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateParution = new DateTime(2027, 6, 15, 12, 0, 0, DateTimeKind.Utc);

            bool result = Abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsFalse(result);
        }
    }
}
