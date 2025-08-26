using NotesApp.DataAccess;
using NotesApp.Domain.Models;
using NotesApp.Dtos.NoteDtos;
using NotesApp.Mapers;
using NotesApp.Services.Interfaces;
using NotesApp.Shared.CustomExceptions;

namespace NotesApp.Services.Implementations
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<User> _userRepository;

        public NoteService(IRepository<Note> noteRepository, IRepository<User> userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public void AddNote(AddNoteDto addNoteDto)
        {
            // Validate the input
            User userDb = _userRepository.GetById(addNoteDto.UserId);
            if (userDb == null)
            {
                throw new NoteDataException($"User with id{addNoteDto.UserId}");
            }
           if(string.IsNullOrEmpty(addNoteDto.Text))
            {
                throw new NoteDataException("Note text cannot be empty.");
            }
            if(addNoteDto.Text.Length > 100)
            {
                throw new NoteDataException("Note text cannot exceed 100 characters.");
            }
            //2. Map to domain model
            Note newNote = addNoteDto.ToNote();
            newNote.User = userDb;

            //3. Add to repository
            _noteRepository.Add(newNote);
        }
        public void DeleteNote(int id)
        {
            //1. Get the note by id
            Note noteDb = _noteRepository.GetById(id);
            if (noteDb == null)
            {
                throw new NoteNotFoundException($"Note with id {id} not found.");
            }
            //2. Delete from repository
            _noteRepository.Delete(id);
        }
        public List<NoteDto> GetAllNotes()
        {
            //1. Get all notes from repository
            List<Note> notes = _noteRepository.GetAll();
            if (notes == null || notes.Count == 0)
            {
                throw new NoteDataException("No notes found.");
            }
            //2. Map to DTOs
            return notes.Select(note => note.ToNoteDto()).ToList();
        }

        public NoteDto GetById(int id)
        {
            //1. Get the note by id
            Note noteDb = _noteRepository.GetById(id);
            if (noteDb == null)
            {
                throw new NoteNotFoundException(id);
            }
            //2. Map to DTO
            return noteDb.ToNoteDto();
        }
        public void UpdateNote(UpdateNoteDto updateNoteDto)
        {
            //1. Get the note by id - validate existence
            Note noteDb = _noteRepository.GetById(updateNoteDto.Id);
            if (noteDb == null)
            {
                throw new NoteNotFoundException($"Note with id{updateNoteDto.Id} not exist");
            }
            User userDb = _userRepository.GetById(updateNoteDto.UserId);
            if (userDb == null)
            {
                throw new NoteDataException($"User with id {updateNoteDto.UserId} not found.");
            }
            //2. Validate the input
            if (string.IsNullOrEmpty(updateNoteDto.Text))
            {
                throw new NoteDataException("Note text cannot be empty.");
            }
            if (updateNoteDto.Text.Length > 100)
            {
                throw new NoteDataException("Note text cannot exceed 100 characters.");
            }
            //3. Map to domain model
            noteDb.Text = updateNoteDto.Text;
            noteDb.Priority = updateNoteDto.Priority;
            noteDb.Tag = updateNoteDto.Tag;
            noteDb.UserId = updateNoteDto.UserId;
            noteDb.User = userDb;
            //4. Update in repository
            _noteRepository.Update(noteDb);
        }
    }
}
