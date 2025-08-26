using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations
{
    public class NoteRepository : IRepository<Note>
    {
        private NotesAppDbCpntext _notesAppDbcontext;
        public NoteRepository(NotesAppDbCpntext notesAppDbcontext)
        {
            _notesAppDbcontext = notesAppDbcontext;
        }

        public void Add(Note entity)
        {
            _notesAppDbcontext.Notes.Add(entity);
            _notesAppDbcontext.SaveChanges();
        }

        public void Delete(int id)
        {
            _notesAppDbcontext.Notes.Remove(GetById(id));
            _notesAppDbcontext.SaveChanges();
        }

        public List<Note> GetAll()
        {
            // Include User to get the related user information
            return _notesAppDbcontext.Notes.Include(x => x.User).ToList();
        }

        public Note GetById(int id)
        {
            return _notesAppDbcontext.Notes
                .Include(x => x.User) // Include User to get the related user information
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(Note entity)
        {
            _notesAppDbcontext.Notes.Update(entity);
            _notesAppDbcontext.SaveChanges(); // Save changes to the database, call Db 
        }
    }
}
