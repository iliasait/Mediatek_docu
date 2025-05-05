﻿using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// Getter sur les suivis
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Getter sur les etats
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        #region Commandes de livres et Dvd
        /// <summary>
        /// Récupère les commandes d'un livre
        /// </summary>
        /// <param name="idLivre">id du livre concernée</param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesLivres(string idLivre)
        {
            return access.GetCommandesLivres(idLivre);
        }
        /// <summary>
        /// AJOUTE une commande dans la base de données
        /// </summary>
        /// <param name="detailsCommande">L'objet DetailsCommande à insérer</param>
        /// <returns>Création du details</returns>
        public bool CreerCommandeDocument(CommandeDocument detailsCommande)
        {
            return access.CreerCommandeDocument(detailsCommande);
        }
        /// <summary>
        /// MODIFIE une commande dans la base de données
        /// </summary>
        /// <param name="detailsCommande">L'objet DetailsCommande à insérer</param>
        /// <returns>Création du details</returns>
        public bool ModifierCommandeDocument(CommandeDocument detailsCommande)
        {
            return access.ModifierCommandeDocument(detailsCommande);
        }
        /// <summary>
        /// SUPPRIME une commande dans la base de données
        /// </summary>
        /// <param name="detailsCommande">L'objet DetailsCommande à insérer</param>
        /// <returns>Création du details</returns>
        public bool SupprimerCommandeDocument(CommandeDocument detailsCommande)
        {
            return access.SupprimerCommandeDocument(detailsCommande);
        }

        #endregion

        #region Les Abonnements
        /// <summary>
        /// Récupère les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns></returns>
        public List<Abonnement> GetCommandesAbonnement(string idRevue)
        {
            return access.GetCommandesAbonnement(idRevue);
        }
        /// <summary>
        /// Créer le détail d'une commande abonnement
        /// </summary>
        /// <param name="detailsCommande"></param>
        /// <returns></returns>
        public bool CreerCommandeAbonnement(Abonnement detailsCommande)
        {
            return access.CreerCommandeAbonnement(detailsCommande);
        }
        /// <summary>
        /// Supprimer le détail d'une commande abonnement
        /// </summary>
        /// <param name="detailsCommande"></param>
        /// <returns></returns>
        public bool SupprimerCommandeAbonnement(Abonnement detailsCommande)
        {
            return access.SupprimerCommandeAbonnement(detailsCommande);
        }
        /// <summary>
        /// Modfier le détail d'une commande abonnement
        /// </summary>
        /// <param name="detailsCommande"></param>
        /// <returns></returns>
        public bool ModifierCommandeAbonnement(Abonnement detailsCommande)
        {
            return access.ModifierCommandeAbonnement(detailsCommande);
        }
        /// <summary>
        /// Récupérer la liste des abonnements qui se termines dans les 30 jours
        /// </summary>
        /// <param name="idAbo"></param>
        /// <returns></returns>
        public List<FinAbonnement30Jours> GetListeFinAbonnement(string idAbo)
        {
            return access.GetListeFinAbonnement(idAbo);
        }
        #endregion

        #region Utilisateur
        /// <summary>
        /// Récupérer la liste des utilisateurs
        /// </summary>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        public List<Utilisateur> GetUserInfo(Utilisateur infoUser)
        {
            return access.GetUserInfo(infoUser);

        }
        #endregion
    }
}
