using Mimir.Core.Models;
using System.Collections.Generic;

namespace Mimir.Kanban
{
    public interface IIndexableHelper
    {
        void ReorderIndexable(IEnumerable<IIndexable> indexables, int oldIndex, int newIndex);
        void MoveIndexable(IEnumerable<IIndexable> indexables, IIndexable @new, int newIndex);
    }
}
