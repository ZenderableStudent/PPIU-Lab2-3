using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace PPIU_Lab2
{
    public partial class DataPage : Window
    {
        private SQLiteDataAdapter m_oDataAdapter = null;
        private DataSet m_oDataSet = null;
        private DataTable m_oDataTable = null;
        public DataPage()
        {
            InitializeComponent();
            InitBinding();
        }
        private void InitBinding()
        {
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
            lstItems.DataContext = m_oDataTable.DefaultView;
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
    }
}