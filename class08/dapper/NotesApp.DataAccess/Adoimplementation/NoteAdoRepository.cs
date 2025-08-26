using NotesApp.Domain.Models;
using Microsoft.Data.SqlClient;
using NotesApp.Domain.Enums;

namespace NotesApp.DataAccess.Adoimplementation
{
    public class NoteAdoRepository : IRepository<Note>
    {
        private string _connectionString;
        public NoteAdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Note entity)
        {
            // Implementation for adding a Note using ADO.NET
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            // Open connection, create command, execute command, handle exceptions, etc.
            sqlConnection.Open();
            // Create and execute SQL command to insert the Note
            SqlCommand command = new SqlCommand();
            // connect command to connection
            command.Connection = sqlConnection;
            command.CommandText = "INSERT INTO dbo.Notes (Text, Priority, Tag, UserId) VALUES (@Text, @Priority, @Tag, @UserId)";
            command.Parameters.AddWithValue("@Text", entity.Text);
            command.Parameters.AddWithValue("@Priority", entity.Priority);
            command.Parameters.AddWithValue("@Tag", entity.Tag);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Delete(int id)
        {
            // Implementation for adding a Note using ADO.NET
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            // Open connection, create command, execute command, handle exceptions, etc.
            sqlConnection.Open();
            // Create and execute SQL command to insert the Note
            SqlCommand command = new SqlCommand();
            // connect command to connection
            command.Connection = sqlConnection;
            command.CommandText = "DELETE FROM dbo.Notes WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Note> GetAll()
        {
            // Implementation for adding a Note using ADO.NET
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            // Open connection, create command, execute command, handle exceptions, etc.
            sqlConnection.Open();
            // Create and execute SQL command to insert the Note
            SqlCommand command = new SqlCommand();
            // connect command to connection
            command.Connection = sqlConnection;
            command.CommandText = "SELECT * FROM dbo.Notes";
            List<Note> notes = new List<Note>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                notes.Add(new Note()
                {
                    Id = (int)reader["Id"],
                    //Text = reader["Text"].ToString(),
                    Text = (string)reader["Text"],
                    Priority = (Priority)reader["Priority"],
                    Tag = (Tag)reader["Tag"],
                    UserId = (int)reader["UserId"]
                });
            }
            sqlConnection.Close();
            return notes;
        }

        public Note GetById(int id)
        {
            // Implementation for adding a Note using ADO.NET
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            // Open connection, create command, execute command, handle exceptions, etc.
            sqlConnection.Open();
            // Create and execute SQL command to insert the Note
            SqlCommand command = new SqlCommand();
            // connect command to connection
            command.Connection = sqlConnection;
            command.CommandText = "SELECT * FROM dbo.Notes WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            List<Note> notes = new List<Note>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                notes.Add(new Note()
                {
                    Id = (int)reader["Id"],
                    //Text = reader["Text"].ToString(),
                    Text = (string)reader["Text"],
                    Priority = (Priority)reader["Priority"],
                    Tag = (Tag)reader["Tag"],
                    UserId = (int)reader["UserId"]
                });
            }
            sqlConnection.Close();
            return notes.FirstOrDefault();

        }

        public void Update(Note entity)
        {
            // Implementation for adding a Note using ADO.NET
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            // Open connection, create command, execute command, handle exceptions, etc.
            sqlConnection.Open();
            // Create and execute SQL command to insert the Note
            SqlCommand command = new SqlCommand();
            // connect command to connection
            command.Connection = sqlConnection;
            command.CommandText = "UPDATE dbo.Notes SET Text = @Text, Priority = @Priority, Tag = @Tag, UserId = @UserId WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Text", entity.Text);
            command.Parameters.AddWithValue("@Priority", entity.Priority);
            command.Parameters.AddWithValue("@Tag", entity.Tag);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.ExecuteNonQuery();
            sqlConnection.Close();

        }
    }
}
