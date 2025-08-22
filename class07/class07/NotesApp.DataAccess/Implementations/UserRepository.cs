using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations
{
    public class UserRepository : IRepository<User>
    {
        private NotesAppDbCpntext _notesAppDbcontext;
        public UserRepository(NotesAppDbCpntext notesAppDbcontext)
        {
            _notesAppDbcontext = notesAppDbcontext;
        }
        public void Add(User entity)
        {
            _notesAppDbcontext.Users.Add(entity);
            _notesAppDbcontext.SaveChanges();
        }
        public void Delete(int id)
        {
            _notesAppDbcontext.Users.Remove(GetById(id));
            _notesAppDbcontext.SaveChanges();
        }
        public List<User> GetAll()
        {
            return _notesAppDbcontext.Users.ToList();
        }
        public User GetById(int id)
        {
            return _notesAppDbcontext.Users.FirstOrDefault(x => x.Id == id);
        }
        public void Update(User entity)
        {
            _notesAppDbcontext.Users.Update(entity);
            _notesAppDbcontext.SaveChanges();
        }
    }
   
}
