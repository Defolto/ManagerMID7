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

namespace ManagerMID7
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Asus\source\repos\ManagerMID7\ManagerMID7\Database.mdf;Integrated Security=True"; // физ расположение бд

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync(); // соединение с бд

            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Sites]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync(); // считываем таблицу
                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add("Адрес: " + Convert.ToString(sqlReader["adress"]) + "     Заказчик: " + Convert.ToString(sqlReader["customer"]) + "       День оплаты: " + Convert.ToString(sqlReader["dataCash"]) + "       Заработано: " + Convert.ToString(sqlReader["profit"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close(); // Без утечек
            }
        }

        private async void button1_Click(object sender, EventArgs e) // регистрация
        {
            if (label5.Visible)
            {
                label5.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox1.Text) || !string.IsNullOrEmpty(textBox2.Text) || !string.IsNullOrEmpty(textBox3.Text) || !string.IsNullOrEmpty(textBox4.Text) ||
                !string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text) || !string.IsNullOrWhiteSpace(textBox3.Text) || !string.IsNullOrWhiteSpace(textBox4.Text))
            {

                SqlCommand command = new SqlCommand("INSERT INTO [Sites] (adress, customer, dataCash, profit)VALUES(@Adress, @Customer, @DataCash, @Profit)", sqlConnection);

                command.Parameters.AddWithValue("Adress", textBox1.Text);
                command.Parameters.AddWithValue("Customer", textBox2.Text);
                command.Parameters.AddWithValue("DataCash", textBox3.Text);
                command.Parameters.AddWithValue("Profit", textBox4.Text);

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                label5.Visible = true;
                label5.Text = "Заполните все поля!";
            }
        }

        private async void tabControl1_Click(object sender, EventArgs e) // обновление при переходе к списку
        {
            listBox1.Items.Clear();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Sites]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync(); // считываем таблицу
                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add("Адрес: " + Convert.ToString(sqlReader["adress"]) + "     Заказчик: " + Convert.ToString(sqlReader["customer"]) + "       День оплаты: " + Convert.ToString(sqlReader["dataCash"]) + "       Заработано: " + Convert.ToString(sqlReader["profit"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (label10.Visible)
            {
                label10.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox5.Text) || !string.IsNullOrEmpty(textBox6.Text) || !string.IsNullOrEmpty(textBox7.Text) || !string.IsNullOrEmpty(textBox8.Text) ||
                !string.IsNullOrWhiteSpace(textBox5.Text) || !string.IsNullOrWhiteSpace(textBox6.Text) || !string.IsNullOrWhiteSpace(textBox7.Text) || !string.IsNullOrWhiteSpace(textBox8.Text))
            {
                SqlCommand command = new SqlCommand("UPDATE [Sites] SET [adress]=@Adress, [customer]=@Customer, [dataCash]=@DataCash, [profit]=@Profit WHERE [adress]=@adress", sqlConnection);

                command.Parameters.AddWithValue("adress", textBox5.Text);
                command.Parameters.AddWithValue("customer", textBox6.Text);
                command.Parameters.AddWithValue("dataCash", textBox7.Text);
                command.Parameters.AddWithValue("profit", textBox8.Text);

                await command.ExecuteNonQueryAsync();
            }
            else if (!string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text))
            {
                label10.Visible = true;
                label10.Text = "Заполните адресс!";
            }
            else {
                label10.Visible = true;
                label10.Text = "Не всё было заполнено!";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (label12.Visible)
            {
                label12.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox9.Text) || !string.IsNullOrWhiteSpace(textBox9.Text))
            {
                SqlCommand command = new SqlCommand("DELETE FROM [Sites] WHERE [adress]=@Adress", sqlConnection);

                command.Parameters.AddWithValue("adress", textBox9.Text);

                await command.ExecuteNonQueryAsync();
            }
            else {
                label12.Visible = true;
                label12.Text = "Заполните адрес!";
            }
        }
    }
}
