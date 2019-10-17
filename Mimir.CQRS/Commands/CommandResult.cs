using System.Collections.Generic;
using System.Linq;

namespace Mimir.CQRS.Commands
{

    public class CommandResult : ICommandResult
    {
        public CommandResult()
        {
        }

        public CommandResult(params string[] errors)
        {
            Errors.AddRange(errors);
        }

        public static CommandResult Success()
        {
            return new CommandResult();
        }

        public static CommandResult Failed(params string[] errors)
        {
            if (errors == null || !errors.Any())
            {
                return new CommandResult("Command has failed");
            }

            return new CommandResult(errors);
        }

        public bool Succeeded => !Errors.Any();

        public List<string> Errors { get; set; } = new List<string>();
    }
}
