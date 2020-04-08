using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace PPIU_Lab2
{
    public partial class MainWindow : Window
    {
        private SQLiteDataAdapter m_oDataAdapter = null;
        private DataSet m_oDataSet = null;
        private DataTable m_oDataTable = null;

        public MainWindow()
        {
            InitializeComponent();
            InitBinding();
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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //TODO: sprawdzenie czy dane istnieją w bazie
            //TODO: porownanie haslo zwyklego i powtórzonego, jesli się zgadza to kod niżej, jeśli nie to popup z errorem:
            DateTime dateTime = DateTime.UtcNow.Date;
            DataRow oDataRow = m_oDataTable.NewRow();
            oDataRow[1] = txbImie.Text;
            oDataRow[2] = txbNazwisko.Text;
            oDataRow[3] = txbEmail.Text;
            oDataRow[4] = txbRola.Text;
            //TODO: płeć będzie wybierana na podstawie danego checkboxa, do implementacji
            oDataRow[5] = chbKobieta.IsChecked == true ? "Kobieta" : "Mężczyzna";
            oDataRow[6] = txbLogin.Text;
            oDataRow[7] = txbHaslo.Text;
            oDataRow[8] = true;
            oDataRow[9] = dateTime.ToString("dd/MM/yyyy");
            m_oDataTable.Rows.Add(oDataRow);
            m_oDataAdapter.Update(m_oDataSet);
            var page = new DataPage();
            page.Show();
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
    }
}
