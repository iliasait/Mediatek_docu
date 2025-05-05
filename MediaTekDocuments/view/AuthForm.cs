using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BCrypt.Net;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    public partial class AuthForm : Form
    {
        private readonly FrmMediatekController controller;

        /// <summary>
        /// Constructeur de la fenêtre d'authentification.
        /// Initialise les composants et le contrôleur.
        /// </summary>
        public AuthForm()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
        }

        /// <summary>
        /// Gère l'événement du clic sur le bouton de connexion.
        /// Vérifie les champs, valide l'utilisateur et redirige vers la page d'accueil si l'authentification réussit.
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            string nomUtilisateur = txbIdentifiantUser.Text;
            string motDePasse = txbMotDePasseUser.Text;

            if (AreFieldsValid(nomUtilisateur, motDePasse))
            {
                Utilisateur utilisateurTrouve = GetUserInfo(nomUtilisateur);

                if (utilisateurTrouve != null && IsPasswordValid(motDePasse, utilisateurTrouve))
                {
                    HandleSuccessfulLogin(utilisateurTrouve);
                }
                else
                {
                    ShowErrorMessage("Nom d'utilisateur incorrect ou mot de passe incorrect.",
                                      "Erreur d'authentification");
                }
            }
            else
            {
                ShowErrorMessage("Veuillez remplir tous les champs.", "Erreur de saisie", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Vérifie si les champs utilisateur et mot de passe sont remplis.
        /// </summary>
        /// <param name="nomUtilisateur">Nom d'utilisateur saisi.</param>
        /// <param name="motDePasse">Mot de passe saisi.</param>
        /// <returns>True si les champs sont remplis, sinon False.</returns>
        private bool AreFieldsValid(string nomUtilisateur, string motDePasse)
        {
            return !string.IsNullOrEmpty(nomUtilisateur) && !string.IsNullOrEmpty(motDePasse);
        }

        /// <summary>
        /// Récupère les informations de l'utilisateur depuis le contrôleur.
        /// </summary>
        /// <param name="nomUtilisateur">Nom de l'utilisateur à rechercher.</param>
        /// <returns>Objet Utilisateur correspondant ou null si non trouvé.</returns>
        private Utilisateur GetUserInfo(string nomUtilisateur)
        {
            Utilisateur infoUser = new Utilisateur(1, nomUtilisateur, string.Empty, 1);
            List<Utilisateur> utilisateurs = controller.GetUserInfo(infoUser);
            return utilisateurs.Find(u => u.Nom == nomUtilisateur);
        }

        /// <summary>
        /// Vérifie si le mot de passe saisi correspond au mot de passe haché enregistré.
        /// </summary>
        /// <param name="motDePasse">Mot de passe saisi.</param>
        /// <param name="utilisateur">Utilisateur trouvé.</param>
        /// <returns>True si le mot de passe est valide, sinon False.</returns>
        private bool IsPasswordValid(string motDePasse, Utilisateur utilisateur)
        {
            return BCrypt.Net.BCrypt.Verify(motDePasse, utilisateur.MotDePasse);
        }

        /// <summary>
        /// Gère la connexion réussie de l'utilisateur et redirige vers l'interface principale.
        /// </summary>
        /// <param name="utilisateurTrouve">Utilisateur authentifié.</param>
        private void HandleSuccessfulLogin(Utilisateur utilisateurTrouve)
        {
            FrmMediatek accueilForm = new FrmMediatek();

            if (utilisateurTrouve.IdService == 3) // Service "Culture"
            {
                MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à cette application.",
                                "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            else
            {
                ConfigureAccueilForm(utilisateurTrouve, accueilForm);
            }

            accueilForm.Owner = this;
            this.Visible = false;
            accueilForm.Show();
        }

        /// <summary>
        /// Configure les options d'affichage de l'accueil en fonction du service de l'utilisateur.
        /// </summary>
        /// <param name="utilisateur">Utilisateur authentifié.</param>
        /// <param name="accueilForm">Formulaire principal à configurer.</param>
        private void ConfigureAccueilForm(Utilisateur utilisateur, FrmMediatek accueilForm)
        {
            if (utilisateur.IdService == 2) // Service "Prêts"
            {
                accueilForm.CacherOnglet();
            }
            else if (utilisateur.IdService == 1 || utilisateur.IdService == 4) // Service "Administratif" ou Administrateur
            {
                accueilForm.AfficherAbonnementsFinDans30Jours();
            }
        }

        /// <summary>
        /// Affiche un message d'erreur sous forme de boîte de dialogue.
        /// </summary>
        /// <param name="message">Message d'erreur à afficher.</param>
        /// <param name="caption">Titre de la boîte de dialogue.</param>
        /// <param name="icon">Icône d'erreur (par défaut : erreur).</param>
        private void ShowErrorMessage(string message, string caption, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
        }
    }
}
