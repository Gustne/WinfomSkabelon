using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Models;

namespace DataAccess
{
    public class PersonDA
    {
        string connString;

        public PersonDA()
        {
            connString = ConfigurationManager.ConnectionStrings["BojeSQL"].ConnectionString;
        }

        public async Task<List<Person>> GetAsync()
        {
            string command = "SELECT * FROM Person";
            using SqlConnection dbConn = new SqlConnection(connString);
            SqlCommand sqlCommand = new SqlCommand(command, dbConn);

            return await LoadData(sqlCommand, dbConn);
        }

        public async Task<Person> GetAsync(int id)
        {
            string command = "SELECT * FROM Kunder where Id = @Id";
            using SqlConnection dbConn = new SqlConnection(connString);
            SqlCommand sqlCommand = new SqlCommand(command, dbConn);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            List<Person> output = await LoadData(sqlCommand, dbConn);

            if (output.Count == 0)
            {
                return new Person();
            }
            else
            {
                return output[0];
            }

        }

        public bool Create(Person person)
        {
            using SqlConnection dbConn = new SqlConnection(connString);
            ConnectToSql(dbConn);

            string command = "INSERT INTO Kunder(ErSælger, ErKøber, Fnavn, Lnavn, CPRnr, Tlf, Mail)" +
                            " values(@Sælger, @Køber, @Fornavn, @Efternavn, @CPRnr, @Tlf, @Mail)";
            using SqlCommand sqlCommand = new SqlCommand(command, dbConn);
            AddParameters(sqlCommand, kunde);

            return ExecuteSql(sqlCommand, dbConn);

        }
        private Task<bool> ConnectToSql(SqlConnection dbConn)
        {
            try
            {
                dbConn.Open();
            }
            catch (Exception)
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    dbConn.Close();
                }
                return false;
            }
            return true;
        }

        private async Task<List<Person>> LoadData(SqlCommand sqlCommand, SqlConnection dbConn)
        {
            List<Person> persons = new List<Person>();
            try
            {
                await dbConn.OpenAsync();
            }
            catch (Exception)
            {
                // her skal vi måske få den til at kaste en exception som vi kan håndtere i UI når den ikke får forbindelse
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    await dbConn.CloseAsync();
                }
                return persons;
            }

            try
            {
                using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Person person = new Person();
                    person.Id = (int)reader["Id"];
                    person.FirstName = (string)reader["FirstName"];
                    person.LastName = (string)reader["LastName"];
                    person.Address = (string)reader["Address"];
                    person.City = (string)reader["City"];
                    person.PostalCode = (int)reader["PostalCode"];
                    person.Email = (string)reader["Email"];
                    person.Phone = (string)reader["Phone"];

                    persons.Add(person);

                }

            }
            catch (Exception)
            {
                //igen noget errorhandling
            }
            finally
            {
                await dbConn.CloseAsync();
            }
            return persons;
        }

    }

}