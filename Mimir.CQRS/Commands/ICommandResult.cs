using System.Collections.Generic;

namespace Mimir.CQRS.Commands
{
    public interface ICommandResult
    {
        bool Succeeded { get; }
        List<string> Errors { get; }
    }
}
