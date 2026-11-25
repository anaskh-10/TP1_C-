
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace TP1
{
    static class UI
    {
        public static Label Lbl(Control p, string t, int x, int y, int w = 120, int h = 20, float size = 9f, bool bold = false)
        {
            var l = new Label { Text = t, Left = x, Top = y, Width = w, Height = h, Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular) };
            p.Controls.Add(l); return l;
        }
        public static TextBox Txt(Control p, string name, int x, int y, int w = 120)
        {
            var t = new TextBox { Name = name, Left = x, Top = y, Width = w };
            p.Controls.Add(t); return t;
        }
        public static Button Btn(Control p, string name, string t, int x, int y, int w = 110, int h = 30)
        {
            var b = new Button { Name = name, Text = t, Left = x, Top = y, Width = w, Height = h };
            p.Controls.Add(b); return b;
        }
        public static GroupBox Grp(Control p, string t, int x, int y, int w, int h)
        {
            var g = new GroupBox { Text = t, Left = x, Top = y, Width = w, Height = h };
            p.Controls.Add(g); return g;
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new frmAccueil());
        }
    }

   
    class frmAccueil : Form
    {
        public frmAccueil()
        {
            Text = "TP 1"; ClientSize = new Size(400, 330);
            StartPosition = FormStartPosition.CenterScreen;
            var bF = UI.Btn(this, "btnFact", "Calcul Factoriel", 80, 30, 240, 55);
            var bT = UI.Btn(this, "btnTab", "Les tableaux", 80, 100, 240, 55);
            var bJ = UI.Btn(this, "btnJeu", "Jouer", 80, 170, 240, 55);
            var bQ = UI.Btn(this, "btnQuit", "Quitter", 80, 240, 240, 55);
            var bS = UI.Btn(this, "btnScore", "Calcul de score", 270, 295, 120, 28);

            bF.Click += (s, e) => new frmFactoriel().ShowDialog();
            bT.Click += (s, e) => new frmTableaux().ShowDialog();
            bJ.Click += (s, e) => new frmJeux().ShowDialog();
            bS.Click += (s, e) => new frmScore().ShowDialog();
            bQ.Click += (s, e) => Application.Exit();
        }
    }

   
    class frmFactoriel : Form
    {
        TextBox txtN; Label lblRes;

        public frmFactoriel()
        {
            Text = "Calcul factoriel"; ClientSize = new Size(520, 340);
            StartPosition = FormStartPosition.CenterParent;
            var lt = UI.Lbl(this, "Factoriel", 20, 20, 480, 50, 20f, true);
            lt.TextAlign = ContentAlignment.MiddleCenter;
            UI.Lbl(this, "Donner un entier positif:", 30, 100, 300, 28, 12f);
            txtN = UI.Txt(this, "txtN", 30, 135, 200);
            txtN.Font = new Font("Segoe UI", 14f, FontStyle.Bold); txtN.TextAlign = HorizontalAlignment.Center;
            var btnCalc = UI.Btn(this, "btnCalc", "Calculer", 260, 120, 220, 55);
            lblRes = UI.Lbl(this, "", 30, 200, 450, 32, 12f, true);
            lblRes.TextAlign = ContentAlignment.MiddleCenter; lblRes.BorderStyle = BorderStyle.FixedSingle;
            var btnRet = UI.Btn(this, "btnRetour", "Retour", 30, 270, 200, 45);
            var btnQuit = UI.Btn(this, "btnQuit", "Quitter", 280, 270, 200, 45);

            txtN.KeyPress += (s, e) => { if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true; };
            btnCalc.Click += BtnCalc_Click;
            btnRet.Click += (s, e) => Close();
            btnQuit.Click += (s, e) => Application.Exit();
            AcceptButton = btnCalc;
        }

        void BtnCalc_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtN.Text, out int n) || n < 0)
            {
                MessageBox.Show("Il faut saisir un entier positif", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtN.Clear(); txtN.Focus(); return;
            }
            BigInteger f = 1;                       // BigInteger : pas de debordement
            for (int i = 2; i <= n; i++) f *= i;    // structure iterative
            lblRes.Text = "Factoriel ( " + n + " ) = " + f;
        }
    }

  
    class frmTableaux : Form
    {
        const int n = 5;
        float[] T = new float[n];
        int i = 0;
        Label lblRes, lblT, lblMoy, lblEc, lblInf, lblSup;
        TextBox txtn; Button BtnEnreg, BtnCalc;

        public frmTableaux()
        {
            Text = "Les Tableaux"; ClientSize = new Size(560, 330);
            StartPosition = FormStartPosition.CenterParent;
            var g1 = UI.Grp(this, "Enregistrement des Notes:", 15, 10, 530, 110);
            lblRes = UI.Lbl(g1, "Entrez la note de l'élève N°: 1", 15, 30, 230);
            txtn = UI.Txt(g1, "txtn", 250, 27, 60);
            BtnEnreg = UI.Btn(g1, "BtnEnreg", "Enregistrer", 330, 22, 130, 32);
            lblT = UI.Lbl(g1, "|", 150, 70, 300, 24, 11f, true);

            var g2 = UI.Grp(this, "Résultats", 15, 130, 530, 185);
            UI.Lbl(g2, "Moyenne de la classe:", 20, 30, 150);
            UI.Lbl(g2, "L'écart-type:", 20, 65, 150);
            UI.Lbl(g2, "Nombre des Notes < 10:", 20, 100, 160);
            UI.Lbl(g2, "Nombre des Notes >= 10:", 20, 135, 160);
            lblMoy = ResLbl(g2, 190, 28, Color.Black);
            lblEc = ResLbl(g2, 190, 63, Color.Black);
            lblInf = ResLbl(g2, 190, 98, Color.Red);
            lblSup = ResLbl(g2, 190, 133, Color.Green);
            BtnCalc = UI.Btn(g2, "BtnCalc", "Calculer", 340, 40, 130, 35);
            var BtnQuit = UI.Btn(g2, "BtnQuit", "Quitter", 340, 95, 130, 35);

            BtnCalc.Enabled = false;
            BtnEnreg.Click += BtnEnreg_Click;
            BtnCalc.Click += BtnCalc_Click;
            BtnQuit.Click += (s, e) => Close();
            AcceptButton = BtnEnreg;
        }

        static Label ResLbl(Control p, int x, int y, Color c)
        {
            var l = UI.Lbl(p, "", x, y, 100, 26, 10f, true);
            l.BorderStyle = BorderStyle.FixedSingle; l.TextAlign = ContentAlignment.MiddleCenter; l.ForeColor = c;
            return l;
        }

        void BtnEnreg_Click(object sender, EventArgs e)
        {
            if (float.TryParse(txtn.Text, out float val) && val >= 0 && val <= 20)
            {
                T[i] = val; i++;
                lblT.Text = lblT.Text + " " + val + " |";
                txtn.Clear(); txtn.Focus();
                if (i == n)
                {
                    BtnCalc.Enabled = true; BtnEnreg.Enabled = false; txtn.Enabled = false;
                    lblRes.Text = "Toutes les notes sont saisies"; BtnCalc.Focus();
                }
                else lblRes.Text = "Entrez la note de l'élève N°: " + (i + 1);
            }
            else
            {
                MessageBox.Show("La note doit être un nombre entre 0 et 20", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtn.Clear(); txtn.Focus();
            }
        }

        void BtnCalc_Click(object sender, EventArgs e)
        {
            float somme = 0; int inf = 0, sup = 0;
            for (int k = 0; k < n; k++)
            {
                somme += T[k];
                if (T[k] < 10) inf++; else sup++;
            }
            float moy = somme / n;
            float ss = 0;
            for (int k = 0; k < n; k++) ss += (T[k] - moy) * (T[k] - moy);
            double ecart = Math.Sqrt(ss / n);              
            lblMoy.Text = moy.ToString("0.##");
            lblEc.Text = ecart.ToString("0.#####");
            lblInf.Text = inf.ToString();
            lblSup.Text = sup.ToString();
        }
    }

    
    class frmJeux : Form
    {
        List<TextBox> LTxtC = new List<TextBox>();   // numero cache
        List<TextBox> LTxtP = new List<TextBox>();   // proposition
        int nbJeux = 0, ticks = 0; bool animation = false;
        Label LblNbJeux; GroupBox GrB1; System.Windows.Forms.Timer timer1;

        public frmJeux()
        {
            Text = "Jeux"; ClientSize = new Size(560, 430);
            StartPosition = FormStartPosition.CenterParent;
            var t = UI.Lbl(this, "Nombre caché", 0, 10, 560, 50, 22f, true);
            t.Font = new Font("Segoe UI", 22f, FontStyle.Bold | FontStyle.Italic); t.TextAlign = ContentAlignment.MiddleCenter;

            var BtnNouv = UI.Btn(this, "BtnNouv", "Nouvelle partie", 25, 90, 120, 50);
            var g = UI.Grp(this, "Numéro à deviner", 160, 70, 190, 70);
            for (int k = 0; k < 4; k++) LTxtC.Add(Digit(g, 12 + k * 42, 28, true));
            var BtnAff = UI.Btn(this, "BtnAfficher", "Afficher", 400, 70, 130, 32);
            var BtnCac = UI.Btn(this, "BtnCacher", "Cacher", 400, 108, 130, 32);
            LblNbJeux = UI.Lbl(this, "1", 0, 150, 560, 30, 16f, true); LblNbJeux.TextAlign = ContentAlignment.MiddleCenter;

            var BtnRej = UI.Btn(this, "BtnRejouer", "Rejouer", 25, 200, 120, 40);
            GrB1 = UI.Grp(this, "Votre proposition", 160, 185, 190, 70);
            for (int k = 0; k < 4; k++) LTxtP.Add(Digit(GrB1, 12 + k * 42, 28, false));
            var BtnVer = UI.Btn(this, "BtnVerifier", "Vérifier", 400, 200, 130, 40);
            var BtnQ = UI.Btn(this, "BtnQuit", "Quitter", 215, 300, 130, 45);

            timer1 = new System.Windows.Forms.Timer { Interval = 300 };
            timer1.Tick += Timer1_Tick;
            BtnNouv.Click += (s, e) => NouvellePartie();
            BtnAff.Click += (s, e) => { for (int i = 0; i < 4; i++) LTxtC[i].ForeColor = Color.Black; };
            BtnCac.Click += (s, e) => { for (int i = 0; i < 4; i++) LTxtC[i].ForeColor = Color.White; };
            BtnRej.Click += (s, e) => Rejouer();
            BtnVer.Click += BtnVerifier_Click;
            BtnQ.Click += (s, e) => Close();
            FormClosing += (s, e) => timer1.Stop();
            NouvellePartie();
        }

        static TextBox Digit(Control p, int x, int y, bool ro)
        {
            var t = new TextBox
            {
                Left = x,
                Top = y,
                Width = 32,
                MaxLength = 1,
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ReadOnly = ro,
                BackColor = Color.White
            };
            if (!ro) t.KeyPress += (s, e) => { if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true; };
            p.Controls.Add(t); return t;
        }

        void NouvellePartie()
        {
            timer1.Stop(); nbJeux = 0; LblNbJeux.Text = "1"; GrB1.Enabled = true;
            Random random = new Random();
            int n1 = random.Next(0, 10), n2, n3, n4;
            do n2 = random.Next(0, 10); while (n2 == n1);
            do n3 = random.Next(0, 10); while (n3 == n2 || n3 == n1);
            do n4 = random.Next(0, 10); while (n4 == n3 || n4 == n2 || n4 == n1);
            LTxtC[0].Text = n1.ToString(); LTxtC[1].Text = n2.ToString();
            LTxtC[2].Text = n3.ToString(); LTxtC[3].Text = n4.ToString();
            for (int i = 0; i < 4; i++) { LTxtC[i].BackColor = Color.White; LTxtC[i].ForeColor = Color.White; } // cache
            Rejouer();
        }

        void Rejouer()
        {
            for (int i = 0; i < 4; i++) { LTxtP[i].BackColor = Color.White; LTxtP[i].Text = ""; }
            LTxtP[0].Focus();
        }

        void BtnVerifier_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
                if (LTxtP[i].Text == "") { MessageBox.Show("Saisir les 4 chiffres"); return; }

            int correct = 0;
            for (int i = 0; i < 4; i++)
            {
                LTxtP[i].BackColor = Color.White;
                if (LTxtP[i].Text == LTxtC[i].Text)             // bien place -> vert
                {
                    LTxtP[i].BackColor = Color.Lime; correct++;
                }
                else
                {
                    for (int j = 0; j < 4; j++)                   // existe mais mal place -> jaune
                        if (LTxtP[i].Text == LTxtC[j].Text) { LTxtP[i].BackColor = Color.Yellow; break; }
                }
            }

            if (correct == 4)
            {
                for (int i = 0; i < 4; i++) LTxtC[i].ForeColor = Color.Black;
                GrB1.Enabled = false;
                MessageBox.Show("Bravo, vous avez gagné !", "Victoire", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                nbJeux++;
                ticks = 0; timer1.Start();                       // animation d'erreur
                if (nbJeux == 3)
                {
                    GrB1.Enabled = false;
                    for (int i = 0; i < 4; i++) LTxtC[i].ForeColor = Color.Black;
                    MessageBox.Show("Vous avez perdu", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else LblNbJeux.Text = (nbJeux + 1).ToString();
            }
        }

        void Timer1_Tick(object sender, EventArgs e)
        {
            Color c = animation ? Color.Yellow : Color.Red;
            animation = !animation;
            for (int i = 0; i < 4; i++) LTxtC[i].BackColor = c;
            if (++ticks >= 6)
            {
                timer1.Stop();
                for (int i = 0; i < 4; i++) LTxtC[i].BackColor = Color.White;
            }
        }
    }

   // Application 2 : Calcul de score d'un candidat
    class frmScore : Form
    {
        TextBox TxtCin, TxtTel, TxtNom, TxtPrenom, TxtLang, TxtTech, TxtChance, TxtScore;
        Label LblDate, LblHeure;
        RadioButton RdMas, RdFem;
        CheckBox ChkExper, ChkDip, ChkMotiv;
        ComboBox CmbAge;
        Button BtnCalc, BtnQuit, BtnInit, BtnChance, BtnAjout;
        ListBox Lst1, Lst2, Lst3, Lst4;
        System.Timers.Timer timer1;
        Random rnd = new Random();

        public frmScore()
        {
            Text = "Calcul de score"; ClientSize = new Size(640, 660);
            StartPosition = FormStartPosition.CenterParent;
            UI.Lbl(this, "Calcul de Score", 15, 10, 300, 40, 18f, true);
            LblDate = UI.Lbl(this, "", 380, 12, 120, 20);
            LblHeure = UI.Lbl(this, "", 510, 12, 120, 20);

            var gi = UI.Grp(this, "Informations", 15, 60, 610, 80);
            UI.Lbl(gi, "CIN", 15, 25, 40); TxtCin = UI.Txt(gi, "TxtCin", 60, 22, 200); TxtCin.MaxLength = 8;
            UI.Lbl(gi, "Tél", 15, 52, 40); TxtTel = UI.Txt(gi, "TxtTel", 60, 49, 200);
            UI.Lbl(gi, "Nom", 310, 25, 50); TxtNom = UI.Txt(gi, "TxtNom", 370, 22, 220);
            UI.Lbl(gi, "Prénom", 310, 52, 55); TxtPrenom = UI.Txt(gi, "TxtPrenom", 370, 49, 220);

            var gn = UI.Grp(this, "Notes", 15, 150, 300, 80);
            UI.Lbl(gn, "Langue", 15, 25, 70); TxtLang = UI.Txt(gn, "TxtLang", 100, 22, 80); TxtLang.Text = "0";
            UI.Lbl(gn, "Technique", 15, 52, 80); TxtTech = UI.Txt(gn, "TxtTech", 100, 49, 80); TxtTech.Text = "0";

            var gs = UI.Grp(this, "Sexe", 325, 150, 300, 80);
            RdMas = new RadioButton { Text = "Masculin", Left = 20, Top = 22, Checked = true, Width = 100 }; gs.Controls.Add(RdMas);
            RdFem = new RadioButton { Text = "Féminin", Left = 20, Top = 48, Width = 100 }; gs.Controls.Add(RdFem);

            var gb = UI.Grp(this, "Bonus", 15, 240, 610, 55);
            ChkExper = new CheckBox { Text = "Expérience Professionnelle", Left = 15, Top = 22, Width = 190 }; gb.Controls.Add(ChkExper);
            ChkMotiv = new CheckBox { Text = "Motivations", Left = 250, Top = 22, Width = 120 }; gb.Controls.Add(ChkMotiv);
            ChkDip = new CheckBox { Text = "Diplômes", Left = 430, Top = 22, Width = 120 }; gb.Controls.Add(ChkDip);

            UI.Lbl(this, "Age", 30, 312, 40);
            CmbAge = new ComboBox { Left = 75, Top = 308, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            CmbAge.Items.AddRange(new object[] { "20-30", "30-40", "40 et plus" }); Controls.Add(CmbAge);
            UI.Lbl(this, "Valeur de Chance", 280, 312, 110);
            TxtChance = UI.Txt(this, "TxtChance", 395, 308, 80); TxtChance.ReadOnly = true; TxtChance.Text = "0";
            BtnChance = UI.Btn(this, "BtnChance", "Générer", 490, 305, 90, 28);

            var gx = UI.Grp(this, "", 15, 345, 610, 55);
            BtnInit = UI.Btn(gx, "BtnInit", "Initialiser", 15, 15, 120, 30);
            BtnCalc = UI.Btn(gx, "BtnCalc", "Calculer", 170, 15, 120, 30);
            BtnAjout = UI.Btn(gx, "BtnAjout", "Ajouter", 325, 15, 120, 30); BtnAjout.Enabled = false;
            BtnQuit = UI.Btn(gx, "BtnQuit", "Quitter", 480, 15, 110, 30);

            UI.Lbl(this, "Score", 30, 480, 80, 28, 14f, true);
            TxtScore = UI.Txt(this, "TxtScore", 30, 515, 90); TxtScore.ReadOnly = true; TxtScore.Text = "0";
            var gl = UI.Grp(this, "Liste de personnes", 140, 410, 485, 240);
            string[] titres = { "CIN", "Nom", "Prénom", "Score" };
            ListBox[] L = new ListBox[4];
            for (int k = 0; k < 4; k++)
            {
                UI.Lbl(gl, titres[k], 10 + k * 118, 20, 110);
                L[k] = new ListBox { Left = 10 + k * 118, Top = 42, Width = 110, Height = 185 }; gl.Controls.Add(L[k]);
            }
            Lst1 = L[0]; Lst2 = L[1]; Lst3 = L[2]; Lst4 = L[3];

            // Evenements
            TxtLang.KeyPress += Chiffres_KeyPress; TxtTech.KeyPress += Chiffres_KeyPress;
            TxtLang.CausesValidation = true; TxtTech.CausesValidation = true;
            TxtLang.Validating += Note_Validating; TxtTech.Validating += Note_Validating;
            TxtCin.KeyPress += Chiffres_KeyPress; TxtTel.KeyPress += Chiffres_KeyPress;
            BtnChance.Click += (s, e) => TxtChance.Text = rnd.Next(0, 11).ToString();
            BtnCalc.Click += BtnCalc_Click; BtnAjout.Click += BtnAjout_Click;
            BtnInit.Click += (s, e) => Initialiser(); BtnQuit.Click += (s, e) => Close();
            System.Windows.Forms.Timer timer1;
            timer1 = new System.Windows.Forms.Timer { Interval = 1000 };
            timer1.Tick += (s, e) => MajHeure(); timer1.Start(); MajHeure();
            FormClosing += (s, e) => timer1.Stop();
        }

        void MajHeure()
        {
            LblDate.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            LblHeure.Text = "Heure : " + DateTime.Now.ToString("HH:mm:ss");
        }

        void Chiffres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        // Note entre 0 et 20
        void Note_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var t = (TextBox)sender;
            if (!int.TryParse(t.Text, out int v) || v < 0 || v > 20)
            {
                MessageBox.Show("La note doit être comprise entre 0 et 20", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                t.Text = "0"; t.SelectAll(); e.Cancel = true;
            }
        }

        void BtnCalc_Click(object sender, EventArgs e)
        {
            if (TxtCin.Text.Length != 8 || TxtNom.Text.Trim() == "" || TxtPrenom.Text.Trim() == "" || CmbAge.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez saisir un CIN (8 chiffres), le nom, le prénom et l'âge", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int score = int.Parse(TxtLang.Text) + int.Parse(TxtTech.Text);
            score += RdMas.Checked ? 7 : 5;                       // sexe
            if (ChkExper.Checked) score += 15;                   // bonus
            if (ChkMotiv.Checked) score += 10;
            if (ChkDip.Checked) score += 5;
            switch (CmbAge.SelectedIndex)                        // age
            {
                case 0: score += 20; break;
                case 1: score += 10; break;
                default: score += 5; break;
            }
            score += int.Parse(TxtChance.Text);                   // valeur de chance
            TxtScore.Text = score.ToString();
            BtnAjout.Enabled = true;
        }

        void BtnAjout_Click(object sender, EventArgs e)
        {
            Lst1.Items.Add(TxtCin.Text); Lst2.Items.Add(TxtNom.Text);
            Lst3.Items.Add(TxtPrenom.Text); Lst4.Items.Add(TxtScore.Text);
            Initialiser();
        }

        void Initialiser()
        {
            TxtCin.Clear(); TxtTel.Clear(); TxtNom.Clear(); TxtPrenom.Clear();
            TxtLang.Text = "0"; TxtTech.Text = "0"; TxtChance.Text = "0"; TxtScore.Text = "0";
            RdMas.Checked = true; ChkExper.Checked = ChkDip.Checked = ChkMotiv.Checked = false;
            CmbAge.SelectedIndex = -1; BtnAjout.Enabled = false; TxtCin.Focus();
        }
    }
}