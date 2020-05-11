using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace PPIU_Lab2
{
    public partial class LoginPage : Window
    {
        private SQLiteDataAdapter m_oDataAdapter = null;
        private DataSet m_oDataSet = null;
        private DataTable m_oDataTable = null;
        public LoginPage()
        {
            InitializeComponent();
            
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
        private void PokazHaslo_Click(object sender, RoutedEventArgs e)
        {
            if (psbHaslo.Visibility == System.Windows.Visibility.Visible)
            {
                btHaslo.Content = "Ukryj hasło";
                psbHaslo.Visibility = System.Windows.Visibility.Hidden;
                tbShowedPass.Text = psbHaslo.Password;
                tbShowedPass.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                btHaslo.Content = "Pokaż hasło";
                psbHaslo.Visibility = System.Windows.Visibility.Visible;
                tbShowedPass.Visibility = System.Windows.Visibility.Hidden;
                psbHaslo.Password = tbShowedPass.Text;
            }

        }
        private void Rejestracja_Click(object sender, RoutedEventArgs e)
        {
            var page = new MainWindow();
            page.Show();
        }
        bool succesLogin = false;
        bool succesHaslo = false;

        void checkingLogin()
        {
            SQLiteConnection oSQLiteConnection =
                 new SQLiteConnection("Data Source=D:\\Projects\\PPIU-Lab2\\PPIU-Lab2\\Database.s3db");
            SQLiteCommand oCommand = oSQLiteConnection.CreateCommand();
            oCommand.CommandText = "SELECT Login FROM User WHERE Login='" + tbLogin.Text + "' AND Haslo='" + psbHaslo.Password + "'";
            m_oDataAdapter = new SQLiteDataAdapter(oCommand.CommandText,
               oSQLiteConnection);
            SQLiteCommandBuilder oCommandBuilder =
                new SQLiteCommandBuilder(m_oDataAdapter);

            m_oDataSet = new DataSet();
            m_oDataAdapter.Fill(m_oDataSet);
            m_oDataTable = m_oDataSet.Tables[0];
            DataRow count = null;
            bool czyUdaloSie = true;
            try
            {
                count = m_oDataTable.Rows[0];
            }
            catch(Exception)
            {
                count = null;
                czyUdaloSie = false;
            }
            if (count != null && czyUdaloSie == true)
            {
                succesLogin = true;
            }
            else
            {
                MessageBox.Show("Podany Login jest nieprawidłowy!");
            }
            m_oDataAdapter.Dispose();
            m_oDataAdapter = null;
        }
        void checkingPassword()
        {
            SQLiteConnection oSQLiteConnection = new SQLiteConnection("Data Source=D:\\Projects\\PPIU-Lab2\\PPIU-Lab2\\Database.s3db");
            SQLiteCommand oCommand = oSQLiteConnection.CreateCommand();
            oCommand.CommandText = "SELECT Haslo FROM User WHERE Haslo='" + psbHaslo.Password + "'";
            m_oDataAdapter = new SQLiteDataAdapter(oCommand.CommandText,
              oSQLiteConnection);
            SQLiteCommandBuilder oCommandBuilder =
                new SQLiteCommandBuilder(m_oDataAdapter);

            m_oDataSet = new DataSet();
            m_oDataAdapter.Fill(m_oDataSet);
            m_oDataTable = m_oDataSet.Tables[0];
            DataRow count = null;
            bool czyUdaloSie = true;

            try
            {
                count = m_oDataTable.Rows[0];
            }
            catch (Exception)
            {
                count = null;
                czyUdaloSie = false;
            }

            if (count != null && czyUdaloSie == true)
            {
                succesHaslo = true;
            }
            else
            {
                MessageBox.Show("Podane Hasło jest nieprawidłowe!");
            }
            m_oDataAdapter.Dispose();
            m_oDataAdapter = null;
        }
        int probyLogowania = 3;
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (probyLogowania != 0)
            {
                if (tbLogin.Text == "admin" && psbHaslo.Password == "admin") //jeśli admin
                {
                    var page = new AdminPage();
                    page.Show();
                }
                else //jeśli user
                {
                    checkingLogin();
                    checkingPassword();
                    if (succesLogin == true && succesHaslo == true)
                    {
                        var page = new EventsPage();
                        page.Show();
                    }
                    if (null != m_oDataAdapter)
                    {
                        m_oDataAdapter.Dispose();
                        m_oDataAdapter = null;
                    }
                    else
                    {
                        probyLogowania--;
                        lbProbyLogowania.Content = probyLogowania;
                    }
                }
            }
            else
            {
                btLogowanie.IsEnabled = false;
            }
        }

        private void Notatka_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Admin: (admin, admin); User: (user, user)!");
        }
    }
}