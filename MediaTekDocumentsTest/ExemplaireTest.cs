using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class ExemplaireTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs à chaque propriété
        [TestMethod]
        public void Exemplaire_Constructeur_AssigneValeursCorrectement()
        {
            int numero = 1;
            DateTime dateAchat = new DateTime(2025, 1, 1);
            string photo = "photo.jpg";
            string idEtat = "Neuf";
            string idDocument = "1234";

            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);

            Assert.AreEqual(numero, exemplaire.Numero);
            Assert.AreEqual(dateAchat, exemplaire.DateAchat);
            Assert.AreEqual(photo, exemplaire.Photo);
            Assert.AreEqual(idEtat, exemplaire.IdEtat);
            Assert.AreEqual(idDocument, exemplaire.Id);
        }

        // Vérifie que la date d'achat est correctement assignée
        [TestMethod]
        public void Exemplaire_DateAchat_AssigneCorrectement()
        {
            DateTime dateAchat = new DateTime(2025, 6, 15);
            Exemplaire exemplaire = new Exemplaire(1, dateAchat, "photo.jpg", "Neuf", "1234");
            Assert.AreEqual(dateAchat, exemplaire.DateAchat);
        }

    }
}
