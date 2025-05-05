using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class FinAbonnement30JoursTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs à DateFinAbonnement et Titre
        [TestMethod]
        public void FinAbonnement30Jours_Constructeur_AssigneValeursCorrectement()
        {
            string titre = "Abonnement A";
            string dateFinAbonnement = "2025-12-31";

            FinAbonnement30Jours abonnement = new FinAbonnement30Jours(titre, dateFinAbonnement);

            Assert.AreEqual(titre, abonnement.Titre);
            Assert.AreEqual(new DateTime(2025, 12, 31), abonnement.DateFinAbonnement);
        }
    }
}
