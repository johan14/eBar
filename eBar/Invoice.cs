using eBar.models;
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
    public partial class Invoice : Form
    {
        String loggedInUserId;
        int tableId { get; set; }
        String connectionStr = @"Data Source = DESKTOP-OBA4Q9G\SQLEXPRESS;Initial Catalog = bar_db; Integrated Security = True";
        DataTable allItems { get; set; }
        

        public Invoice(String id)
        {
            InitializeComponent();
            // this.artCombo.SelectedIndexChanged += this.artCombo_SelectedIndexChanged;
            tableId = Convert.ToInt32(id);

            GetUserData();
            labelTable.Text = id;
            loggedInUserId = UserInformation.CurrentLoggedInUser;
            getAllItems();
            getInvBody();
        }

        private void getInvBody()
        {
            String query = "Select ib.*, ih.*,a.art_name, nm.measure_name from Inv_Body as ib "+
                            "INNER JOIN Inv_Header as ih on( ib.inv_header_id = ih.id)"+
                            "INNER JOIN Artikull as a on (a.id = ib.art_id)"+
                            "INNER JOIN Njesi_matese as nm on (nm.id = ib.measure_id)" +
                            "Where ih.table_id = 1 AND ih.status = 0";
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            SqlCommand sql = new SqlCommand(query, sqlConn);
            sql.Parameters.AddWithValue("@table_id", tableId);

            sqlConn.Open();
            SqlDataReader dr = sql.ExecuteReader();
            while (dr.Read()) {
                // this.listView1.Items[0]
                string[] arr = new string[5];
                ListViewItem itm;
                arr[0] = dr["art_name"].ToString();
                arr[1] = dr["quantity"].ToString();
                arr[2] = dr["price"].ToString();
                arr[3] = dr["measure_name"].ToString();
                arr[4] = (Convert.ToInt32(dr["quantity"]) * Convert.ToInt32(dr["price"])).ToString();
                itm = new ListViewItem(arr);
                listView1.Items.Add(itm);
            }
            sqlConn.Close();
            
        }

        private void GetUserData()
        {
            SqlConnection sqlConn = new SqlConnection(connectionStr);
            String getLoggedInUserQuery = "SELECT * FROM [User] WHERE id = @id";
            SqlCommand sql = new SqlCommand(getLoggedInUserQuery, sqlConn);
            sql.Parameters.AddWithValue("@id", Convert.ToInt32(UserInformation.CurrentLoggedInUser));


           sqlConn.Open();
            SqlDataReader dr = sql.ExecuteReader();
           System.Diagnostics.Debug.WriteLine("ID" + loggedInUserId);
            while (dr.Read()) {
          
                InitUserData(dr["name"].ToString(), dr["last_name"].ToString());
            }
            
            
        }
        private void InitUserData(String name, String lastname) {

            labelName.Text = name;
            labelLastname.Text = lastname;
      
        }
        private void getAllItems() {
            String getItemsQuery = "SELECT Artikull.*, Njesi_matese.measure_name " +
                                    "FROM Artikull INNER JOIN Njesi_matese ON Artikull.njesi_id=Njesi_matese.id";
            SqlConnection sqlconn = new SqlConnection(connectionStr);
            SqlCommand getItems = new SqlCommand(getItemsQuery, sqlconn);
            sqlconn.Open();
            SqlDataReader dr = getItems.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            allItems = dt;
            var datasource = new List<Artikull>();
            this.artCombo.DisplayMember = "art_name";
            this.artCombo.ValueMember = "id";
       
            this.artCombo.DataSource = dt;
            sqlconn.Close();
        }
       
        private void artCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (DataRow row in allItems.Rows) {
                 
                if (Convert.ToInt32(row["id"]) == Convert.ToInt32(this.artCombo.SelectedValue)) {
                    this.textBox1.Text = row["price"].ToString();
                    this.textBox2.Text = row["measure_name"].ToString();
                }
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
