using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eBar
{
    public partial class MenaxherHome : Form
    {
        String connectionStr = @"Data Source = JOHAN-PC\SQLEXPRESS;Initial Catalog = bar_db; Integrated Security = True";

        public MenaxherHome()
        {
            InitializeComponent();
            listBox1.SelectedIndex = 0;
            getKategoriList();
            getKatID();
            getArtikullList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel[] pan = { Kategori, Artikull, NjesiMatese, Furnitore, NrTavoline, Furnizim, Kamarier, HappyHour };
            pan[listBox1.SelectedIndex].BringToFront();

        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Kategoria                                           ///
        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        private void shtoKategori()
        {
            String shtoKatquery = "INSERT INTO Kategoria (emri) VALUES (@emri)";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand shtoKatCmd = new SqlCommand(shtoKatquery, sqlConn);
            shtoKatCmd.Parameters.AddWithValue("@emri", shtoKatText.Text);

            sqlConn.Open();
            int i = shtoKatCmd.ExecuteNonQuery();
            if (i == 1)
                MessageBox.Show("Kategoria u shtua me sukses!");
            sqlConn.Close();
            katListView.Clear();
            getKategoriList();

        }

        private void shtoKatButton_Click(object sender, EventArgs e)
        {
            shtoKategori();
        }

        private void getKategoriList()
        {
            String shfaqKat = "SELECT emri FROM Kategoria";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqKatCmd = new SqlCommand(shfaqKat, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqKatCmd.ExecuteReader();

            while (dt.Read())
            {
                var listViewItem = new ListViewItem(Convert.ToString(dt["emri"]));
                katListView.Items.Add(listViewItem);
            }
            katListView.View = View.List;
            sqlconn.Close();
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Artikull                                           ///
        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        public void getKatID()
        {
            String getKatIDquery = "SELECT id,emri FROM Kategoria";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand getKatIDcmd = new SqlCommand(getKatIDquery, sqlConn);

            sqlConn.Open();

            SqlDataReader dr = getKatIDcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            katCombo.ValueMember = "id";
            katCombo.DisplayMember = "emri";
            katCombo.DataSource = dt;
            //Console.WriteLine(katCombo.SelectedValue.ToString());
            sqlConn.Close();

        }

        private void shtoArtButton_Click(object sender, EventArgs e)
        {
            shtoArtikull();
        }

        private void shtoArtikull()
        {
            String shtoArtquery = "INSERT INTO Artikull (art_name,kat_id,is_simple,cmimi,gjendja,sasi_min,sasi_mes) " +
                "VALUES (@art_name,@kat_id,@is_simple,@cmimi,@gjendja,@sasi_min,@sasi_mes)";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand shtoArtCmd = new SqlCommand(shtoArtquery, sqlConn);
            shtoArtCmd.Parameters.AddWithValue("@art_name", emriArt.Text);
            shtoArtCmd.Parameters.AddWithValue("@kat_id", katCombo.SelectedValue.ToString());
            shtoArtCmd.Parameters.AddWithValue("@is_simple", thjeshteJo.Checked == true ? 0 : 1);
            shtoArtCmd.Parameters.AddWithValue("@cmimi", cmimiArt.Text);
            shtoArtCmd.Parameters.AddWithValue("@gjendja", gjendjaArt.Text);
            shtoArtCmd.Parameters.AddWithValue("@sasi_min", sasiaMin.Text);
            shtoArtCmd.Parameters.AddWithValue("@sasi_mes", sasiaMes.Text);

            sqlConn.Open();
            int i = shtoArtCmd.ExecuteNonQuery();
            if (i == 1)
                MessageBox.Show("Artikulli u shtua me sukses!");
            sqlConn.Close();
            ClearPanels(groupBox9);
            artListView.Clear();
            getArtikullList();
            

        }

        public void ClearPanels(Control control)
        {

            foreach (Control childControl in control.Controls)
            {
                if (childControl is TextBox)
                {
                    childControl.ResetText();
                    ClearPanels(childControl); // recursive call
                }
            }
        }

        private void getArtikullList()
        {
            String shfaqKat = "SELECT art_name FROM Artikull";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqKatCmd = new SqlCommand(shfaqKat, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqKatCmd.ExecuteReader();

            while (dt.Read())
            {
                var listViewItem = new ListViewItem(Convert.ToString(dt["art_name"]));
                artListView.Items.Add(listViewItem);
            }
            artListView.View = View.List;
            sqlconn.Close();
        }

        private void thjeshteJo_CheckedChanged(object sender, EventArgs e)
        {
            if (thjeshteJo.Checked && emriArt.Text!="")
            {
                MessageBox.Show("haahahah");
            }
        }
    }
}
