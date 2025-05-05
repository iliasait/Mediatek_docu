using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private const string IdRayonColumn = "idRayon";
        private const string IdCommandeInvalide = "ID commande invalide.";
        private const string DateCommandeColumnName = "DateCommande";
        private const string IdGenreColumnName = "idGenre";
        private const string Modification = "Modification";
        private const string Suppression = "Suppression";
        private const string Ajout = "Ajout";
        private const string Annulee = "Annulée";
        private const string NbExemplaire = "NbExemplaire";
        private const string Montant = "Montant";
        private const string idPublic = "idPublic";
        private const string Erreur = "Erreur";
        private const string NumeroIntrouvable = "numéro introuvable";
        private const string Titre = "titre";
        private const string UneImage = "image";
        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }
        public void CacherOnglet()
        {
            TabPage tabCmdLivreToHide = tabOngletsApplication.TabPages["tabCmdLivre"];
            TabPage tabCmdDvdToHide = tabOngletsApplication.TabPages["tabCmdDvd"];
            TabPage tabCmdRevueToHide = tabOngletsApplication.TabPages["tabCmdRevue"];

            tabOngletsApplication.TabPages.Remove(tabCmdLivreToHide);
            tabOngletsApplication.TabPages.Remove(tabCmdDvdToHide);
            tabOngletsApplication.TabPages.Remove(tabCmdRevueToHide);
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Rempli un combo de suivi
        /// </summary>
        /// <param name="lesSuivis">liste des etats de suivis</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboSuivi(List<Suivi> lesSuivis, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesSuivis;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Renvoie la liste des revues dont l'abonnement se termine dans moins de 30 jours
        /// </summary>
        public void AfficherAbonnementsFinDans30Jours()
        {
            // Récupère la liste d'abonnements en appelant l'API
            var abonnementsFinDans30Jours = controller.GetListeFinAbonnement("idAbo");

            // Initialisation du StringBuilder pour construire le message
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Abonnements se terminant dans 30 jours :\n");

            // Ajout des informations des abonnements
            foreach (var abonnement in abonnementsFinDans30Jours)
            {
                string dateFinAbonnement = abonnement.DateFinAbonnement.ToString("dd/MM/yyyy");
                messageBuilder.AppendLine($"Titre : {abonnement.Titre} - Date de fin : {dateFinAbonnement}");
            }

            // Conversion du StringBuilder en chaîne
            string message = messageBuilder.ToString();

            // Affichage du message dans un MessageBox
            MessageBox.Show(message, "Alerte Abonnement", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Rempli un combo d'etat
        /// </summary>
        /// <param name="lesEtats"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        public void RemplirComboEtat(List<Etat> lesEtats, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesEtats;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }





        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }
        

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns[IdRayonColumn].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns[idPublic].Visible = false;
            dgvLivresListe.Columns[UneImage].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns[Titre].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns[IdRayonColumn].Visible = false;
            dgvDvdListe.Columns[IdGenreColumnName].Visible = false;
            dgvDvdListe.Columns[idPublic].Visible = false;
            dgvDvdListe.Columns[UneImage].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns[Titre].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns[IdRayonColumn].Visible = false;
            dgvRevuesListe.Columns[IdGenreColumnName].Visible = false;
            dgvRevuesListe.Columns[idPublic].Visible = false;
            dgvRevuesListe.Columns[UneImage].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns[Titre].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", Erreur);
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Onglet Commande de Livre
        private List<CommandeDocument> lesCommandesDocument = new List<CommandeDocument>();
        // Mode (Ajout, Supp, Modif)
        private string Mode = "";

        /// <summary>
        /// Gérer la visibilité du groupBox
        /// </summary>
        private void VisibleGroupBoxCommandeLivre()
        {
            label70.Visible = true;
            txbLivresComNbCommande.Visible = true;
            txbLivresComNbCommande.ReadOnly = true;
            txbLivresComMontant.ReadOnly = true;
            dtpLivresComDateCommande.Enabled = false;
            txbLivresComNbExemplaires.ReadOnly = true;
            txbLivresComNumLivre.Visible = false;
            cbxLivresComEtat.Visible = false;
            labelNumCmdLivre.Visible = false;
            labelEtatCmdLivre.Visible = false;
            btnLivresComValider.Visible = false;
            btnLivresComAnnuler.Visible = false;
        }
        /// <summary>
        /// Gérer la visibilité du groupBox et des boutons valider et annuler
        /// </summary>
        private void VisibleCommandeLivre()
        {
            label70.Visible = false;
            txbLivresComNbCommande.Visible = false;
            txbLivresComMontant.ReadOnly = false;
            dtpLivresComDateCommande.Enabled = true;
            txbLivresComNbExemplaires.ReadOnly = false;
            txbLivresComNumLivre.Visible = true;
            txbLivresComNumLivre.ReadOnly = true;
            labelEtatCmdLivre.Visible = false;
            cbxLivresComEtat.Visible = false;
            labelNumCmdLivre.Visible = true;
            btnLivresComValider.Visible = true;
            btnLivresComAnnuler.Visible = true;
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void ViderCmdLivresInfos()
        {
            txbLivresComNbCommande.Text = "";
            dtpLivresComDateCommande.Value = DateTime.Now;
            txbLivresComMontant.Text = "";
            txbLivresComNbExemplaires.Text = "";
            txbLivresComNumLivre.ReadOnly = true;
        }
        /// <summary>
        /// Modifie la visibilité de deux boutons pour les rendre invisibles si true, et visibles si falses.
        /// </summary>
        /// <param name="button1">Premier bouton</param>
        /// <param name="button2">Deuxième bouton</param>
        private void RendreBoutonsVisiblesOuInvisibles(Button button1, Button button2, Button button3, bool cacher)
        {
            if (cacher)
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
            else
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        /// <summary>
        /// Gére l'affichage des infos d'une commande dans les datagridview
        /// </summary>
        /// <param name="commande">Une commande de livres</param>
        private void AfficheCommandeLivreInfo(CommandeDocument commande)
        {
            if (dgvLivresComListe.Rows.Count > 0 && commande != null)
            {
                // Remplir les champs avec les info de la commande sélectionnée
                txbLivresComNbCommande.Text = commande.Id.ToString();
                dtpLivresComDateCommande.Value = commande.DateCommande;
                txbLivresComMontant.Text = commande.Montant.ToString("F2");
                txbLivresComNbExemplaires.Text = commande.NbExemplaire.ToString();


                // nettoyer les éléments existants dans le ComboBox
                cbxLivresComEtat.Items.Clear();
                // récupérer tous les suivis
                var suivis = controller.GetAllSuivis();
                // Ajouter les suivis dans le ComboBox
                var suivisItems = suivis.Select(s => new Suivi(s.Id, s.Etat)).ToArray();
                cbxLivresComEtat.Items.AddRange(suivisItems);
                // définir pair clé/valeur
                cbxLivresComEtat.DisplayMember = "Etat";
                cbxLivresComEtat.ValueMember = "Id";
                // trouver l'état de la commande + sélectionner dans le ComboBox
                var etatCommande = Array.Find(suivisItems, s => s.Id == commande.IdSuivi);
                if (etatCommande != null)
                {
                    cbxLivresComEtat.SelectedItem = etatCommande;
                }
            }

            else
            {
                ViderCmdLivresInfos();
            }
        }
        /// <summary>
        /// Remplit la dgv de commande de livres et vide les textbox etc...
        /// </summary>
        private void RemplirCmdLivresListeComplete()
        {
            RemplirCmdLivresListe(lesLivres);
            VideCmdLivresZones();
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideCmdLivresZones()
        {
            cbxLivresComGenres.SelectedIndex = -1;
            cbxLivresComRayons.SelectedIndex = -1;
            cbxLivresComPublics.SelectedIndex = -1;
            txtNumDoc.Text = "";
            txtCommandeLivresRecherche.Text = "";
        }
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirCmdLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresComListe.DataSource = bdgLivresListe;
            dgvLivresComListe.Columns["isbn"].Visible = false;
            dgvLivresComListe.Columns[IdRayonColumn].Visible = false;
            dgvLivresComListe.Columns[IdGenreColumnName].Visible = false;
            dgvLivresComListe.Columns[idPublic].Visible = false;
            dgvLivresComListe.Columns[UneImage].Visible = false;
            dgvLivresComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresComListe.Columns["id"].DisplayIndex = 0;
            dgvLivresComListe.Columns[Titre].DisplayIndex = 1;
        }
        /// <summary>
        /// Remplit le DataGridView avec les commandes associées au livre sélectionné.
        /// </summary>
        /// <param name="commandes">Liste des commandes.</param>
        private void RemplirCmdLivresCommandes(List<CommandeDocument> commandes)
        {
            BindingSource bdgCommandes = new BindingSource();
            bdgCommandes.DataSource = commandes;
            dgvLivresComListeCom.DataSource = bdgCommandes;

            // Réorganiser les colonnes en fonction de leur index d'affichage
            dgvLivresComListeCom.Columns["Id"].DisplayIndex = 0;
            dgvLivresComListeCom.Columns["DateCommande"].DisplayIndex = 1;
            dgvLivresComListeCom.Columns[Montant].DisplayIndex = 2;
            dgvLivresComListeCom.Columns[NbExemplaire].DisplayIndex = 3;
            dgvLivresComListeCom.Columns["Etat"].DisplayIndex = 4;
            dgvLivresComListeCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        /// <summary>
        /// Désactive la DataGridView pour ne pas avoir de problème en cas d'ajout ou de modification
        /// </summary>
        /// <param name="dgv">Le DataGridView à configurer</param>
        /// <param name="bloquer">Booléen à passer</param>
        private void DesactiverDataGridView(DataGridView dgv, bool bloquer)
        {
            if (bloquer)
            {
                // Désactiver la sélection et interdire les interactions
                dgv.ClearSelection();
                dgv.Enabled = false;
                dgv.ReadOnly = true;
            }
            else
            {
                // Réactiver la sélection et les interactions
                dgv.ClearSelection();
                dgv.Enabled = true;
                dgv.ReadOnly = true;
            }
        }
        /// <summary>
        /// Permet de gérer les Etat du comboBox en fonction du choix fait
        /// </summary>
        /// <param name="etatActuel">id de l'état actuel à passer</param>
        /// <param name="combobox">objet Combobox à passer</param>
        private void GererEtatComboBox(int etatActuel, ComboBox combobox)
        {
            combobox.Items.Clear();

            // Ajouter l'état actuel pour qu'il puisse être sélectionné
            string etatActuelLibelle = "";
            if (etatActuel == 1) etatActuelLibelle = "En cours";
            else if (etatActuel == 2) etatActuelLibelle = "Livrée";
            else if (etatActuel == 3) etatActuelLibelle = "Réglée";
            else if (etatActuel == 4) etatActuelLibelle = "Relancée";
            else if (etatActuel == 5) etatActuelLibelle = "Annulée";

            if (!string.IsNullOrEmpty(etatActuelLibelle))
            {
                combobox.Items.Add(new Suivi(etatActuel, etatActuelLibelle));
            }

            switch (etatActuel)
            {
                case 1: // En cours
                    combobox.Items.Add(new Suivi(2, "Livrée"));
                    combobox.Items.Add(new Suivi(4, "Relancée"));
                    combobox.Items.Add(new Suivi(5, Annulee));
                    break;

                case 2: // Livrée
                    combobox.Items.Add(new Suivi(3, "Réglée"));
                    combobox.Items.Add(new Suivi(5, Annulee));
                    break;

                case 3: // Réglée
                    combobox.Items.Add(new Suivi(5, Annulee));
                    break;

                case 4: // Relancée
                    combobox.Items.Add(new Suivi(2, "Livrée"));
                    combobox.Items.Add(new Suivi(5, Annulee));
                    break;

                case 5: // Annulée
                    MessageBox.Show("Une commande annulée ne peut pas changer d'état.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                default:
                    MessageBox.Show("État inconnu.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // Sélectionner automatiquement l'état actuel si présent
            foreach (Suivi item in combobox.Items)
            {
                if (item.Id == etatActuel)
                {
                    combobox.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Affiche ou non le label sur les opérations CRUD
        /// </summary>
        /// <param name="mode">Le mode d'opération à effectuer</param>
        /// <param name="cacher">Booléen à passer</param>
        private void LabelCrudTitre(string mode, bool cacher)
        {
            // Visibilité du texte
            lblCrudTitre.Visible = cacher;

            switch (mode)
            {
                case Ajout:
                    lblCrudTitre.Text = "Êtes-vous sûr de réaliser cet ajout ?";
                    break;
                case Suppression:
                    lblCrudTitre.Text = "Êtes-vous sûr de réaliser cette suppression ?";
                    break;
                case Modification:
                    lblCrudTitre.Text = "Êtes-vous sûr de réaliser cette modification ?";
                    break;
                default:
                    lblCrudTitre.Text = "";
                    break;
            }
        }
        /// <summary>
        /// Méthode qui vérifie si une ligne est bien sélectionnée dans la DGV avant de réaliser une opération
        /// </summary>
        /// <param name="dgvLivresComListeCom">DGV</param>
        private bool VerifierSelection(DataGridView dgvLivresComListeCom)
        {
            if (dgvLivresComListeCom.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vous devez sélectionner une ligne pour réaliser cette opération.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Aucun élément sélectionné
            }
            return true; // Une ligne est sélectionnée
        }
        /// <summary>
        /// Cache les boutons de validation et gère aussi l'affichage
        /// </summary>
        private void CacherValider()
        {
            LabelCrudTitre(null, false);
            VisibleGroupBoxCommandeLivre();
            DesactiverDataGridView(dgvLivresComListe, false);
            DesactiverDataGridView(dgvLivresComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, false);
        }


        /// <summary>
        /// Evenement d'entrée dans le tab (ouverture de l'onglet)
        /// </summary>
        private void TabCmdLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresComGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresComPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresComRayons);
            RemplirCmdLivresListeComplete();
            VisibleGroupBoxCommandeLivre();
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du premier combobox
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            RemplirCmdLivresListeComplete();
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du deuxième combobox
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            RemplirCmdLivresListeComplete();
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du troisième combobox
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            RemplirCmdLivresListeComplete();
        }
        /// <summary>
        /// Recherche d'un livre via son titre dès que l'on commence à écrire dans la textbox
        /// </summary>
        private void txtCommandeLivresRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txtCommandeLivresRecherche.Text.Equals(""))
            {
                cbxLivresComGenres.SelectedIndex = -1;
                cbxLivresComRayons.SelectedIndex = -1;
                cbxLivresComPublics.SelectedIndex = -1;
                txtNumDoc.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txtCommandeLivresRecherche.Text.ToLower()));
                RemplirCmdLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresComGenres.SelectedIndex < 0 && cbxLivresComPublics.SelectedIndex < 0 && cbxLivresComRayons.SelectedIndex < 0
                    && txtNumDoc.Text.Equals(""))
                {
                    RemplirCmdLivresListeComplete();
                }
            }
        }
        /// <summary>
        /// Recherche d'un livre via son numéro dès que l'on clique sur le bouton 
        /// </summary>
        private void btnRechercherCom_Click(object sender, EventArgs e)
        {
            if (!txtNumDoc.Text.Equals(""))
            {
                txtCommandeLivresRecherche.Text = "";
                cbxLivresComGenres.SelectedIndex = -1;
                cbxLivresComRayons.SelectedIndex = -1;
                cbxLivresComPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtNumDoc.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirCmdLivresListe(livres);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                    RemplirCmdLivresListeComplete();
                }
            }
            else
            {
                RemplirCmdLivresListeComplete();
            }
        }
        /// <summary>
        /// Première datagridview qui affiche la liste des livres dans la bdd
        /// </summary>
        private void dgvLivresComListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresComListe.CurrentRow != null)
            {
                // Récupère le livre sélectionné
                Livre livreSelectionne = (Livre)dgvLivresComListe.CurrentRow.DataBoundItem;

                // Utilise la méthode du contrôleur pour récupérer les commandes associées au livre sélectionné
                List<CommandeDocument> commandesAssociees = controller.GetCommandesLivres(livreSelectionne.Id);
                lesCommandesDocument = controller.GetCommandesLivres(livreSelectionne.Id);
                // Remplir la DataGridView avec les commandes récupérées
                RemplirCmdLivresCommandes(commandesAssociees);
                txbLivresComNumLivre.Text = livreSelectionne.Id.ToString();

            }
        }
        /// <summary>
        /// Evenement de sélection sur la deuxième datagridview, qui affiche les commandes pour un livre sélectionné dans la première dgv
        /// </summary>
        private void dgvLivresComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresComListeCom.CurrentRow != null)
            {
                // Récupérer la commande sélectionnée dans la DataGridView
                var commande = (CommandeDocument)dgvLivresComListeCom.CurrentRow.DataBoundItem;
                // Appeler AfficheCommandeLivreInfo avec une seule commande
                AfficheCommandeLivreInfo(commande);
            }
        }
        /// <summary>
        ///Evenement au clic sur le bouton Modifier, on passe la variable mode à "modification" et l'on gère l'affichage
        /// </summary>
        private void btnLivresComModifier_Click(object sender, EventArgs e)
        {
            Mode = Modification;

            if (!VerifierSelection(dgvLivresComListeCom))
            {
                Mode = "";
            }
            else
            {
                LabelCrudTitre(Mode, true);
                VisibleCommandeLivre();
                RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, true);
                cbxLivresComEtat.Visible = true;
                DesactiverDataGridView(dgvLivresComListe, true);
                DesactiverDataGridView(dgvLivresComListeCom, true);
                // Vérifier si un élément a été sélectionné dans le ComboBox
                if (cbxLivresComEtat.SelectedItem != null)
                {
                    // Récupérer l'objet Suivi sélectionné
                    Suivi suiviSelectionne = (Suivi)cbxLivresComEtat.SelectedItem;

                    // Récupérer l'ID du suivi sélectionné
                    int idSuivi = suiviSelectionne.Id;
                    GererEtatComboBox(idSuivi, cbxLivresComEtat);
                }
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton Supprimer, on passe la variable mode à "suppression" et l'on gère l'affichage
        /// </summary>
        private void btnLivresComSupprimer_Click(object sender, EventArgs e)
        {
            Mode = Suppression;
            Suivi suiviSelectionne = (Suivi)cbxLivresComEtat.SelectedItem;
            if (!VerifierSelection(dgvLivresComListeCom))
            {
                Mode = "";
            }
            // Vérifie si l'ID de suivi est 2 (livré)
            if (suiviSelectionne != null && suiviSelectionne.Id == 2)
            {
                // Affiche une boîte de dialogue avec le message d'erreur
                MessageBox.Show("Impossible de supprimer une commande de Livres déjà livrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LabelCrudTitre(Mode, true);
                groupBoxInfoCommandeLivre.Visible = false;
                RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, true);
                VisibleCommandeLivre();
            }

        }
        /// <summary>
        /// Evenement au clic sur le bouton Ajout, on passe la variable mode à "ajout" et l'on gère l'affichage
        /// </summary>
        private void btnLivresComAjouter_Click(object sender, EventArgs e)
        {
            Mode = Ajout;
            LabelCrudTitre(Mode, true);
            VisibleCommandeLivre();
            RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, true);
            DesactiverDataGridView(dgvLivresComListe, true);
            DesactiverDataGridView(dgvLivresComListeCom, true);
            ViderCmdLivresInfos();
        }
        /// <summary>
        /// Evenement au clic sur le bouton Valider. La logique CRUD est présente ici via un switch
        /// </summary>
        private void btnLivresComValider_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération des données communes
                DateTime dateCommande = dtpLivresComDateCommande.Value;
                double montant;
                if (!double.TryParse(txbLivresComMontant.Text, out montant))
                {
                    MessageBox.Show("Montant invalide.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int nbExemplaires;
                if (!int.TryParse(txbLivresComNbExemplaires.Text, out nbExemplaires))
                {
                    MessageBox.Show("Nombre d'exemplaires invalide.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string numLivre = txbLivresComNumLivre.Text;
                if (string.IsNullOrEmpty(numLivre))
                {
                    MessageBox.Show("Numéro du Livre manquant.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CommandeDocument commandeDocument = null;

                // Vérifie le mode sélectionné et crée l'objet `CommandeDocument`
                switch (Mode)
                {
                    case Ajout:
                        // Valeurs spécifiques à l'ajout
                        commandeDocument = new CommandeDocument(null, dateCommande, montant, nbExemplaires, numLivre, 1, "En Cours");
                        ExecuterOperation(() => controller.CreerCommandeDocument(commandeDocument), "Commande ajoutée avec succès.", "Erreur lors de l'ajout.");
                        List<CommandeDocument> commandesAssociees = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = commandesAssociees;  // ou utiliser directement la variable si nécessaire
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdLivresCommandes(commandesAssociees);
                        ViderCmdLivresInfos();
                        CacherValider();
                        break;

                    case Suppression:
                        // Vérification de la sélection dans le ComboBox
                        if (cbxLivresComEtat.SelectedItem == null)
                        {
                            MessageBox.Show("Veuillez sélectionner un état de suivi pour la suppression.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        Suivi suiviSelectionneSupp = (Suivi)cbxLivresComEtat.SelectedItem;
                        int idSuiviSupp = suiviSelectionneSupp.Id;
                        string etatSuiviSupp = suiviSelectionneSupp.Etat;

                        int idCommandeSupp;
                        if (!int.TryParse(txbLivresComNbCommande.Text, out idCommandeSupp))
                        {
                            MessageBox.Show(IdCommandeInvalide, Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Ou toute autre gestion d'erreur que vous préférez.
                        }
                        commandeDocument = new CommandeDocument(idCommandeSupp, dateCommande, montant, nbExemplaires, numLivre, idSuiviSupp, etatSuiviSupp);
                        ExecuterOperation(() => controller.SupprimerCommandeDocument(commandeDocument), "Commande supprimée avec succès.", "Erreur lors de la suppression.");
                        
                        List<CommandeDocument> commandesAssocieesSupp = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = commandesAssocieesSupp; 
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdLivresCommandes(commandesAssocieesSupp);

                        groupBoxInfoCommandeLivre.Visible = true;
                        ViderCmdLivresInfos();
                        CacherValider();
                        break;

                    case Modification:
                        // Vérification de la sélection dans le ComboBox
                        if (cbxLivresComEtat.SelectedItem == null)
                        {
                            MessageBox.Show("Veuillez sélectionner un état de suivi pour la modification.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        Suivi suiviSelectionneModif = (Suivi)cbxLivresComEtat.SelectedItem;
                        int idSuiviModif = suiviSelectionneModif.Id;
                        string etatSuiviModif = suiviSelectionneModif.Etat;

                        int idCommandeModif;
                        if (!int.TryParse(txbLivresComNbCommande.Text, out idCommandeModif))
                        {
                            MessageBox.Show(IdCommandeInvalide, Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        commandeDocument = new CommandeDocument(idCommandeModif, dateCommande, montant, nbExemplaires, numLivre, idSuiviModif, etatSuiviModif);
                        ExecuterOperation(() => controller.ModifierCommandeDocument(commandeDocument), "Commande modifiée avec succès.", "Erreur lors de la modification.");


                        List<CommandeDocument> commandesAssocieesModif = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = commandesAssocieesModif;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdLivresCommandes(commandesAssocieesModif);

                        ViderCmdLivresInfos();
                        CacherValider();
                        break;

                    default:
                        MessageBox.Show("Veuillez sélectionner une opération.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Méthode pour exécuter une opération et afficher un message selon le résultat.
        /// </summary>
        private void ExecuterOperation(Func<bool> operation, string messageSucces, string messageErreur)
        {
            try
            {
                bool result = operation();
                MessageBox.Show(result ? messageSucces : messageErreur, result ? "Succès" : Erreur, MessageBoxButtons.OK, result ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur inattendue est survenue : {ex.Message}", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton annuler. On gère principalement l'affichage.
        /// </summary>
        private void btnLivresComAnnuler_Click(object sender, EventArgs e)
        {
            LabelCrudTitre(null, false);
            VisibleGroupBoxCommandeLivre();
            DesactiverDataGridView(dgvLivresComListe, false);
            DesactiverDataGridView(dgvLivresComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, false);
            groupBoxInfoCommandeLivre.Visible = true;

        }
        /// <summary>
        /// Tri sur les colonnes de la datagridview
        /// </summary>
        private void dgvLivresComListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideCmdLivresZones();
            string titreColonne = dgvLivresComListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirCmdLivresListe(sortedList);
        }
        /// <summary>
        /// Tri sur les colonnes de la datagridview
        /// </summary>
        private void dgvLivresComListeCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ViderCmdLivresInfos();
                string titreColonne = dgvLivresComListeCom.Columns[e.ColumnIndex].HeaderText;
                List<CommandeDocument> sortedList;
                switch (titreColonne)
                {
                    case "Id":
                        sortedList = lesCommandesDocument.OrderBy(o => o.Id).ToList();
                        break;
                    case "DateCommande":
                        sortedList = lesCommandesDocument.OrderBy(o => o.DateCommande).ToList();
                        break;
                    case "Montant":
                        sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                        break;
                    case "NbExemplaire":
                        sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                        break;
                    case "Etat":
                        sortedList = lesCommandesDocument.OrderBy(o => o.Etat).ToList();
                        break;
                    case "IdLivreDvd":
                        sortedList = lesCommandesDocument.OrderBy(o => o.IdLivreDvd).ToList();
                        break;
                    case "IdSuivi":
                        sortedList = lesCommandesDocument.OrderBy(o => o.IdSuivi).ToList();
                        break;
                    default:
                        MessageBox.Show("Colonne inconnue");
                        return;
                }

                RemplirCmdLivresCommandes(sortedList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
        #endregion

        #region Onglet Commande de DVD
        /// <summary>
        /// Gérer la visibilité du groupBox
        /// </summary>
        private void VisibleGroupBoxCommandeDvd()
        {
            label80.Visible = true;
            txbDvdComNbCommande.Visible = true;
            txbDvdComNbCommande.ReadOnly = true;
            txbDvdComMontant.ReadOnly = true;
            dtpDvdComDateCommande.Enabled = false;
            txbDvdComNbExemplaires.ReadOnly = true;
            txbComNumDvd.Visible = false;
            cbxDvdComEtat.Visible = false;
            labelNumCmdDvd.Visible = false;
            labelEtatCmdDvd.Visible = false;
            btnDvdComValider.Visible = false;
            btnDvdComAnnuler.Visible = false;
        }
        /// <summary>
        /// Gérer la visibilité du groupBox et des boutons valider et annuler
        /// </summary>
        private void VisibleCommandeDvd()
        {
            label80.Visible = false;
            txbDvdComNbCommande.Visible = false;
            txbDvdComMontant.ReadOnly = false;
            dtpDvdComDateCommande.Enabled = true;
            txbDvdComNbExemplaires.ReadOnly = false;
            txbComNumDvd.Visible = true;
            txbComNumDvd.ReadOnly = true;
            labelEtatCmdDvd.Visible = false;
            cbxDvdComEtat.Visible = false;
            labelNumCmdDvd.Visible = true;
            btnDvdComValider.Visible = true;
            btnDvdComAnnuler.Visible = true;
        }
        /// <summary>
        /// Remplit la dgv de commande de dvd et vide les textbox etc...
        /// </summary>
        private void RemplirCmdDvdListeComplete()
        {
            RemplirCmdDvdListe(lesDvd);
            VideCmdDvdZones();
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void ViderCmdDvdInfos()
        {
            txbDvdComNbCommande.Text = "";
            dtpDvdComDateCommande.Value = DateTime.Now;
            txbDvdComMontant.Text = "";
            txbDvdComNbExemplaires.Text = "";
            txbComNumDvd.ReadOnly = true;
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideCmdDvdZones()
        {
            cbxCmdDvdGenres.SelectedIndex = -1;
            cbxCmdDvdRayons.SelectedIndex = -1;
            cbxCmdDvdPublics.SelectedIndex = -1;
            txbCmdDvdNumRecherche.Text = "";
            txbCmdDvdTitreRecherche.Text = "";
        }
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirCmdDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdComListe.DataSource = bdgDvdListe;
            dgvDvdComListe.Columns[IdRayonColumn].Visible = false;
            dgvDvdComListe.Columns[IdGenreColumnName].Visible = false;
            dgvDvdComListe.Columns[idPublic].Visible = false;
            dgvDvdComListe.Columns[UneImage].Visible = false;
            dgvDvdComListe.Columns["synopsis"].Visible = false;
            dgvDvdComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdComListe.Columns["id"].DisplayIndex = 0;
            dgvDvdComListe.Columns[Titre].DisplayIndex = 1;
        }
        /// <summary>
        /// Remplit le DataGridView avec les commandes associées au dvd sélectionné.
        /// </summary>
        /// <param name="commandes">Liste des commandes.</param>
        private void RemplirCmdDvdCommandes(List<CommandeDocument> commandes)
        {
            BindingSource bdgCommandes = new BindingSource();
            bdgCommandes.DataSource = commandes;
            dgvDvdComListeCom.DataSource = bdgCommandes;

            // Réorganiser les colonnes en fonction de leur index d'affichage
            dgvDvdComListeCom.Columns["Id"].DisplayIndex = 0;
            dgvDvdComListeCom.Columns[DateCommandeColumnName].DisplayIndex = 1;
            dgvDvdComListeCom.Columns[Montant].DisplayIndex = 2;
            dgvDvdComListeCom.Columns[NbExemplaire].DisplayIndex = 3;
            dgvDvdComListeCom.Columns["Etat"].DisplayIndex = 4;

            dgvDvdComListeCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        /// <summary>
        /// Gére l'affichage des infos d'une commande dans les datagridview
        /// </summary>
        /// <param name="commande">Une commande de dvd</param>
        private void AfficheCommandeDvdInfo(CommandeDocument commande)
        {
            if (dgvDvdComListeCom.Rows.Count > 0 && commande != null)
            {
                // Remplir les champs avec les info de la commande sélectionnée
                txbDvdComNbCommande.Text = commande.Id.ToString();
                dtpDvdComDateCommande.Value = commande.DateCommande;
                txbDvdComMontant.Text = commande.Montant.ToString("F2");
                txbDvdComNbExemplaires.Text = commande.NbExemplaire.ToString();

                // nettoyer les éléments existants dans le ComboBox
                cbxDvdComEtat.Items.Clear();
                // récupérer tous les suivis
                var suivis = controller.GetAllSuivis();
                // Ajouter les suivis dans le ComboBox
                var suivisItems = suivis.Select(s => new Suivi(s.Id, s.Etat)).ToArray();
                cbxDvdComEtat.Items.AddRange(suivisItems);
                // définir pair clé/valeur
                cbxDvdComEtat.DisplayMember = "Etat";
                cbxDvdComEtat.ValueMember = "Id";
                // trouver l'état de la commande + sélectionner dans le ComboBox
                var etatCommande = Array.Find(suivisItems, s => s.Id == commande.IdSuivi);
                if (etatCommande != null)
                {
                    cbxDvdComEtat.SelectedItem = etatCommande;
                }

            }

            else
            {
                ViderCmdDvdInfos();
            }
        }
        /// <summary>
        /// Affiche ou non le label sur les opérations CRUD
        /// </summary>
        /// <param name="mode">Le mode d'opération à effectuer</param>
        /// <param name="cacher">Booléen à passer</param>
        private void LabelCrudTitreDvd(string mode, bool cacher)
        {
            // Visibilité du texte
            lblCrudTitreDvd.Visible = cacher;

            switch (mode)
            {
                case Ajout:
                    lblCrudTitreDvd.Text = "Êtes-vous sûr de réaliser cet ajout ?";
                    break;
                case Suppression:
                    lblCrudTitreDvd.Text = "Êtes-vous sûr de réaliser cette suppression ?";
                    break;
                case Modification:
                    lblCrudTitreDvd.Text = "Êtes-vous sûr de réaliser cette modification ?";
                    break;
                default:
                    lblCrudTitreDvd.Text = "";
                    break;
            }
        }
        /// <summary>
        /// Cache les boutons de validation et gère aussi l'affichage
        /// </summary>
        private void CacherValiderDvd()
        {
            LabelCrudTitreDvd(null, false);
            VisibleGroupBoxCommandeDvd();
            DesactiverDataGridView(dgvDvdComListe, false);
            DesactiverDataGridView(dgvDvdComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, false);
        }


        /// <summary>
        /// Evenement d'entrée dans le tab (ouverture de l'onglet)
        /// </summary>
        private void TabCmdDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxCmdDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxCmdDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxCmdDvdRayons);
            RemplirCmdDvdListeComplete();
            VisibleGroupBoxCommandeDvd();
        }
        /// <summary>
        /// Événement déclenché lorsque la sélection du ComboBox des genres change.
        /// Réinitialise les champs de recherche, filtre les DVD par genre et met à jour la liste affichée.
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void cbxCmdDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdDvdGenres.SelectedIndex >= 0)
            {
                txbCmdDvdTitreRecherche.Text = "";
                txbCmdDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxCmdDvdGenres.SelectedItem;
                List<Dvd> dvds = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirCmdDvdListe(dvds);
                cbxCmdDvdPublics.SelectedIndex = -1;
                cbxCmdDvdRayons.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Événement déclenché lorsque la sélection du ComboBox des publics change.
        /// Réinitialise les champs de recherche, filtre les DVD par public et met à jour la liste affichée.
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void cbxCmdDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdDvdPublics.SelectedIndex >= 0)
            {
                txbCmdDvdTitreRecherche.Text = "";
                txbCmdDvdNumRecherche.Text = "";
                Public publics = (Public)cbxCmdDvdPublics.SelectedItem;
                List<Dvd> dvds = lesDvd.FindAll(x => x.Public.Equals(publics.Libelle)); // Correction ici
                RemplirCmdDvdListe(dvds);
                cbxCmdDvdRayons.SelectedIndex = -1;
                cbxCmdDvdGenres.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Événement déclenché lorsque la sélection du ComboBox des rayons change.
        /// Réinitialise les champs de recherche, filtre les DVD par rayon et met à jour la liste affichée.
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void cbxCmdDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdDvdRayons.SelectedIndex >= 0)
            {
                txbCmdDvdTitreRecherche.Text = "";
                txbCmdDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxCmdDvdRayons.SelectedItem;
                List<Dvd> dvds = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle)); // Correction ici
                RemplirCmdDvdListe(dvds);
                cbxCmdDvdGenres.SelectedIndex = -1;
                cbxCmdDvdPublics.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du premier combobox
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnCmdDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirCmdDvdListeComplete();

        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du deuxième combobox
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnCmdDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirCmdDvdListeComplete();

        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du troisème combobox
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnCmdDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirCmdDvdListeComplete();

        }
        /// <summary>
        /// Recherche d'un dvd via son titre dès que l'on commence à écrire dans la textbox
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void txbCmdDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbCmdDvdTitreRecherche.Text.Equals(""))
            {
                cbxCmdDvdGenres.SelectedIndex = -1;
                cbxCmdDvdRayons.SelectedIndex = -1;
                cbxCmdDvdPublics.SelectedIndex = -1;
                txbCmdDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbCmdDvdTitreRecherche.Text.ToLower()));
                RemplirCmdDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxCmdDvdGenres.SelectedIndex < 0 && cbxCmdDvdPublics.SelectedIndex < 0 && cbxCmdDvdRayons.SelectedIndex < 0
                    && txbCmdDvdNumRecherche.Text.Equals(""))
                {
                    RemplirCmdDvdListeComplete();
                }
            }
        }
        /// <summary>
        /// Recherche d'un dvd via son numéro dès que l'on clique sur le bouton 
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnCmdDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCmdDvdNumRecherche.Text.Equals(""))
            {
                txbCmdDvdTitreRecherche.Text = "";
                cbxCmdDvdGenres.SelectedIndex = -1;
                cbxCmdDvdRayons.SelectedIndex = -1;
                cbxCmdDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbCmdDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirCmdDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                    RemplirCmdDvdListeComplete();
                }
            }
            else
            {
                RemplirCmdDvdListeComplete();
            }
        }
        /// <summary>
        /// Première datagridview qui affiche la liste des dvd dans la bdd
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void dgvDvdComListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdComListe.CurrentRow != null)
            {
                // Récupère le dvd sélectionné
                Dvd dvdSelectionne = (Dvd)dgvDvdComListe.CurrentRow.DataBoundItem;

                // Utilise la méthode du contrôleur pour récupérer les commandes associées au dvd sélectionné
                List<CommandeDocument> commandesAssociees = controller.GetCommandesLivres(dvdSelectionne.Id);
                lesCommandesDocument = controller.GetCommandesLivres(dvdSelectionne.Id);
                // Remplir la DataGridView avec les commandes récupérées
                RemplirCmdDvdCommandes(commandesAssociees);
                txbComNumDvd.Text = dvdSelectionne.Id.ToString();
            }
        }
        /// <summary>
        /// Evenement de sélection sur la deuxième datagridview, qui affiche les commandes pour un dvd sélectionné dans la première dgv
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void dgvDvdComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdComListeCom.CurrentRow != null)
            {
                // Récupérer la commande sélectionnée dans la DataGridView
                var commande = (CommandeDocument)dgvDvdComListeCom.CurrentRow.DataBoundItem;

                AfficheCommandeDvdInfo(commande);
            }
        }
        /// <summary>
        /// Tri sur les colonnes de la datagridview
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void dgvDvdComListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideCmdDvdZones();
            string titreColonne = dgvDvdComListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirCmdDvdListe(sortedList);
        }
        /// <summary>
        /// Tri sur les colonnes de la datagridview
        /// </summary>
        private void dgvDvdComListeCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ViderCmdLivresInfos();
                string titreColonne = dgvDvdComListeCom.Columns[e.ColumnIndex].HeaderText;
                List<CommandeDocument> sortedList;
                switch (titreColonne)
                {
                    case "Id":
                        sortedList = lesCommandesDocument.OrderBy(o => o.Id).ToList();
                        break;
                    case "DateCommande":
                        sortedList = lesCommandesDocument.OrderBy(o => o.DateCommande).ToList();
                        break;
                    case "Montant":
                        sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                        break;
                    case "NbExemplaire":
                        sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                        break;
                    case "Etat":
                        sortedList = lesCommandesDocument.OrderBy(o => o.Etat).ToList();
                        break;
                    case "IdLivreDvd":
                        sortedList = lesCommandesDocument.OrderBy(o => o.IdLivreDvd).ToList();
                        break;
                    case "IdSuivi":
                        sortedList = lesCommandesDocument.OrderBy(o => o.IdSuivi).ToList();
                        break;
                    default:
                        MessageBox.Show("Colonne inconnue");
                        return;
                }

                RemplirCmdDvdCommandes(sortedList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
        /// <summary>
        ///Evenement au clic sur le bouton Modifier, on passe la variable mode à "modification" et l'on gère l'affichage
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnDvdComModifier_Click(object sender, EventArgs e)
        {
            Mode = Modification;

            if (!VerifierSelection(dgvDvdComListeCom))
            {
                Mode = "";
            }
            else
            {
                LabelCrudTitreDvd(Mode, true);
                VisibleCommandeDvd();
                RendreBoutonsVisiblesOuInvisibles(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, true);
                cbxDvdComEtat.Visible = true;
                DesactiverDataGridView(dgvDvdComListe, true);
                DesactiverDataGridView(dgvDvdComListeCom, true);
                // Vérifier si un élément a été sélectionné dans le ComboBox
                if (cbxDvdComEtat.SelectedItem != null)
                {
                    // Récupérer l'objet Suivi sélectionné
                    Suivi suiviSelectionne = (Suivi)cbxDvdComEtat.SelectedItem;

                    // Récupérer l'ID du suivi sélectionné
                    int idSuivi = suiviSelectionne.Id;
                    GererEtatComboBox(idSuivi, cbxDvdComEtat);
                }
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton Supprimer, on passe la variable mode à "suppression" et l'on gère l'affichage
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnDvdComSupprimer_Click(object sender, EventArgs e)
        {
            Mode = Suppression;
            Suivi suiviSelectionne = (Suivi)cbxDvdComEtat.SelectedItem;
            if (!VerifierSelection(dgvDvdComListeCom))
            {
                Mode = "";
            }
            // Vérifie si l'ID de suivi est 2 (livré)
            if (suiviSelectionne != null && suiviSelectionne.Id == 2)
            {
                // Affiche une boîte de dialogue avec le message d'erreur
                MessageBox.Show("Impossible de supprimer une commande de DVD déjà livrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LabelCrudTitreDvd(Mode, true);
                RendreBoutonsVisiblesOuInvisibles(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, true);
                groupBoxInfoCommandeDvd.Visible = false;
                VisibleCommandeDvd();
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton Ajout, on passe la variable mode à "ajout" et l'on gère l'affichage
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnDvdComAjouter_Click(object sender, EventArgs e)
        {
            Mode = Ajout;
            LabelCrudTitreDvd(Mode, true);
            VisibleCommandeDvd();
            RendreBoutonsVisiblesOuInvisibles(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, true);
            DesactiverDataGridView(dgvDvdComListe, true);
            DesactiverDataGridView(dgvDvdComListeCom, true);
            ViderCmdDvdInfos();
        }
        /// <summary>
        /// Evenement au clic sur le bouton Valider. La logique CRUD est présente ici via un switch
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnDvdComValider_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération des données communes
                DateTime dateCommande = dtpDvdComDateCommande.Value;
                double montant;
                if (!double.TryParse(txbDvdComMontant.Text, out montant))
                {
                    MessageBox.Show("Montant invalide.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int nbExemplaires;
                if (!int.TryParse(txbDvdComNbExemplaires.Text, out nbExemplaires))
                {
                    MessageBox.Show("Nombre d'exemplaires invalide.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string numDvd = txbComNumDvd.Text;
                if (string.IsNullOrEmpty(numDvd))
                {
                    MessageBox.Show("Numéro de Dvd manquant.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CommandeDocument commandeDocument = null;

                // Vérifie le mode sélectionné et crée l'objet `CommandeDocument`
                switch (Mode)
                {
                    case Ajout:

                        // Valeurs spécifiques à l'ajout
                        commandeDocument = new CommandeDocument(null, dateCommande, montant, nbExemplaires, numDvd, 1, "En Cours");

                        // Affichage dans le MessageBox
                        ExecuterOperation(() => controller.CreerCommandeDocument(commandeDocument), "Commande ajoutée avec succès.", "Erreur lors de l'ajout.");

                        List<CommandeDocument> commandesAssociees = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = commandesAssociees;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdDvdCommandes(commandesAssociees);

                        ViderCmdDvdInfos();
                        CacherValiderDvd();
                        break;

                    case Suppression:
                        // Vérification de la sélection dans le ComboBox
                        if (cbxDvdComEtat.SelectedItem == null)
                        {
                            MessageBox.Show("Veuillez sélectionner un état de suivi pour la suppression.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        Suivi suiviSelectionneSupp = (Suivi)cbxDvdComEtat.SelectedItem;
                        int idSuiviSupp = suiviSelectionneSupp.Id;
                        string etatSuiviSupp = suiviSelectionneSupp.Etat;

                        int idCommandeSupp;
                        if (!int.TryParse(txbDvdComNbCommande.Text, out idCommandeSupp))
                        {
                            MessageBox.Show(IdCommandeInvalide, Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        commandeDocument = new CommandeDocument(idCommandeSupp, dateCommande, montant, nbExemplaires, numDvd, idSuiviSupp, etatSuiviSupp);
                        ExecuterOperation(() => controller.SupprimerCommandeDocument(commandeDocument), "Commande supprimée avec succès.", "Erreur lors de la suppression.");

                        List<CommandeDocument> commandesAssocieesSupp = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = commandesAssocieesSupp;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdDvdCommandes(commandesAssocieesSupp);

                        ViderCmdDvdInfos();
                        CacherValiderDvd();
                        groupBoxInfoCommandeDvd.Visible = true;
                        break;

                    case Modification:
                        // Vérification de la sélection dans le ComboBox
                        if (cbxDvdComEtat.SelectedItem == null)
                        {
                            MessageBox.Show("Veuillez sélectionner un état de suivi pour la modification.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        Suivi suiviSelectionneModif = (Suivi)cbxDvdComEtat.SelectedItem;
                        int idSuiviModif = suiviSelectionneModif.Id;
                        string etatSuiviModif = suiviSelectionneModif.Etat;

                        int idCommandeModif;
                        if (!int.TryParse(txbDvdComNbCommande.Text, out idCommandeModif))
                        {
                            MessageBox.Show(IdCommandeInvalide, Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        commandeDocument = new CommandeDocument(idCommandeModif, dateCommande, montant, nbExemplaires, numDvd, idSuiviModif, etatSuiviModif);
                        ExecuterOperation(() => controller.ModifierCommandeDocument(commandeDocument), "Commande modifiée avec succès.", "Erreur lors de la modification.");

                        List<CommandeDocument> commandesAssocieesModif = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = controller.GetCommandesLivres(commandeDocument.IdLivreDvd);
                        lesCommandesDocument = commandesAssocieesModif;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdDvdCommandes(commandesAssocieesModif);

                        ViderCmdDvdInfos();
                        CacherValiderDvd();
                        break;

                    default:
                        MessageBox.Show("Veuillez sélectionner une opération.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton annuler. On gère principalement l'affichage.
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void btnDvdComAnnuler_Click(object sender, EventArgs e)
        {
            LabelCrudTitreDvd(null, false);
            VisibleGroupBoxCommandeDvd();
            DesactiverDataGridView(dgvDvdComListe, false);
            DesactiverDataGridView(dgvDvdComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, false);
            groupBoxInfoCommandeDvd.Visible = true;
        }
        #endregion

        #region Onglet Abonnement
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues">liste de revues</param>
        private void RemplirCmdRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvCmdRevuesListe.DataSource = bdgRevuesListe;
            dgvCmdRevuesListe.Columns[IdRayonColumn].Visible = false;
            dgvCmdRevuesListe.Columns[IdGenreColumnName].Visible = false;
            dgvCmdRevuesListe.Columns[idPublic].Visible = false;
            dgvCmdRevuesListe.Columns[UneImage].Visible = false;
            dgvCmdRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCmdRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvCmdRevuesListe.Columns[Titre].DisplayIndex = 1;
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideCmdRevuesZones()
        {
            cbxCmdRevuesGenres.SelectedIndex = -1;
            cbxCmdRevuesRayon.SelectedIndex = -1;
            cbxCmdRevuesPublic.SelectedIndex = -1;
            txbCmdRevuesNumRecherche.Text = "";
            txbCmdRevuesTitreRecherche.Text = "";
        }
        /// <summary>
        /// Remplit la dgv de commande d'abonnement et vide les textbox etc...
        /// </summary>
        private void RemplirCmdRevuesListeComplete()
        {
            RemplirCmdRevuesListe(lesRevues);
            VideCmdRevuesZones();
        }
        /// <summary>
        /// Remplit le DataGridView avec les commandes associées à l'abonnement sélectionné.
        /// </summary>
        /// <param name="commandes">Liste des commandes.</param>
        private void RemplirCmdAbonnementCommandes(List<Abonnement> commandes)
        {
            BindingSource bdgCommandes = new BindingSource();
            bdgCommandes.DataSource = commandes;
            dgvRevueComListeCom.DataSource = bdgCommandes;

            // Réorganiser les colonnes en fonction de leur index d'affichage
            dgvRevueComListeCom.Columns["Id"].DisplayIndex = 0;
            dgvRevueComListeCom.Columns[DateCommandeColumnName].DisplayIndex = 1;
            dgvRevueComListeCom.Columns[Montant].DisplayIndex = 2;
            dgvRevueComListeCom.Columns["DateFinAbonnement"].DisplayIndex = 3;

            dgvRevueComListeCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void ViderAbonnementDvdInfos()
        {
            txbAbonnementComNbCommande.Text = "";
            dtpAbonnementComDateCommande.Value = DateTime.Now;
            txbAbonnementComMontant.Text = "";
            dtpFinAbonnementDateCommande.Value = DateTime.Now;
            txbComNumRevue.ReadOnly = true;
        }
        /// <summary>
        /// Gére l'affichage des infos d'une commande dans les datagridview
        /// </summary>
        /// <param name="commande">Une commande d'abonnements</param>
        private void AfficheCommandeAbonnementInfo(Abonnement commande)
        {
            if (dgvRevueComListeCom.Rows.Count > 0 && commande != null)
            {
                // Remplir les champs avec les info de la commande sélectionnée
                txbAbonnementComNbCommande.Text = commande.Id.ToString();
                dtpAbonnementComDateCommande.Value = commande.DateCommande;
                txbAbonnementComMontant.Text = commande.Montant.ToString("F2");
                dtpFinAbonnementDateCommande.Value = (DateTime)commande.DateFinAbonnement;
            }

            else
            {
                ViderAbonnementDvdInfos();
            }
        }
        /// <summary>
        /// Affiche ou non le label sur les opérations CRUD
        /// </summary>
        /// <param name="mode">Le mode d'opération à effectuer</param>
        /// <param name="cacher">Booléen à passer</param>
        private void LabelCrudTitreAbonnement(string mode, bool cacher)
        {
            // Visibilité du texte
            lblCrudTitreAbonnement.Visible = cacher;

            switch (mode)
            {
                case Ajout:
                    lblCrudTitreAbonnement.Text = "Êtes-vous sûr de réaliser cet ajout ?";
                    break;
                case Suppression:
                    lblCrudTitreAbonnement.Text = "Êtes-vous sûr de réaliser cette suppression ?";
                    break;
                case Modification:
                    lblCrudTitreAbonnement.Text = "Êtes-vous sûr de réaliser cette modification ?";
                    break;
                default:
                    lblCrudTitreAbonnement.Text = "";
                    break;
            }
        }
        /// <summary>
        /// Gérer la visibilité du groupBox et des boutons valider et annuler
        /// </summary>
        private void VisibleCommandeAbonnement()
        {
            label89.Visible = false;
            txbAbonnementComNbCommande.Visible = false;
            txbAbonnementComMontant.ReadOnly = false;
            dtpAbonnementComDateCommande.Enabled = true;
            dtpFinAbonnementDateCommande.Enabled = true;
            txbComNumRevue.ReadOnly = true;
            btnAbonnementComValider.Visible = true;
            btnAbonnementComAnnuler.Visible = true;
        }
        /// <summary>
        /// Gérer la visibilité du groupBox
        /// </summary>
        private void VisibleGroupBoxCommandeAbonnement()
        {
            label89.Visible = true;
            txbAbonnementComNbCommande.Visible = true;
            txbAbonnementComNbCommande.ReadOnly = true;
            txbAbonnementComMontant.ReadOnly = true;
            dtpAbonnementComDateCommande.Enabled = false;
            dtpFinAbonnementDateCommande.Enabled = false;
            txbComNumRevue.ReadOnly = true;
            btnAbonnementComValider.Visible = false;
            btnAbonnementComAnnuler.Visible = false;


        }
        /// <summary>
        /// Cache les boutons de validation et gère aussi l'affichage
        /// </summary>
        private void CacherValiderAbonnement()
        {
            LabelCrudTitreAbonnement(null, false);
            VisibleGroupBoxCommandeAbonnement();
            DesactiverDataGridView(dgvCmdRevuesListe, false);
            DesactiverDataGridView(dgvRevueComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnAbonnementComModifier, btnAbonnementComSupprimer, btnAbonnementComAjouter, false);
        }

        /// <summary>
        /// Evenement d'entrée dans le tab (ouverture de l'onglet)
        /// </summary>
        private void tabCmdRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxCmdRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxCmdRevuesPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxCmdRevuesRayon);
            RemplirCmdRevuesListeComplete();
            VisibleGroupBoxCommandeAbonnement();

        }
        /// <summary>
        /// Recherche d'une revue via son titre dès que l'on commence à écrire dans la textbox
        /// </summary>
        private void txbCmdRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbCmdRevuesTitreRecherche.Text.Equals(""))
            {
                cbxCmdRevuesGenres.SelectedIndex = -1;
                cbxCmdRevuesRayon.SelectedIndex = -1;
                cbxCmdRevuesPublic.SelectedIndex = -1;
                txbCmdRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbCmdRevuesTitreRecherche.Text.ToLower()));
                RemplirCmdRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxCmdRevuesGenres.SelectedIndex < 0 && cbxCmdRevuesPublic.SelectedIndex < 0 && cbxCmdRevuesRayon.SelectedIndex < 0
                    && txbCmdRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirCmdRevuesListeComplete();
                }
            }
        }
        /// <summary>
        /// Recherche d'une revue via son numéro dès que l'on clique sur le bouton 
        /// </summary>
        private void btnCmdRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCmdRevuesNumRecherche.Text.Equals(""))
            {
                txbCmdRevuesTitreRecherche.Text = "";
                cbxCmdRevuesGenres.SelectedIndex = -1;
                cbxCmdRevuesRayon.SelectedIndex = -1;
                cbxCmdRevuesPublic.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbCmdRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirCmdRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show(NumeroIntrouvable);
                    RemplirCmdRevuesListeComplete();
                }
            }
            else
            {
                RemplirCmdRevuesListeComplete();
            }
        }
        /// <summary>
        /// Remplit la première dgv en fonction du choix fait dans le premier combobox (tri)
        /// </summary>
        private void cbxCmdRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdRevuesGenres.SelectedIndex >= 0)
            {
                txbCmdRevuesTitreRecherche.Text = "";
                txbCmdRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxCmdRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirCmdRevuesListe(revues);
                cbxCmdRevuesRayon.SelectedIndex = -1;
                cbxCmdRevuesPublic.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Remplit la première dgv en fonction du choix fait dans le deuxième combobox (tri)
        /// </summary>
        private void cbxCmdRevuesPublic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdRevuesPublic.SelectedIndex >= 0)
            {
                txbCmdRevuesTitreRecherche.Text = "";
                txbCmdRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxCmdRevuesPublic.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirCmdRevuesListe(revues);
                cbxCmdRevuesRayon.SelectedIndex = -1;
                cbxCmdRevuesGenres.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Remplit la première dgv en fonction du choix fait dans le troisème combobox (tri)
        /// </summary>
        private void cbxCmdRevuesRayon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdRevuesRayon.SelectedIndex >= 0)
            {
                txbCmdRevuesTitreRecherche.Text = "";
                txbCmdRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxCmdRevuesRayon.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirCmdRevuesListe(revues);
                cbxCmdRevuesGenres.SelectedIndex = -1;
                cbxCmdRevuesPublic.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du premier combobox
        /// </summary>
        private void btnCmdRevuesAnnulGenre_Click(object sender, EventArgs e)
        {
            RemplirCmdRevuesListeComplete();
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du deuxième combobox
        /// </summary>
        private void btnCmdRevuesAnnulPublic_Click(object sender, EventArgs e)
        {
            RemplirCmdRevuesListeComplete();
        }
        /// <summary>
        /// Bouton sous forme de croix qui vide les champs du troisième combobox
        /// </summary>
        private void btnCmdRevuesAnnulRayon_Click(object sender, EventArgs e)
        {
            RemplirCmdRevuesListeComplete();
        }
        /// <summary>
        /// Première datagridview qui affiche la liste des abonnements présents dans la bdd
        /// </summary>
        private void dgvCmdRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdRevuesListe.CurrentRow != null)
            {
                // Récupère le dvd sélectionné
                Revue revueSelectionne = (Revue)dgvCmdRevuesListe.CurrentRow.DataBoundItem;

                // Utilise la méthode du contrôleur pour récupérer les commandes associées au dvd sélectionné
                List<Abonnement> commandesAssociees = controller.GetCommandesAbonnement(revueSelectionne.Id);
                lesAbonnements = controller.GetCommandesAbonnement(revueSelectionne.Id);
                // Remplir la DataGridView avec les commandes récupérées
                RemplirCmdAbonnementCommandes(commandesAssociees);
                txbComNumRevue.Text = revueSelectionne.Id.ToString();

            }
        }
        /// <summary>
        /// Evenement de sélection sur la deuxième datagridview, qui affiche les commandes pour un abonnement sélectionné dans la première dgv
        /// </summary>
        private void dgvRevueComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevueComListeCom.CurrentRow != null)
            {
                // Récupérer la commande sélectionnée dans la DataGridView
                var commande = (Abonnement)dgvRevueComListeCom.CurrentRow.DataBoundItem;
                AfficheCommandeAbonnementInfo(commande);
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton Ajout, on passe la variable mode à "ajout" et l'on gère l'affichage
        /// </summary>
        private void btnAbonnementComAjouter_Click(object sender, EventArgs e)
        {
            Mode = Ajout;
            LabelCrudTitreAbonnement(Mode, true);
            VisibleCommandeAbonnement();
            RendreBoutonsVisiblesOuInvisibles(btnAbonnementComModifier, btnAbonnementComSupprimer, btnAbonnementComAjouter, true);
            DesactiverDataGridView(dgvCmdRevuesListe, true);
            DesactiverDataGridView(dgvRevueComListeCom, true);
            ViderAbonnementDvdInfos();
        }
        /// <summary>
        ///Evenement au clic sur le bouton Modifier, on passe la variable mode à "modification" et l'on gère l'affichage
        /// </summary>
        private void btnAbonnementComModifier_Click(object sender, EventArgs e)
        {
            Mode = Modification;

            if (!VerifierSelection(dgvRevueComListeCom))
            {
                Mode = "";
            }
            else
            {
                LabelCrudTitreAbonnement(Mode, true);
                VisibleCommandeAbonnement();
                RendreBoutonsVisiblesOuInvisibles(btnAbonnementComModifier, btnAbonnementComSupprimer, btnAbonnementComAjouter, true);
                DesactiverDataGridView(dgvCmdRevuesListe, true);
                DesactiverDataGridView(dgvRevueComListeCom, true);
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton Supprimer, on passe la variable mode à "suppression" et l'on gère l'affichage
        /// </summary>
        private void btnAbonnementComSupprimer_Click(object sender, EventArgs e)
        {
            Mode = Suppression;
            if (!VerifierSelection(dgvRevueComListeCom))
            {
                Mode = "";
            }
            else
            {
                LabelCrudTitreAbonnement(Mode, true);
                groupBoxCommandeAbonnement.Visible = false;
                RendreBoutonsVisiblesOuInvisibles(btnAbonnementComModifier, btnAbonnementComSupprimer, btnAbonnementComAjouter, true);
                VisibleCommandeAbonnement();
            }
        }
        /// <summary>
        /// Evenement au clic sur le bouton annuler. On gère principalement l'affichage.
        /// </summary>
        private void btnAbonnementComAnnuler_Click(object sender, EventArgs e)
        {
            LabelCrudTitreAbonnement(null, false);
            VisibleGroupBoxCommandeAbonnement();
            DesactiverDataGridView(dgvCmdRevuesListe, false);
            DesactiverDataGridView(dgvRevueComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnAbonnementComModifier, btnAbonnementComSupprimer, btnAbonnementComAjouter, false);
            groupBoxCommandeAbonnement.Visible = true;

        }
        /// <summary>
        /// Evenement au clic sur le bouton Valider. La logique CRUD est présente ici via un switch
        /// </summary>
        private void btnAbonnementComValider_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération des données communes
                DateTime dateCommandeAbonnement = dtpAbonnementComDateCommande.Value;
                DateTime dateFinAbonnement = dtpFinAbonnementDateCommande.Value;
                double montant;
                if (!double.TryParse(txbAbonnementComMontant.Text, out montant))
                {
                    MessageBox.Show("Attention ! Montant invalide ou manquant.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string numRevue = txbComNumRevue.Text;
                if (string.IsNullOrEmpty(numRevue))
                {
                    MessageBox.Show("Numéro de revue manquant.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Abonnement commandeRevue = null;

                // Vérifie le mode sélectionné et crée l'objet `Abonnement`
                switch (Mode)
                {
                    case Ajout:
                        // Valeurs spécifiques à l'ajout
                        commandeRevue = new Abonnement(null, dateCommandeAbonnement, montant, dateFinAbonnement, numRevue);
                        // Affichage dans le MessageBox
                        ExecuterOperation(() => controller.CreerCommandeAbonnement(commandeRevue), "Commande ajoutée avec succès.", "Erreur lors de l'ajout.");

                        List<Abonnement> commandesAssociees = controller.GetCommandesAbonnement(commandeRevue.IdRevue);
                        lesAbonnements = controller.GetCommandesAbonnement(commandeRevue.IdRevue);
                        lesAbonnements = commandesAssociees;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdAbonnementCommandes(commandesAssociees);

                        ViderAbonnementDvdInfos();
                        CacherValiderAbonnement();
                        break;

                    case Suppression:
                        int idAbonnement;
                        if (!int.TryParse(txbAbonnementComNbCommande.Text, out idAbonnement))
                        {
                            MessageBox.Show("Identifiant de commande invalide ou manquant.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        commandeRevue = new Abonnement(idAbonnement, dateCommandeAbonnement, montant, dateFinAbonnement, numRevue);

                        var exemplaires = controller.GetExemplairesRevue(commandeRevue.IdRevue);
                        bool exemplaireRattache = exemplaires.Exists(exemplaire =>Abonnement.ParutionDansAbonnement(commandeRevue.DateCommande, commandeRevue.DateFinAbonnement.Value, exemplaire.DateAchat));

                        if (exemplaireRattache)
                        {
                            MessageBox.Show("Impossible de supprimer l'abonnement. Un ou plusieurs exemplaires sont rattachés.");
                            return;
                        }
                        if (controller.SupprimerCommandeAbonnement(commandeRevue))
                        {
                            MessageBox.Show("L'abonnement a été supprimé avec succès.");
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression de l'abonnement.");
                        }

                        List<Abonnement> commandesAssocieesSupp = controller.GetCommandesAbonnement(commandeRevue.IdRevue);
                        lesAbonnements = controller.GetCommandesAbonnement(commandeRevue.IdRevue);
                        lesAbonnements = commandesAssocieesSupp;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdAbonnementCommandes(commandesAssocieesSupp);

                        ViderAbonnementDvdInfos();
                        CacherValiderAbonnement();
                        groupBoxCommandeAbonnement.Visible = true;
                        break;
                    case Modification:

                        int idAbonnementModif;
                        if (!int.TryParse(txbAbonnementComNbCommande.Text, out idAbonnementModif))
                        {
                            MessageBox.Show("Identifiant d'abonnement invalide ou manquant.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        commandeRevue = new Abonnement(idAbonnementModif, dateCommandeAbonnement, montant, dateFinAbonnement, numRevue);
                        ExecuterOperation(() => controller.ModifierCommandeAbonnement(commandeRevue), "Abonnement modifiée avec succès.", "Erreur lors de la modification.");

                        List<Abonnement> commandesAssocieesModifie= controller.GetCommandesAbonnement(commandeRevue.IdRevue);
                        lesAbonnements = controller.GetCommandesAbonnement(commandeRevue.IdRevue);
                        lesAbonnements = commandesAssocieesModifie;
                        // Mise à jour de la DataGridView pour afficher les commandes associées
                        RemplirCmdAbonnementCommandes(commandesAssocieesModifie);

                        ViderAbonnementDvdInfos();
                        CacherValiderAbonnement();
                        break;
                    default:
                        MessageBox.Show("Veuillez sélectionner une opération.", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", Erreur, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
        /// <summary>
        /// Tri sur les colonnes de la datagridview
        /// </summary>
        private void dgvCmdRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ViderCmdLivresInfos();
                string titreColonne = dgvCmdRevuesListe.Columns[e.ColumnIndex].HeaderText;
                List<Revue> sortedList;
                switch (titreColonne)
                {
                    case "Id":
                        sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                        break;
                    case "Titre":
                        sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                        break;
                    case "Periodicite":
                        sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                        break;
                    case "DelaiMiseADispo":
                        sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                        break;
                    case "Genre":
                        sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                        break;
                    case "Public":
                        sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                        break;
                    case "Rayon":
                        sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                        break;
                    default:
                        MessageBox.Show("Colonne inconnue");
                        return;
                }

                RemplirCmdRevuesListe(sortedList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
        /// <summary>
        /// Tri sur les colonnes de la datagridview
        /// </summary>
        private void dgvRevueComListeCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ViderCmdLivresInfos();
                string titreColonne = dgvRevueComListeCom.Columns[e.ColumnIndex].HeaderText;
                List<Abonnement> sortedList;
                switch (titreColonne)
                {
                    case "Id":
                        sortedList = lesAbonnements.OrderBy(o => o.Id).ToList();
                        break;
                    case "DateCommande":
                        sortedList = lesAbonnements.OrderBy(o => o.DateCommande).ToList();
                        break;
                    case "Montant":
                        sortedList = lesAbonnements.OrderBy(o => o.Montant).ToList();
                        break;
                    case "DateFinAbonnement":
                        sortedList = lesAbonnements.OrderBy(o => o.DateFinAbonnement).ToList();
                        break;
                    case "IdRevue":
                        sortedList = lesAbonnements.OrderBy(o => o.IdRevue).ToList();
                        break;
                    default:
                        MessageBox.Show("Colonne inconnue");
                        return;
                }
                RemplirCmdAbonnementCommandes(sortedList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        #endregion


    }
}

