using Dapper;
using Microsoft.Data.SqlClient;
using NotesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace NotesApp.DataAccess.DapperImplentations
{
    public class NoteDapperRepository : IRepository<Note>
    {
        // Implementation of Note repository using Dapper
        private string _connectionString;
        public NoteDapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string insertQuery = "INSERT INTO dbo.Notes(Text, Priority, Tag, UserId) VALUS(@text, @priority, @tag, @userId)";

                sqlConnection.Query(insertQuery, new
                {
                    text = entity.Text,
                    priority = entity.Priority,
                    tag = entity.Tag,
                    userId = entity.UserId
                });
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string deleteQuery = "DELETE FROM dbo.Notes WHERE Id=@id";
                sqlConnection.Execute(deleteQuery, new { id = id });
            }
        }

        public List<Note> GetAll()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                List<Note> notesDb = sqlConnection.Query<Note>("SELECT * FROM dbo.Notes").ToList();
                return notesDb;
            }
        }

        public Note GetById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                Note note = sqlConnection.Query<Note>("SELECT * FROM dbo.Notes WHERE Id=@NoteId", new { NoteId = id }).FirstOrDefault();

                return note;
            }
        }

        public void Update(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string updateQuery = "UPDATE dbo.Notes SET Text = @text, Tag = @tag, Priority = @priority, UserId = @userId WHERE Id=@id";

                sqlConnection.Query(updateQuery, new
                {
                    text = entity.Text,
                    priority = entity.Priority,
                    tag = entity.Tag,
                    userId = entity.UserId,
                    id = entity.Id
                });
            }
        }
    }
}
