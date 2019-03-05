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
    public partial class KamarierHome : Form
    {
        String connectionStr = @"Data Source = DESKTOP-OBA4Q9G\SQLEXPRESS;Initial Catalog = bar_db; Integrated Security = True";
        public KamarierHome()
        {
            InitializeComponent();
            InitTables();
        }
        private void InitTables() {
            
            String getTablesQuery = "SELECT * FROM Tavolina";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand tables = new SqlCommand(getTablesQuery, sqlconn);
            sqlconn.Open();

            SqlDataReader dt = tables.ExecuteReader();
            int i = 0;
            int j = 0;
            while (dt.Read())
            {
                Button button = new Button();
                    button.Text = Convert.ToString(dt["id"]);
                    button.Location = new Point(button.Width * (j%10), button.Height * i);
                button.Click += (s, e) =>{
                    Invoice inv = new Invoice(button.Text);
                    inv.Show();
                
                };
                
                    listView1.Controls.Add(button);
                j++;
                if (j % 10 == 0) {

                    i++;
                }
            }
        

            sqlconn.Close();
        }

        private void KamarierHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogIn log = new LogIn();
            log.Show();
        }
    }
}
