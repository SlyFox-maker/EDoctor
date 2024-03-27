using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDoctor.Controllers
{
    class ControllerDoctorDataBase
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SlyFox\source\repos\EDoctor\EDoctor\bd\Doctor.mdf;Integrated Security=True";

        public ControllerDoctorDataBase() {
        }


        public String createNewUser(String fullName, String CURP, String phone, String email, String password)
        {
            String status = "";


            //creating of user for UserDb

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Debug.WriteLine("Соединение установлено успешно.");

                    string sqlQueryForUserTable = "INSERT INTO dbo.[User] (Phone, Login, [Password]) OUTPUT INSERTED.Id VALUES (@Phone, @Login, @Password);";
                    string sqlQueryForDrInformationTable = "INSERT INTO DrInformation (UserId, FullName, CURP) VALUES (@UserId, @FullName, @CURP);";

                    using (SqlCommand command = new SqlCommand(sqlQueryForUserTable, conn))
                    {
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Login", email);
                        command.Parameters.AddWithValue("@Password", getHash(password));

                        int userId = (int)command.ExecuteScalar();

                        command.Dispose();


                        using (SqlCommand drInformationCommand = new SqlCommand(sqlQueryForDrInformationTable, conn))
                        {
                            drInformationCommand.Parameters.AddWithValue("@UserId", userId);
                            drInformationCommand.Parameters.AddWithValue("@FullName", fullName);
                            drInformationCommand.Parameters.AddWithValue("@CURP", CURP);

                            // Выполняем запрос для вставки данных в таблицу DrInformation
                            int drRowsAffected = drInformationCommand.ExecuteNonQuery();

                            // Проверяем успешность операции
                            if (drRowsAffected > 0)
                            {
                                status = "Пользователь успешно создан.";
                            }
                            else
                            {
                                status = "Не удалось создать пользователя.";
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

        public string getHash(String password)
        {
            using (var hash = SHA256.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        } 
    }
}
