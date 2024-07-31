using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class MockCommandAPIRepo : ICommandAPIRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            List<Command> commands = new List<Command>
            {
                new Command
                {
                    Id=0,
                    HowTo="How to generate a migration",
                    CommandLine="dotnet ef migations add <Name of migation>",
                    Platform=".Net Core EF"
                },
                new Command
                {
                    Id=1,
                    HowTo="Run migrations",
                    CommandLine="dotnet ef database update",
                    Platform=".Net Core EF"
                },
                new Command
                {
                    Id=2,
                    HowTo="List active migrations",
                    CommandLine="dotnet ef migrations list",
                    Platform=".Net Core EF"
                },
            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command
                {
                    Id=0,
                    HowTo="How to generate a migration",
                    CommandLine="dotnet ef migations add <Name of migation>",
                    Platform=".Net Core EF"
                };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}