using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace eBar
{
    public partial class MenaxherHome : Form
    {
        String connectionStr = @"Data Source = DESKTOP-OBA4Q9G\SQLEXPRESS;Initial Catalog = bar_db; Integrated Security = True";

        public MenaxherHome()
        {
            InitializeComponent();
            listBox1.SelectedIndex = 0;
            getValues();
            setPassDots();
        }

        private void getValues()
        {
            getKategoriList();
            getKatID();
            getArtikullList();
            getFurnList();
            getNjesiList();
            getNjesiID();
            getPerdoruesList();
            data.Text =DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void setPassDots() {
            passwordField.Text = "";
            // The password character is an asterisk.  
            passwordField.PasswordChar = '*';
            // The control will allow no more than 14 characters.  
            passwordField.MaxLength = 14;
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogIn log = new LogIn();
            log.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel[] pan = { Kategori, Artikull, NjesiMatese, Furnitore, NrTavoline, Furnizim, Kamarier, HappyHour,Perdorues};
            pan[listBox1.SelectedIndex].BringToFront();

        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Kategoria                                           ///
        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        private void shtoKategori()
        {
            String shtoKatquery = "INSERT INTO Kategoria (name) VALUES (@emri)";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand shtoKatCmd = new SqlCommand(shtoKatquery, sqlConn);
            shtoKatCmd.Parameters.AddWithValue("@emri", shtoKatText.Text);

            sqlConn.Open();
            int i = shtoKatCmd.ExecuteNonQuery();
            if (i == 1)
                MessageBox.Show("Kategoria u shtua me sukses!");
            shtoKatText.Clear();
            sqlConn.Close();
            katListView.Clear();
            getKategoriList();
            getKatID();

        }

        private void shtoKatButton_Click(object sender, EventArgs e)
        {
            shtoKategori();
        }

        private void getKategoriList()
        {
            String shfaqKat = "SELECT name FROM Kategoria";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqKatCmd = new SqlCommand(shfaqKat, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqKatCmd.ExecuteReader();

            while (dt.Read())
            {
                var listViewItem = new ListViewItem(Convert.ToString(dt["name"]));
                katListView.Items.Add(listViewItem);
            }
            katListView.View = View.List;
            sqlconn.Close();
        }

        private void editKategori()
        {
            ListView.SelectedIndexCollection indices = katListView.SelectedIndices; 
            if (indices.Count > 0) { Console.WriteLine("Indices selected:" + indices); }
            String shfaqKat = "SELECT name FROM Kategoria";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqKatCmd = new SqlCommand(shfaqKat, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqKatCmd.ExecuteReader();

            while (dt.Read())
            {
                var listViewItem = new ListViewItem(Convert.ToString(dt["name"]));
                katListView.Items.Add(listViewItem);
            }
            katListView.View = View.List;
            sqlconn.Close();
        }


        private void katListView_MouseClick(object sender, MouseEventArgs e)
        {
             Console.WriteLine(katListView.SelectedItems[0].Text);
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Artikull                                           ///
        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        public void getKatID()
        {
            String getKatIDquery = "SELECT id,name FROM Kategoria";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand getKatIDcmd = new SqlCommand(getKatIDquery, sqlConn);

            sqlConn.Open();

            SqlDataReader dr = getKatIDcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            katCombo.ValueMember = "id";
            katCombo.DisplayMember = "name";
            katCombo.DataSource = dt;
            //Console.WriteLine(katCombo.SelectedValue.ToString());
            sqlConn.Close();

        }
        public void getNjesiID()
        {
            String getNjesiIDquery = "SELECT id,name FROM Njesi_Shitjeje";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand getNjesiIDcmd = new SqlCommand(getNjesiIDquery, sqlConn);

            sqlConn.Open();

            SqlDataReader dr = getNjesiIDcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBoxNjesi.ValueMember = "id";
            comboBoxNjesi.DisplayMember = "name";
            comboBoxNjesi.DataSource = dt;
            //Console.WriteLine(katCombo.SelectedValue.ToString());
            sqlConn.Close();

        }

        private void shtoArtButton_Click(object sender, EventArgs e)
        {
            shtoArtikull();
        }

        private void shtoArtikull()
        {
            String shtoArtquery = "INSERT INTO Artikull (art_name,kat_id,is_simple,price,gjendja,sasi_min,sasi_mes) " +
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
                MessageBox.Show("Message Box!!!!!!!!!");
            }
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Furnitor                                            ///
        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        private void shtoFurnitor()
        {
            String shtoFurnquery = "INSERT INTO Furnitor (fur_name) VALUES (@emri)";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand shtoFurnCmd = new SqlCommand(shtoFurnquery, sqlConn);
            if (shtoFurnText.Text != "")
            {
                shtoFurnCmd.Parameters.AddWithValue("@emri", shtoFurnText.Text);

                sqlConn.Open();
                Console.WriteLine();
                int i = shtoFurnCmd.ExecuteNonQuery();
                if (i == 1)
                    MessageBox.Show("Furnitori u shtua me sukses!");
                shtoFurnText.Clear();
                sqlConn.Close();
                furnListView.Clear();
                getFurnList();
            }
            else
                MessageBox.Show("Jepni tekst, jo hapesira");
        }

        private void shtoFurnB_Click(object sender, EventArgs e)
        {
            shtoFurnitor();
        }

        private void getFurnList()
        {
            String shfaqFurn = "SELECT fur_name FROM Furnitor";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqFurnCmd = new SqlCommand(shfaqFurn, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqFurnCmd.ExecuteReader();

            while (dt.Read())
            {
                var listViewItem = new ListViewItem(Convert.ToString(dt["fur_name"]));
                furnListView.Items.Add(listViewItem);
            }
            furnListView.View = View.List;
            sqlconn.Close();
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Njesi Matese                                        ///
        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        private void shtoNjesi()
        {
            String shtoNjesiquery = "INSERT INTO Njesi_matese (measure_name) VALUES (@emri)";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand shtoNjesiCmd = new SqlCommand(shtoNjesiquery, sqlConn);
            if (shtoNjesiText.Text != "")
            {
                shtoNjesiCmd.Parameters.AddWithValue("@emri", shtoNjesiText.Text);

                sqlConn.Open();
                Console.WriteLine();
                int i = shtoNjesiCmd.ExecuteNonQuery();
                if (i == 1)
                    MessageBox.Show("Njesia Matese u shtua me sukses!");
                shtoNjesiText.Clear();
                sqlConn.Close();
                njesiListView.Clear();
                getNjesiList();
            }
            else
                MessageBox.Show("Jepni tekst, jo hapesira");
        }

        private void shtoNjesiB_Click_1(object sender, EventArgs e)
        {
            shtoNjesi();
        }

        private void getNjesiList()
        {
            String shfaqNjesi = "SELECT measure_name FROM Njesi_matese";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqNjesiCmd = new SqlCommand(shfaqNjesi, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqNjesiCmd.ExecuteReader();

            while (dt.Read())
            {
                var listViewItem = new ListViewItem(Convert.ToString(dt["measure_name"]));
                njesiListView.Items.Add(listViewItem);
            }
            njesiListView.View = View.List;
            sqlconn.Close();
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Shto Perdorues                                      ///
        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        private void ShtoButton_Click(object sender, EventArgs e)
        {
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            String shtoUserquery = "INSERT INTO [User] (name,last_name,role_id,user_name,password) " +
              "VALUES (@name,@last_name,@role_id,@user_name,@password)";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand shtoUserCmd = new SqlCommand(shtoUserquery, sqlConn);
            shtoUserCmd.Parameters.AddWithValue("@name", nameTextBox.Text);
            shtoUserCmd.Parameters.AddWithValue("@last_name", textBoxLastName.Text);
            shtoUserCmd.Parameters.AddWithValue("@user_name", usernameTextBox.Text);
            shtoUserCmd.Parameters.AddWithValue("@njesi_id", comboBoxNjesi.SelectedValue.ToString());
            shtoUserCmd.Parameters.AddWithValue("@role_id", radioButtonKam.Checked == true ? 1 : 2);
            Console.WriteLine(encryptionHelper.Encrypt(passwordField.Text));
            shtoUserCmd.Parameters.AddWithValue("@password", encryptionHelper.Encrypt(passwordField.Text));

            sqlConn.Open();
            int i = shtoUserCmd.ExecuteNonQuery();
            if (i == 1) {
                MessageBox.Show("Perdoruesi u shtua me sukses!");
                ShtoUSerNjesi();
            }
        
            sqlConn.Close();
            ClearPanels(groupBox10);
            perdoruesListView.Items.Clear();
            getPerdoruesList();
            

        }

        private void ShtoUSerNjesi() {
             
            String getLastUserIdQuery = "SELECT TOP 1 id FROM [User] ORDER BY id DESC";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            sqlConn.Open();
            SqlCommand getLastUserIDcmd = new SqlCommand(getLastUserIdQuery, sqlConn);
            Int32 njesiId = Convert.ToInt32(comboBoxNjesi.SelectedValue);
            Int32 userId = 0;
            SqlDataReader dt = getLastUserIDcmd.ExecuteReader();
            while (dt.Read())
            {
                userId = Convert.ToInt32(dt["id"]);  
            }
            dt.Close();

            String shtoUserNjesiQuery = "INSERT INTO User_njesi (user_id, njesi_id) VALUES (@user_id, @njesi_id)";
            SqlCommand shtoUserNjesiCommand = new SqlCommand(shtoUserNjesiQuery, sqlConn);
            shtoUserNjesiCommand.Parameters.AddWithValue("@user_id", userId);
            shtoUserNjesiCommand.Parameters.AddWithValue("@njesi_id", njesiId);
          
            int i = shtoUserNjesiCommand.ExecuteNonQuery();
            sqlConn.Close();
            
        }

        private void getPerdoruesList()
        {
            String shfaqUser = "SELECT name,last_name FROM [User]";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand shfaqUserCmd = new SqlCommand(shfaqUser, sqlconn);
            sqlconn.Open();
            SqlDataReader dt = shfaqUserCmd.ExecuteReader();

            while (dt.Read())
            {
                string[] arr = new string[2];
                ListViewItem itm;
                arr[0] = dt.GetString(0);
                arr[1] = dt.GetString(1);
                itm = new ListViewItem(arr);
                perdoruesListView.Items.Add(itm);
            }
            
            sqlconn.Close();
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            Shto Furnizim                                      ///
        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        /// 

        public void shtoFurnizim()
        {

        }
    }
}
