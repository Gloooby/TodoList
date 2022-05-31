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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;
namespace TodoList
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string connStr;
        private SQLiteDataAdapter sqLiteDataAdapter = null;
        private DataSet dataSet = null;
        private SQLiteConnection conn = null;
        private SQLiteCommand sqlCommand = new SQLiteCommand();
        private void create()
        {
            try
            {
                string baseName = "Todolist.db";

                SQLiteConnection.CreateFile(baseName);

                SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                {
                    connection.ConnectionString = "Data Source = " + baseName;
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText =
                        @"CREATE TABLE [Todolist](
                        [Id]    INTEGER NOT NULL,
                        [Text]  TEXT,
                        [Time]  TEXT,
                        PRIMARY KEY([Id] AUTOINCREMENT)
                        );";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}
        

        private void takeData(bool reOrderd)
        {
            try
            {
                if (reOrderd)
                {
                    connStr = "Data Source=.\\Todolist.db";
                    conn = new SQLiteConnection(connStr);
                    conn.Open();
                    sqlCommand.Connection = conn;
                }
                dataSet = new DataSet();
                sqLiteDataAdapter = new SQLiteDataAdapter("SELECT * FROM Todolist ORDER BY Time", conn);
                sqLiteDataAdapter.Fill(dataSet, "Todolist");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            initDB();
        }

        private void initDB()
        {
            if (!File.Exists(".\\Todolist.db"))
            {
                create();
            }
            takeData(true);
        }

        private void addNewElem(string text, DateTime date)
        {
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
                return;
            }

            try
            {
                sqlCommand.CommandText = "INSERT INTO Todolist ('Text', 'Time') values ('" +
                    text + "' , '" +
                    date + "')";

                sqlCommand.ExecuteNonQuery();
                takeData(false);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void removeElem(int nelem)
        {
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
                return;
            }

            try
            {
                sqlCommand.CommandText = "DELETE FROM `Todolist` WHERE `ID` = " + nelem + " LIMIT 1";

                sqlCommand.ExecuteNonQuery();
                takeData(false);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //textBlock_Copy.Text = dataSet.Tables["Todolist"].Rows[0]["Text"].ToString();
            removeElem(1);
            addNewElem("треня2", DateTime.Now);
            textBlock.Text = dataSet.Tables["Todolist"].Rows[0]["Text"].ToString();
            textBlock_Copy1.Text = dataSet.Tables["Todolist"].Rows[0]["Time"].ToString();
        }
    }
}
