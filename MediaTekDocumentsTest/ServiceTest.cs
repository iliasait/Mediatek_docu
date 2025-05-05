using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class ServiceTest
    {
        // Vérifie que le constructeur assigne correctement les valeurs
        [TestMethod]
        public void Service_Constructeur_AssigneValeursCorrectement()
        {
            Service service = new Service(1, "Informatique");

            Assert.AreEqual(1, service.IdService);
            Assert.AreEqual("Informatique", service.NomService);
        }
    }
}
