using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;

namespace PPIU_Lab2
{
    /// <summary>
    /// Interaction logic for EventsPage.xaml
    /// </summary>
    public partial class EventsPage : Window
    {

        private SQLiteDataAdapter m_oDataAdapter = null;
        private DataSet m_oDataSet = null;
        private DataTable m_oDataTable = null;

        public EventsPage()
        {
            InitializeComponent();
            InitBinding();
        }

        private void InitBinding()
        {
            SQLiteConnection oSQLiteConnection =
                new SQLiteConnection("Data Source=D:\\Projects\\PPIU-Lab2\\PPIU-Lab2\\Database.s3db");
            SQLiteCommand oCommand = oSQLiteConnection.CreateCommand();
            oCommand.CommandText = "SELECT * FROM event";
            m_oDataAdapter = new SQLiteDataAdapter(oCommand.CommandText,
                oSQLiteConnection);
            SQLiteCommandBuilder oCommandBuilder =
                new SQLiteCommandBuilder(m_oDataAdapter);
            m_oDataSet = new DataSet();
            m_oDataAdapter.Fill(m_oDataSet);
            m_oDataTable = m_oDataSet.Tables[0];
        }

        private void PrzypisanieDoUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Poprawnie zapisano!");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbWydarzenia.SelectedIndex == 0)
            {
                DataColumn oDataColumn = m_oDataTable.Columns[0];
                DataRow oDataRow = m_oDataTable.Rows[0];
                Agenda.Text = "Super opis wydarzenia sportowego";
            }
            else
            {
                DataColumn oDataColumn = m_oDataTable.Columns[1];
                DataRow oDataRow = m_oDataTable.Rows[1];
                Agenda.Text = "Super opis wydarzenia strażackiego";
            }
                
        }
    }
}
