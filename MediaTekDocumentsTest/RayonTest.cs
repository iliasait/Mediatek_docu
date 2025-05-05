using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MediaTekDocuments.model;

namespace test_mediatekdocument.modele
{
    [TestClass]
    public class RayonTest
    {
        private const string id = "PR345";
        private const string libelle = "Manga";

        private readonly Rayon rayon = new Rayon(id, libelle);

        [TestMethod]
        public void TestConstructor()
        {
            Assert.AreEqual(id, rayon.Id, "Devrait réussir : Id retourné");
            Assert.AreEqual(libelle, rayon.Libelle, "Devrait réussir : Libelle retourné");
        }
    }
}