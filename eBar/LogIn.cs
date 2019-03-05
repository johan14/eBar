using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace eBar
{
    public partial class LogIn : Form
    {
        

        public LogIn()
        {
            InitializeComponent();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            
        }

        private void logButton_Click(object sender, EventArgs e)
        {
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            String connectionStr = @"Data Source = JOHAN-PC\SQLEXPRESS;Initial Catalog = bar_db; Integrated Security = True";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand getNumberRows = new SqlCommand("SELECT COUNT(*) as cnt FROM [User] WHERE user_name = @username AND password = @password ",sqlconn);
            SqlCommand getRolID = new SqlCommand("SELECT role_id FROM [user] WHERE user_name = @username AND password = @password ",sqlconn);
            getNumberRows.Parameters.Clear();
            getNumberRows.Parameters.AddWithValue("@username", username.Text);
            getNumberRows.Parameters.AddWithValue("@password", encryptionHelper.Encrypt(password.Text));
            getRolID.Parameters.Clear();
            getRolID.Parameters.AddWithValue("@username", username.Text);
            getRolID.Parameters.AddWithValue("@password", encryptionHelper.Encrypt(password.Text));

            
            sqlconn.Open();
            
                if (getNumberRows.ExecuteScalar().ToString() == "1")
                {           
                SqlDataReader dr = getRolID.ExecuteReader();
                if (dr.Read())
                {
                    if(Convert.ToInt32(dr["role_id"])== 1){
                        KamarierHome kh = new KamarierHome();
                        kh.Show();
                        this.Hide();
                        
                    }
                    else
                    {

                        MenaxherHome mh = new MenaxherHome();
                        mh.Show();
                        this.Hide();
                    }
                }          
                }
                else
                {
                    MessageBox.Show("Nuk u gjend asnje rekord");
                }

            username.Clear();
            password.Clear();
            sqlconn.Close();
            

        }

        private void LogIn_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
