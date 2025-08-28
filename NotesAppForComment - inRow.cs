namespace NotesApp.Domain.Models
{    //A base class (or parent class). It's designed to be inherited by other classes to avoid repeating common code.
    //Why it's used: To provide a consistent primary key (Id) for all your database entities (like Note and User)
    //This is a core principle of DRY (Don't Repeat Yourself) programming.
    public class BaseEntity
    {
        // This marks the property as the Primary Key (PK) for EF Core.
        [Key] // PRIMARY KEY: Uniquely identifies each database row. Input: Auto-generated. Output: The unique ID.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configures the ID as an auto-incrementing identity column. 
                                                              // EF Core will auto-generate the ID when inserting into the database.
        public int Id { get; set; } // Core entity property. Input: not set by the user, it's generated automatically.
                                    // Output: System-generated unique identifier.
                                    //When an entity is saved to the database, the database generates 
                                    // a unique integer Id for it, which is then stored in this property.
   }
}
namespace NotesApp.Domain.Models
{
    public class Note : BaseEntity // Inherits the 'Id' property from BaseEntity.
    {
        // A Domain Model class. It represents a real-world concept (a Note) in your application and maps directly to a database table.
        // Why it's used: This is the central object your application works with. It defines the structure of the data for a Note.
        public string Text { get; set; } // The main content of the note. Input: content written by the user. Output: persisted note text,To client/DB.
        public Priority Priority { get; set; } // Input as an enum value, Enum for note importance. Input: From user. 
                                               // Output:Safer than using numbers/strings, To client/DB.
        public Tag Tag { get; set; } // Enum for note category. Input: From user. Output: To client/DB.
        public int UserId { get; set; } // FOREIGN KEY: Links to the User who owns this note. Input: From user/client. 
                                        // Output: EF Core uses it for relationship mapping. To DB for relationship.
        public User User { get; set; } // NAVIGATION PROPERTY: Allows loading the related User object. Not input/output; used internally by EF Core.
        //It's used by Entity Framework Core to load the related User object when needed (e.g., when you want to display the user's name along with the note).
    }
}
namespace NotesApp.Domain.Models
{
    public class User : BaseEntity // Inherits the 'Id' property from BaseEntity.
    {
        // Another Domain Model class, representing a user in the system.
        // Why it's used: To store user information and define a one-to-many relationship with the Note class (one User has many Notes).
        public string? FirstName { get; set; } // Nullable (optional). User's first name. Input: From user. Output: To client/DB. (Nullable)
        public string? LastName { get; set; } // Nullable. User's last name. Input: From user. Output: To client/DB. (Nullable)
        public string Username { get; set; } // User's identifier. Input: From user. Output: To client/DB. (Required, configured later in DbContext).
        public List<Note> Notes { get; set; } // NAVIGATION PROPERTY: Collection of notes owned by this user. Not input/output; used internally.
    }
}
namespace NotesApp.Domain.Enums
{
    // An enumeration (enum). A special type that defines a set of named constants.
    // Why it's used: To restrict a property to a specific, predefined set of values. 
   public enum Priority { Low = 1, Medium, High }
   public enum Tag { Work = 1, Health, SocialLife }
}
namespace NotesApp.DataAccess
{
    // INTERFACE: Defines "rules" or a "contract".
    // No implementation here, just method signatures.
    // Benefit: We can swap implementations without changing other code.
    // Why it's used: Abstraction: It hides the complex details of how data is accessed (e.g., SQL queries, database connections).
    // Loose Coupling: The rest of your application (like Services) depends on this interface, not a specific implementation. 
    // This makes your code flexible and easy to test. You could swap out the database (e.g., from SQL Server to PostgreSQL) 
   // and only need to change the repository implementation, not the service logic.
    public interface IRepository<T> where T : class // GENERIC INTERFACE: Defines a contract for all repository classes.
    {
        T GetById(int id); // Input: ID. Output: A single entity of type T.
        List<T> GetAll(); // Input: None. Output: A list of all entities of type T.
        void Add(T entity); // Input: An entity object. Output: None (saves to DB).
        void Update(T entity); // Input: An updated entity object. Output: None (saves changes to DB).
        void Delete(int id); // Input: An ID. Output: None (removes entity from DB).
    }
}
namespace NotesApp.DataAccess.Implementations
{
    // ... implementation of IRepository<Note> methods ...
    // Implements IRepository<Note>, so this class gives the real logic.
    // Why it's used: To contain all the data access logic for the Note entity. It uses the DbContext to talk to the database.
    public class NoteRepository : IRepository<Note> // CONCRETE REPOSITORY: Implements data access operations for the Note entity.
    {
        private NotesAppDbCpntext _notesAppDbcontext; // DEPENDENCY: The DbContext used to communicate with the database. Injected via constructor.
        public NoteRepository(NotesAppDbCpntext notesAppDbcontext) // CONSTRUCTOR: Dependency Injection (DI) provides the DbContext here.
        {
            _notesAppDbcontext = notesAppDbcontext; // Injected by DI
        }
        public List<Note> GetAll()
        {
            // Input: none → Output: all notes including related user
            return _notesAppDbcontext.Notes.Include(x => x.User).ToList(); // EAGER LOADING: Fetches Notes and their related User data in one query. Output: List<Note>.
        }
        public void Add(Note entity)
        {
            _notesAppDbcontext.Notes.Add(entity); // Add note
            _notesAppDbcontext.SaveChanges();     // Commit to DB
        }
        public Note GetById(int id)
        {
            // Input: id → Output: single Note + User
            return _notesAppDbcontext.Notes
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }
        public void Delete(int id)
        {
            _notesAppDbcontext.Notes.Remove(GetById(id));
            // Reuses GetById (input: id → output: Note)
            _notesAppDbcontext.SaveChanges();
        }
        public void Update(Note entity)
        {
            _notesAppDbcontext.Notes.Update(entity);
            _notesAppDbcontext.SaveChanges(); // Save changes to DB
        }
        // This keeps database code organized and separate from business logic.
        // Key Concept: The .Include(x => x.User) method is part of Eager Loading. It tells Entity Framework to automatically 
        // fetch the related User data in the same database query, preventing errors when you later try to access note.User.FirstName.
    }
}
namespace NotesApp.DataAccess
{
    // The heart of Entity Framework Core. It represents a session with the database.
    // Why it's used: Querying: Allows you to write LINQ queries against DbSet<T> which are translated into SQL.
   // Change Tracking: Keeps track of changes made to entities so it knows what to update.
   // Persistence: Saves entities to the database (SaveChanges).
   // Database Configuration: The OnModelCreating method is where you define your database schema 
   // (rules, relationships, constraints) using Fluent API, like making Text required and setting its maximum length.
    public class NotesAppDbCpntext : DbContext // Inherits DB CONTEXT: The central class for interacting with the database using EF Core.
    {
        public NotesAppDbCpntext(DbContextOptions options) : base(options) { }
        public DbSet<Note> Notes { get; set; } // Represents the 'Notes' table in the database.
        public DbSet<User> Users { get; set; } // Represents the 'Users' table in the database.
        protected override void OnModelCreating(ModelBuilder modelBuilder) // FLUENT API: Used to configure database schema, rules, and relationships.
        {
            base.OnModelCreating(modelBuilder);
            #region NoteConfig
            // Note
            //Constaints za text propertito
            modelBuilder.Entity<Note>()
                .Property(e => e.Text)
                .HasMaxLength(100)
                .IsRequired(); // CONFIGURATION: Defines a maximum length and makes the field mandatory (NOT NULL) in the DB.
            modelBuilder.Entity<Note>()
                .Property(x => x.Priority)
                .IsRequired();
            modelBuilder.Entity<Note>()
                .Property(x => x.Tag)
                .IsRequired();
            // Relation: Note → User (many notes per user)
            modelBuilder.Entity<Note>()
                .HasOne(x => x.User)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.UserId); // RELATIONSHIP: Configures the one-to-many relationship between User and Note.
            #endregion

            #region UserConfig
            //USER
            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .HasMaxLength(30)
                .IsRequired();

            //modelBuilder.Entity<User>()
            //    .Ignore(x => x.Age);
            #endregion
            #region SeedData
            // SEED if needed
            #endregion
        }
    }
}
namespace NotesApp.Dtos.NoteDtos
{
    // Simple classes designed to carry data between layers of an application, especially between the API controller and the client.
//  Why it's used: Security: Prevents over-posting attacks. You expose only the properties you want the client to send/receive
//  (e.g., you wouldn't want a client to be able to send a UserFullName).
// Decoupling: The API contract (what data is sent/received) is decoupled from the internal domain model.
//  You can change your Note class without necessarily breaking your API.
// Convenience: They can shape data specifically for the client's needs (e.g., NoteDto combines note data with UserFullName in one object).
    public class NoteDto // DATA TRANSFER OBJECT (DTO): For outputting data to the client.
    {
        public string Text { get; set; } // Output from service/controller to client.
        public Priority Priority { get; set; } // Output from service/controller to client.
        public Tag Tag { get; set; } // Output from service/controller to client.
        public string UserFullName { get; set; } // Output: A calculated field not in the Domain Model, 
                                                 // created for client convenience. maps User FirstName + LastName.
    }
    public class AddNoteDto // DTO: For receiving data from the client to create a new note.
    {
        public string Text { get; set; } // Input from client to service.
        public int UserId { get; set; } // Input: user owning the note
        public Priority Priority { get; set; } // Input from client to service.
        public Tag Tag { get; set; } // Input from client to service.
    }
}
namespace NotesApp.Services.Interfaces
{
    // Service interface hides repository complexity.
    // The Service Layer. It contains the core business logic and orchestrates interactions between repositories.
    // Why it's used: Business Logic: It encapsulates rules and workflows 
    // (e.g., validation that text is not empty or too long, checking if a user exists before adding a note).
   // Abstraction: It provides a clean, high-level API for the controllers to use, hiding the complexity of using multiple repositories and mapping data.
  // Reusability: The same service logic can be used by different controllers (e.g., a web API controller and a MVC controller).
    public interface INoteService // SERVICE INTERFACE: Defines the business logic contract.
    {
        List<NoteDto> GetAllNotes(); // Output: List of NoteDtos.
        NoteDto GetById(int id); // Input: ID. Output: A NoteDto.
        void AddNote(AddNoteDto addNoteDto); // Input: AddNoteDto.
        void UpdateNote(UpdateNoteDto noteDto); // Input: UpdateNoteDto.
        void DeleteNote(int id); // Input: ID.
    }
}
namespace NotesApp.Services.Implementations
{
    public class NoteService : INoteService // CONCRETE SERVICE: Implements business logic and orchestrates between repos and mappers.
    {
        private readonly IRepository<Note> _noteRepository; // DEPENDENCY: Injected repository for Notes.
        private readonly IRepository<User> _userRepository; // DEPENDENCY: Injected repository for Users.
        public NoteService(IRepository<Note> noteRepository, IRepository<User> userRepository) // CONSTRUCTOR: DI provides the repositories.
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public void AddNote(AddNoteDto addNoteDto)
        {
            // BUSINESS LOGIC/VALIDATION: Checks rules before acting on data.Validate the input
            User userDb = _userRepository.GetById(addNoteDto.UserId);
            if (userDb == null) { throw new NoteDataException($"User with id{addNoteDto.UserId}"); } // VALIDATION: Check user exists.
            if (string.IsNullOrEmpty(addNoteDto.Text)) { throw new NoteDataException("Note text cannot be empty."); } // VALIDATION: Check text.

            Note newNote = addNoteDto.ToNote(); // MAPPING: Convert the input DTO to a Domain Model entity.
            newNote.User = userDb; // ASSIGNMENT: Link the validated user to the new note.

            _noteRepository.Add(newNote); // PERSISTENCE: Send the validated and mapped entity to the repository to be saved.
        }
        public void DeleteNote(int id)
        {
            //1. Get the note by id
            Note noteDb = _noteRepository.GetById(id);
            if (noteDb == null) throw new NoteNotFoundException($"Note with id {id} not found.");

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
namespace NotesApp.Helpers
{
    // STATIC CLASS: Cannot be instantiated (no new object).
    // Why? Only used as a utility for registering dependencies.
    // To keep the Program.cs file clean and organized by encapsulating all the code that registers your dependencies 
// (DbContext, Repositories, Services) with the built-in Inversion of Control (IoC) Container.
// Key Concept:
// AddScoped: Creates one instance per HTTP request. Perfect for DbContext and Repositories, as you want them to share the same instance throughout a single request.
// AddTransient: Creates a new instance every time it's requested.
    public static class DependencyInjectionHelper // STATIC CLASS: A utility for organizing Dependency Injection registration.
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            // Input: IServiceCollection (DI container).
            // Output: DbContext registered in DI for EF Core.
            services.AddDbContext<NotesAppDbCpntext>(options =>
                options.UseSqlServer("Server=...DB Connection String...")); // REGISTRATION: Registers DbContext with SQL Server provider.
                                                                            // Register repositories
            services.AddScoped<IRepository<Note>, NoteRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
        }
        public static void InjectRepositories(IServiceCollection services)
        {
            // Alternative registration (Scoped vs Transient)
            services.AddTransient<IRepository<Note>, NoteRepository>(); // REGISTRATION: Maps interface to concrete implementation.
            services.AddTransient<IRepository<User>, UserRepository>();

        }
        public static void InjectServices(IServiceCollection services)
        {
            // Register services
            services.AddTransient<INoteService, NoteService>();
        }
    }
}
namespace NotesApp.Mapers
{
    // A static class containing extension methods.
    // Why??: To centralize the logic for converting between Domain Models (Note) and DTOs (NoteDto). This is crucial for separating concerns. 
    // The service calls these methods instead of having mapping logic cluttering its code.
   // How: The this Note note parameter makes it an extension method, allowing you to call it neatly like: myNote.ToNoteDto().
    public static class NoteMaper
    {
        public static NoteDto ToNoteDto(this Note note)
        {
            return new NoteDto
            {
                Tag = note.Tag,
                Priority = note.Priority,
                Text = note.Text,
                UserFullName = $"{note.User.FirstName} {note.User.LastName}",
            };
        }
        public static Note ToNote(this AddNoteDto addNoteDto)
        {
            return new Note
            {
                Text = addNoteDto.Text,
                Priority = addNoteDto.Priority,
                Tag = addNoteDto.Tag,
                UserId = addNoteDto.UserId

            };
        }
        public static Note ToNote(this UpdateNoteDto noteDto, Note noteDb)
        {
            noteDb.Text = noteDto.Text;
            noteDb.Priority = noteDto.Priority;
            noteDb.Tag = noteDto.Tag;
            return noteDb;
        }
    }
}
namespace NotesApp.Controllers
{
    // An API Controller. It handles HTTP requests and responses.
    // Why it's used: It's the entry point to your application from the web. Its job is to:
    // Receive HTTP requests.
    // Call the appropriate method on the service layer.
    // Translate the results from the service into HTTP responses (e.g., Ok(notes), NotFound()).
    // Handle exceptions and return the appropriate HTTP status code (e.g., BadRequest for validation errors).
    [ApiController]
    public class NotesController : ControllerBase // API CONTROLLER: Handles HTTP requests and responses.
    {
        private readonly INoteService _noteService; // DEPENDENCY: The service layer, injected via DI.
        public NotesController(INoteService noteService) // CONSTRUCTOR: DI provides the service.
        {
            _noteService = noteService;
        }
        [HttpGet]
        public ActionResult<List<NoteDto>> GetAllNotes() // HTTP GET ENDPOINT.
        {
            try
            {
                var notes = _noteService.GetAllNotes(); // Calls service layer.
                return Ok(notes); // Returns HTTP 200 status code with the data.
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message); // ERROR HANDLING: Returns HTTP 400 for business logic errors.
            }
        }
        [HttpPost]
        public ActionResult AddNote([FromBody] AddNoteDto addNoteDto) // HTTP POST ENDPOINT. Input: Data from request body.
        {
            try
            {
                _noteService.AddNote(addNoteDto); // Calls service to execute business logic.
                return CreatedAtAction(nameof(GetById), addNoteDto); // Returns HTTP 201 (Created) on success.
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message); // Returns HTTP 400 if validation fails.
            }
        }
    }
}
namespace NotesApp.Shared.CustomExceptions
{
    // Custom, specific exception classes that inherit from the base Exception class.
    //  Why it's used: To throw very specific errors from the service layer. 
    // This allows the controller to catch a precise exception type and decide 
    // on the most appropriate HTTP status code to return, making your API more RESTful and easier to debug.
    public class NoteDataException : Exception
    {
        public NoteDataException(string message) : base(message) { }
    }
}
namespace NotesApp.Shared.CustomExceptions
{
    public class NoteNotFoundException : Exception
    {
        public NoteNotFoundException(int id) : base($"Note with ID {id} not found."){ }
        public NoteNotFoundException(string message) : base(message){}
    }
 }
//Program.cs
//  The startup configuration of the application using the Dependency Injection (DI) pattern.
var builder = WebApplication.CreateBuilder(args);    

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject the DbContext and repositories
DependencyInjectionHelper.InjectDbContext(builder.Services);
DependencyInjectionHelper.InjectRepositories(builder.Services);
DependencyInjectionHelper.InjectServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Why it's used: This is where you "wire up" your application. You tell the DI container, 
// "When you see a constructor asking for an INoteService, please provide a NoteService object.
//  And to create a NoteService, you'll need to provide it with a NoteRepository, which needs a DbContext...".
//  The container automatically handles creating and supplying these dependencies, leading to clean, testable, and loosely coupled code.
1. Separation of Concerns (SoC)
This is the single most important principle you've applied. Your code is neatly divided into layers, and each part has a single, well-defined job.

NotesApp.Domain: This is your core business logic and data structure. It knows what a Note and User are, and nothing else. It doesn't know about databases or web requests. This makes your domain model reusable and easy to test.

NotesApp.DataAccess: This layer's only job is to communicate with the database. The IRepository interface abstracts away the complexity, and the concrete NoteRepository handles the details of EF Core, like _notesAppDbcontext.Notes.Include(x => x.User).ToList().

NotesApp.Services: This is where your business rules live. The AddNote method is a perfect example: it validates the input and calls the repository to save the data. It doesn't know how the data is saved, only that it needs to be.

NotesApp.Controllers: The controller is the "receptionist" of your application. It receives requests, delegates the work to the service layer, and returns the result. It doesn't contain business logic, which is exactly how it should be.

NotesApp.Dtos: These are your "data messengers." They protect your application from bad input and ensure you only send the data the client needs. Using a different DTO for adding (AddNoteDto) versus getting all notes (NoteDto) is a great practice.

2. Dependency Injection (DI)
You've used DI throughout, which is a hallmark of maintainable, testable code.

What it is: Instead of a class creating its own dependencies (e.g., a NoteService creating a new NoteRepository()), the dependencies are "injected" into the class through the constructor.

Why it's used: It makes your code loosely coupled. Your NoteService doesn't care if it's getting a NoteRepository that talks to SQL Server, or a mock repository used for testing. This makes unit testing incredibly easy.

Value in/out:

Input: The Program.cs file is the orchestrator. It "configures the container" by telling it what to do. For example, services.AddScoped<IRepository<Note>, NoteRepository>() is a key input to the DI container.

Output: When your NotesController is created, the DI container "outputs" a new NoteService object, and a new NoteRepository object, and a new NotesAppDbCpntext object to satisfy all the constructor requirements. The controller itself is completely unaware of how these objects were created.

3. Abstraction and Interfaces
The use of interfaces like IRepository<T> and INoteService is crucial.

What it is: An interface is a contract. It says, "Any class that implements me must have these methods." It defines what a class does, but not how it does it.

Why it's used: It allows you to program to an interface, not a concrete implementation. This is what allows for the loose coupling we just discussed. If you wanted to switch from SQL Server to a different database, you would just create a new repository class that implements IRepository<T>, and the rest of your code would not need to change.

Areas for Future Growth
Your code is very strong. To take it to the next level, here are a few things to consider:

Mapping: You've created a NoteMaper static class, which is a solid, clean approach. For larger projects, consider a dedicated library like AutoMapper. It automates the mapping between DTOs and domain models, reducing repetitive code.

Asynchronous Methods: For web applications, all data access methods should ideally be asynchronous (e.g., _notesAppDbcontext.Notes.ToListAsync()). This prevents your application from blocking and helps it handle more concurrent requests.

Comments vs. Code: Your comments are excellent and incredibly helpful. As you write more complex code, the goal is often to write code that is so clear it needs fewer comments. You've already made a great start with descriptive variable names and class names.