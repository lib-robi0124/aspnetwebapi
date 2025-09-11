using Avenga.NotesApp.Domain.Enums;
using Avenga.NotesApp.Dtos.NoteDtos;
using Avenga.NotesApp.Services.Implementations;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Avenga.NotesApp.Tests.FakeRepositories;

namespace Avenga.NotesApp.Tests
{
    [TestClass]
    public class NoteTests
    {
        [TestMethod]
        public void AddNote_InvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            //we make a new instance with our NoteService from the Services Project
            //But we initialize it with the fake repositories, not the real ones
            //because they communicate with the production Database
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            //we created new note with invalid user Id
            var newNote = new AddNoteDto()
            {
                Priority = Priority.Low,
                Tag = Tag.Work,
                Text = "Do your work!",
                UserId = 3
            };
            // Act & Assert
            //We Check if the application will throw an exception if we use our actual service and
            //try to add new note with invalid user id (user that does not exist!)
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote));

        }
        [TestMethod]
        public void AddNote_EmptyText_Exception()
        {
            //arrange
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            var newNote = new AddNoteDto()
            {
                Priority = Priority.Low,
                Tag = Tag.Work,
                Text = "",
                UserId = 1
            };

            //Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote));
        }
        [TestMethod]
        public void AddNote_LargerText_Exception()
        {
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            var newNote = new AddNoteDto()
            {
                Priority = Priority.Low,
                Tag = Tag.Work,
                Text = new string('a', 1001), // Text with 101 characters
                UserId = 1
            };

            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote));
        }
        [TestMethod]
        public void GetNoteById_NoteDoesNotExist_ThrowsNoteNotFoundException()
        {
            // Arrange
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            int nonExistentNoteId = 999; // Assuming this ID does not exist in the fake repository
            // Act & Assert
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.DeleteNote(nonExistentNoteId));
        }
        [TestMethod]
        public void GetNoteById_ValidUser_NoteDto()
        {
            // Arrange
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            var expectedNoteText = "Do something";
            // Act
            //var result = noteService.GetById(1);
            NoteDto noteDto = noteService.GetById(1);
            // Assert
            Assert.AreEqual(expectedNoteText, noteDto.Text);
        }
    }
}
