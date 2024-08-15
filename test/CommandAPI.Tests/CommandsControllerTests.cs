using CommandAPI.Controllers;
using CommandAPI.Data;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests:IDisposable
    {
        Mock<ICommandAPIRepo> _mockRepo;
        CommandsProfile _realProfile;
        MapperConfiguration _configuration;
        IMapper _mapper;

        public CommandsControllerTests()
        {
            _mockRepo = new Mock<ICommandAPIRepo>();
            _realProfile = new CommandsProfile();
            _configuration = new MapperConfiguration(cfg=>cfg.AddProfile(_realProfile));
            _mapper = new Mapper(_configuration);
        }

        public void Dispose()
        {
            _mockRepo = null;
            _realProfile = null;
            _configuration = null;
            _mapper = null;
        }

        [Fact]
        public void GetCommandItems_Returns200OK_WhenDBIsEmpty()
        {
            //arrange
            _mockRepo.Setup(repo=> repo.GetAllCommands()).Returns(GetCommands(0));
            //var realProfile = new CommandsProfile();
            //var configuration = new MapperConfiguration(cfg=>cfg.AddProfile(realProfile));
            //IMapper mapper = new Mapper(configuration);

            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //act
            var result = controller.GetAllCommands();

            ////assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            _mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController (_mockRepo.Object, _mapper);

            var result = controller.GetAllCommands();

            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;
            
            Assert.Single(commands);
        }

        [Fact]
        public void GetAllCommands_Returns200OK_WhenDbHasOneResource()
        {
            _mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.GetAllCommands();

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDbHasOneResource()
        {
            _mockRepo.Setup(repo=>repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(_mockRepo.Object, _mapper);
            var result = controller.GetAllCommands();

            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        [Fact]
        public void GetCommandById_Returns404NotFound_WhenNonExistentIdProvided()
        {
            _mockRepo.Setup(repo=>repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.GetCommandById(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_Returns200OK_WhenValidIdProvided()
        {
            _mockRepo.Setup(repo=>repo.GetCommandById(1)).Returns(new Command {  Id = 1, HowTo="mock", Platform="mock", CommandLine="mock" });
            var controller = new CommandsController (_mockRepo.Object, _mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_ReturnsActionResultCommandReadDto_WhenValidIdProvided()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "mock", CommandLine = "mock" });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "mock", CommandLine = "mock" });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.CreateCommand(new CommandCreateDto());

            Assert.IsType<ActionResult<CommandReadDto>>(result); 
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "mock", CommandLine = "mock" });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.CreateCommand(new CommandCreateDto());

            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "mock", CommandLine = "mock" });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.UpdateCommand(1, new CommandUpdateDto());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIdSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(()=>null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.UpdateCommand(1, new CommandUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Patch_PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIdSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(() => null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.PartialCommandUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto> {  });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidResourcIdSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "mock", CommandLine = "mock" });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.DeleteCommand(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns404NotFound_WhenNonExistentResourceIdSubmitted()
        {
            _mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(()=>null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.DeleteCommand(1);

            Assert.IsType<NotFoundResult>(result);
        }

        #region private utility functions
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(
                    new Command
                    {
                        Id = 0,
                        HowTo = "How to generate a migration",
                        CommandLine = "dotnet ef migrations add <name of migration>",
                        Platform = ".Net Core EF"
                    });
            }
            return commands;
        } 
        #endregion

    }
}
