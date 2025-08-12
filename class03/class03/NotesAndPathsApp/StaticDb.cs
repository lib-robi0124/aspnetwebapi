using NotesAndTagssApp.Models;
using System.Collections.Generic;
using System.Drawing;

namespace NotesAndTagssApp
{
    public static class StaticDb
    {
        public static List<Note> Notes = new List<Note>()
        {
            new Note(){Text = "Do Homework", Priority = Models.Enums.Priority.High, Tags = new List<Tag>
                {
                    new Tag(){ Name ="Homework", Color = "cyan" },
                    new Tag(){ Name = "Avenga", Color = "blue"}
                }
            },
             new Note(){Text = "Drink more water", Priority = Models.Enums.Priority.Medium, Tags = new List<Tag>
                {
                    new Tag(){ Name ="Healthy", Color = "orange" },
                    new Tag(){ Name = "Priority High", Color = "red"}
                }
            },
              new Note(){Text = "Go to the gym", Priority = Models.Enums.Priority.Low, Tags = new List<Tag>
                {
                    new Tag(){ Name ="exercise", Color = "blue" },
                    new Tag(){ Name = "Priority Medium", Color = "yelow"}
                }
            },

        };
    }
}
