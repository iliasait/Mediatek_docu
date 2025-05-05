using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un service (exemple : un département ou une équipe).
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Identifiant unique du service.
        /// </summary>
        public int IdService { get; set; }

        /// <summary>
        /// Nom du service.
        /// </summary>
        public string NomService { get; set; }

        /// <summary>
        /// Constructeur de la classe Service.
        /// </summary>
        /// <param name="idService">Identifiant unique du service.</param>
        /// <param name="nomService">Nom du service.</param>
        public Service(int idService, string nomService)
        {
            this.IdService = idService;
            this.NomService = nomService;
        }
    }
}