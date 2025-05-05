using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class UtilisateurTest
    {
        // Vérifie si l'utilisateur est bien créé avec les valeurs passées au constructeur
        [TestMethod]
        public void Utilisateur_Constructeur_AssigneValeursCorrectement()
        {
            Utilisateur utilisateur = new Utilisateur(1, "Jean Dupont", "password123", 2);

            Assert.AreEqual(1, utilisateur.Id);
            Assert.AreEqual("Jean Dupont", utilisateur.Nom);
            Assert.AreEqual("password123", utilisateur.MotDePasse);
            Assert.AreEqual(2, utilisateur.IdService);
        }

        // Vérifie si les setters et getters fonctionnent correctement
        [TestMethod]
        public void Utilisateur_GettersSetters_ModifieValeursCorrectement()
        {
            Utilisateur utilisateur = new Utilisateur(1, "Jean Dupont", "password123", 2);

            utilisateur.Id = 5;
            utilisateur.Nom = "Marie Curie";
            utilisateur.MotDePasse = "securePass!";
            utilisateur.IdService = 10;

            Assert.AreEqual(5, utilisateur.Id);
            Assert.AreEqual("Marie Curie", utilisateur.Nom);
            Assert.AreEqual("securePass!", utilisateur.MotDePasse);
            Assert.AreEqual(10, utilisateur.IdService);
        }

        // Vérifie si la classe gère les valeurs limites (id négatif, chaîne vide)
        [TestMethod]
        public void Utilisateur_Initialisation_ValeursLimites()
        {
            Utilisateur utilisateur = new Utilisateur(-1, "", "", -5);
            Assert.AreEqual(-1, utilisateur.Id);
            Assert.AreEqual("", utilisateur.Nom);
            Assert.AreEqual("", utilisateur.MotDePasse);
            Assert.AreEqual(-5, utilisateur.IdService);
        }
    }
}
