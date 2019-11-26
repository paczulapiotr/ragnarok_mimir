using System.Threading.Tasks;
using Mimir.API.Repositories;
using Mimir.CQRS.Commands;

namespace Mimir.API.Commands
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommandHandler.Command>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task HandleAsync(Command command)
        {
            _userRepository.CreateUser(command.AuthId, command.Name);
            return Task.CompletedTask;
        }


        public class Command:ICommand
        {
            public Command()
            {
            }

            public Command(string authId, string name)
            {
                AuthId = authId;
                Name = name;
            }

            public string AuthId { get; }
            public string Name { get; }
        }
    }
}
