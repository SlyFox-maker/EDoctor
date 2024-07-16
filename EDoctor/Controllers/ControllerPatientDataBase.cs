using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EDoctor.Controllers
{
    class ControllerPatientDataBase
    {
        private string connectionString = "";

        public ControllerPatientDataBase()
        {

            string relativePath = @"..\..\..\bd\Patients.mdf";
            string absolutePath = System.IO.Path.GetFullPath(relativePath);

            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + absolutePath + ";Integrated Security=True";

        }
        public bool createNewPatient(String fullName, String surnamePaterno, String surnameMaterno, DateTime dateOfBrith, String sex, String address=null,String city=null, String numberPhone=null, String postalCode = null, String email = null)
        {
            bool status = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Debug.WriteLine("Соединение установлено успешно.");

                    string sqlQueryForPatientTable = "INSERT INTO dbo.[Patient] (FullName, SurnamePaterno, SurnameMaterno, DateOfBirth, Sex) OUTPUT INSERTED.IdPatient VALUES (@FullName, @SurnamePaterno" +
                                                                                                                                                                ", @SurnameMaterno, @DateOfBrith, @Sex);";
                    string sqlQueryForPatientContactTable = "INSERT INTO [dbo].[PatinentContact] (IdPatient, Address, City, NumberPhone, PostalCode, Email) VALUES (@patientId, @Address, @City," +
                                                                                                                                                                "@numberPhone, @PostalCode, @Email);";

                    using (SqlCommand command = new SqlCommand(sqlQueryForPatientTable, conn))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@SurnamePaterno", surnamePaterno);
                        command.Parameters.AddWithValue("@SurnameMaterno", surnameMaterno);
                        command.Parameters.AddWithValue("@DateOfBrith", dateOfBrith);
                        command.Parameters.AddWithValue("@Sex", sex);

                        int patientId = (int)command.ExecuteScalar();

                        command.Dispose();


                        using (SqlCommand patientContactCommand = new SqlCommand(sqlQueryForPatientContactTable, conn))
                        {
                            patientContactCommand.Parameters.AddWithValue("@patientId", patientId);
                            patientContactCommand.Parameters.AddWithValue("@Address", address);
                            patientContactCommand.Parameters.AddWithValue("@City", city);
                            patientContactCommand.Parameters.AddWithValue("@numberPhone", numberPhone);
                            patientContactCommand.Parameters.AddWithValue("@PostalCode", postalCode);
                            patientContactCommand.Parameters.AddWithValue("@Email", email);

                            // Выполняем запрос для вставки данных в таблицу DrInformation
                            int drRowsAffected = patientContactCommand.ExecuteNonQuery();

                            // Проверяем успешность операции
                            if (drRowsAffected > 0)
                            {
                                status = true;
                            }
                        }
                        conn.Close();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
                }
                conn.Close();
            }
            return status;    
        }

        public List<List<Object>> getResultOfSearchPatient(String searchName)
        {
            List<List<Object>> results = new List<List<Object>>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Debug.WriteLine("Соединение установлено успешно.");

                    string sqlQueryForSearch = @"
                SELECT FullName, SurnamePaterno, SurnameMaterno, IdPatient, DateOfBirth, Sex
                FROM dbo.[Patient]
                WHERE FullName LIKE @searchName 
                OR SurnamePaterno LIKE @searchName 
                OR SurnameMaterno LIKE @searchName";

                    using (SqlCommand command = new SqlCommand(sqlQueryForSearch, conn))
                    {
                        command.Parameters.AddWithValue("@searchName", searchName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Debug.WriteLine("Ничего не найдено");
                                return null;
                            }
                            while (reader.Read())
                            {
                                List<Object> patient = new List<Object>();

                                String fullName = reader["FullName"].ToString() + " " + reader["SurnamePaterno"].ToString()+" " + reader["SurnameMaterno"].ToString();
                                int id = Convert.ToInt32(reader["IdPatient"]);
                                DateTime birthDate = Convert.ToDateTime(reader["DateOfBirth"]);
                                String sex = reader["Sex"].ToString();

                                patient.Add(id);
                                patient.Add(fullName);
                                patient.Add(birthDate);
                                patient.Add(sex);

                                results.Add(patient);

                                Debug.WriteLine(fullName);
                            }
                        }


                        conn.Close();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
                }
                conn.Close();
            }
            return results;
        }
    
        public List<int> getStatisticOfPatients()
        {
            List<int> result = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Debug.WriteLine("Соединение установлено успешно.");

                    string totalPatientsQuery = "SELECT COUNT(*) FROM dbo.[Patient]";
                    int totalPatients = GetCountFromQuery(totalPatientsQuery, conn);


                    // Получаем число пациентов женщин
                    string femalePatientsQuery = "SELECT COUNT(*) FROM dbo.[Patient] WHERE Sex = 'Mujer'";
                    int femalePatients = GetCountFromQuery(femalePatientsQuery, conn);


                    // Получаем число пациентов мужчин
                    string malePatientsQuery = "SELECT COUNT(*) FROM dbo.[Patient] WHERE Sex = 'Hombre'";
                    int malePatients = GetCountFromQuery(malePatientsQuery, conn);

                    result.Add(totalPatients);
                    result.Add(femalePatients);
                    result.Add(malePatients);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
                }
                conn.Close();
            }
            return result;
        }
        static int GetCountFromQuery(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (int)command.ExecuteScalar();
            }
        }
    }
}
