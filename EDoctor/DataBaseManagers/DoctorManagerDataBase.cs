using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDoctor.DataBaseManagers
{
    class DoctorManagerDataBase
    {
        SqlConnection conn;
        public DoctorManagerDataBase() {

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SlyFox\source\repos\EDoctor\EDoctor\bd\Doctor.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    Debug.WriteLine("Соединение установлено успешно.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
                }
            }
        }
        public void closeConnection()
        {
            conn.Close();
        }

        public List<String> getLogins()
        {
            List<String> listOfLogins = new List<String>();


            return listOfLogins;
        }
    }
}
