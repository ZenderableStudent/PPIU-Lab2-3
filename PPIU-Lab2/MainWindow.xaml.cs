using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace PPIU_Lab2
{
    public partial class MainWindow : Window
    {
        private SQLiteDataAdapter m_oDataAdapter = null;
        private DataSet m_oDataSet = null;
        private DataTable m_oDataTable = null;
        private bool goodEmail;

        public MainWindow()
        {
            this.InitializeComponent();
            InitBinding();
        }

        public static void emailSending(string mail)
        {
            string to = mail;
            string from = "automailzenderable@gmail.com"; //trzeba wpisac swoj email i potwierdzic w ustawieniach bezpieczenstwa!
            MailMessage wiadomosc = new MailMessage(from, to);
            wiadomosc.Subject = "Potwierdzenie";
            wiadomosc.Body = "Mail weryfikacyjny.";

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("automailzenderable@gmail.com", "pizza123"); //testowe dane, do testu trzeba wpisac wlasne i potwierdzic w ustawienaich bezpieczeństwa np. w Google!
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Send(wiadomosc);
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            if (chbMezczyzna.IsChecked == true)
            {
                chbKobieta.IsChecked = false;
            }
        }

        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            if (chbKobieta.IsChecked == true)
            {
                chbMezczyzna.IsChecked = false;
            }
        }

        private void InitBinding()
        {
            //trzeba wpisac własną ścieżkę do bazy danych
            SQLiteConnection oSQLiteConnection =
                new SQLiteConnection("Data Source=D:\\Projects\\PPIU-Lab2\\PPIU-Lab2\\Database.s3db");
            SQLiteCommand oCommand = oSQLiteConnection.CreateCommand();
            oCommand.CommandText = "SELECT * FROM user";
            m_oDataAdapter = new SQLiteDataAdapter(oCommand.CommandText,
                oSQLiteConnection);
            SQLiteCommandBuilder oCommandBuilder =
                new SQLiteCommandBuilder(m_oDataAdapter);
            m_oDataSet = new DataSet();
            m_oDataAdapter.Fill(m_oDataSet);
            m_oDataTable = m_oDataSet.Tables[0];
        }

        public bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
        
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            goodEmail = IsValidEmailAddress(txbEmail.Text);
            if (chbKobieta.IsChecked == false && chbMezczyzna.IsChecked == false)
            {
                MessageBox.Show("Wybierz płeć!!");
            }
            else if (txbHaslo.Text == txbPotwHaslo.Text && goodEmail == true)
            {
                DateTime dateTime = DateTime.UtcNow.Date;
                DataRow oDataRow = m_oDataTable.NewRow();
                oDataRow[1] = txbImie.Text;
                oDataRow[2] = txbNazwisko.Text;
                oDataRow[3] = txbEmail.Text;
                oDataRow[4] = "user";
                oDataRow[5] = chbKobieta.IsChecked == true ? "Kobieta" : "Mężczyzna";
                oDataRow[6] = txbLogin.Text;
                oDataRow[7] = txbHaslo.Text;
                oDataRow[8] = true;
                oDataRow[9] = dateTime.ToString("dd/MM/yyyy");
                m_oDataTable.Rows.Add(oDataRow);
                m_oDataAdapter.Update(m_oDataSet);
                //emailSending(txbEmail.Text);
                var page = new DataPage();
                page.Show();
            }
            else if (txbHaslo.Text != txbPotwHaslo.Text && goodEmail == true)
            {
                MessageBox.Show("Źle powtórzone hasło!");
            }
            else if (goodEmail == false && txbHaslo.Text == txbPotwHaslo.Text)
            {
                MessageBox.Show("Niepoprawny adres e-mail!");
            }
            else if(txbHaslo.Text != txbPotwHaslo.Text && goodEmail == false)
            {
                MessageBox.Show("Niepoprawny adres e-mail oraz źle powtórzone hasła!");
            }
        }

        private void Window_Closing(object sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (null != m_oDataAdapter)
            {
                m_oDataAdapter.Dispose();
                m_oDataAdapter = null;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txbImie.Text = "";
            txbNazwisko.Text = "";
            txbLogin.Text = "";
            txbEmail.Text = "";
            txbHaslo.Text = "";
            txbPotwHaslo.Text = "";
            txbRola.Text = "";
        }

        private void btnDataBase_Click(object sender, RoutedEventArgs e)
        {
            var page = new DataPage();
            page.Show();
        }
    }
}