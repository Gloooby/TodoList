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
        int i = 0;
        private static string connStr;
        private SQLiteConnection sqLiteConnection = null;
        private SQLiteDataAdapter sqLiteDataAdapter = null;
        private SQLiteCommandBuilder sqLiteBuilder = null;
        private DataSet dataSet = null;
        private void create()
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
                    //@"CREATE TABLE [Todolist] (
                    //[Id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    //[Text] char(250) NOT NULL,
                    //[Time] char(100) NOT NULL
                    //);";
                    command.ExecuteNonQuery();
                }
            }
        }
        public static class SQLiteConnectionString
        {

            public static string GetConnectionString(string path)
            {
                return GetConnectionString(path, null);
            }

            public static string GetConnectionString(string path, string password)
            {
                if (string.IsNullOrEmpty(password))
                {
                    return "Data Source=" + path;
                }
                else
                {
                    return "Data Source=" + path + ";Password=" + password;
                }
            }

        }

        private void add()
        {
            connStr = SQLiteConnectionString.GetConnectionString(".\\Todolist.db");
            SQLiteConnection conn = new SQLiteConnection(connStr);

            dataSet = new DataSet();
            sqLiteDataAdapter = new SQLiteDataAdapter("SELECT * FROM Todolist ORDER BY Time", conn);
            sqLiteBuilder = new SQLiteCommandBuilder(sqLiteDataAdapter);
            sqLiteBuilder.GetUpdateCommand();
            sqLiteBuilder.GetDeleteCommand();
            sqLiteDataAdapter.Fill(dataSet, "Todolist");
            textBlock_Copy1.Text = dataSet.Tables["Todolist"].Rows.Count.ToString();
            textBlock_Copy.Text = dataSet.Tables["Todolist"].Rows[0]["Text"].ToString();
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            add();
            textBlock.Text = i.ToString();
            i++;
        }
    }
}
