using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace DataBaseWorkingWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string MyConnection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        SqlConnection connection = new SqlConnection(MyConnection);
        async void Form1_Load(object sender, System.EventArgs e)
        {
            SqlConnection connection = new SqlConnection(MyConnection);
            await connection.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [tbl_workDB]", connection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while(await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["id"]) + "     " + Convert.ToString(sqlReader["Name"]) + "      " + Convert.ToString(sqlReader["Price"]));

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
        }

        void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
        }

        async void BtnAdd_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtboxNameProduct.Text) && (!string.IsNullOrEmpty(txtboxNameProduct.Text)) &&
                !string.IsNullOrEmpty(txtboxPrice.Text) && !string.IsNullOrEmpty(txtboxPrice.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [tbl_workDB] (Name, Price) VALUES (@Name, @Price)", connection);
                command.Parameters.AddWithValue("Name", txtboxNameProduct.Text);
                command.Parameters.AddWithValue("Price", txtboxPrice.Text);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                MessageBox.Show("Заполните поля!", "Уведомлдение об ошибке!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            await connection.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [tbl_workDB]", connection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["id"]) + "     " + Convert.ToString(sqlReader["Name"]) + "      " + Convert.ToString(sqlReader["Price"]));

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }
    }
}
