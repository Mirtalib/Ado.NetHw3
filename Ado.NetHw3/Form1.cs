using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace Ado.NetHw3
{
    public partial class Form1 : Form
    {
        DbProviderFactory? providerFactory = null;
        DbConnection? connection = null;
        IConfigurationRoot? configuration = null;
        string? conStr = null;

        public Form1()
        {
            InitializeComponent();
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", typeof(SqlClientFactory));
            conStr = "Data Source=.;Initial Catalog=Library1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            CreateConnection();
        }

        private void CreateConnection()
        {
            providerFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            connection = providerFactory.CreateConnection();
            connection.ConnectionString = conStr;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtQuery.Text))
                {
                    MessageBox.Show("It can't be empty, write something");
                    return;
                }
                TabPage tabPage = new TabPage();
                tabPage.Text = "Result";
                DataGridView dataGridView = new DataGridView();
                dataGridView.Size = new Size(810, 385);


                using var command = connection.CreateCommand();
                command.CommandText = txtQuery.Text;
                using var dataAdapter = providerFactory?.CreateDataAdapter();
                dataAdapter.SelectCommand = command;
                DataTable table = new();
                dataAdapter.Fill(table);


                dataGridView.DataSource = table;
                tabPage.Controls.Add(dataGridView);
                tabControl1.Controls.Add(tabPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
        }
    }
}