using Mimir.Core.Models;
using System.Collections.Generic;

namespace Mimir.Kanban
{
    public interface IIndexableHelper
    {
        void RemapIndexes(IEnumerable<IIndexable> indexables, int startingIndex = 0);
        void MoveIndexable(IEnumerable<IIndexable> indexables, int oldIndex, int newIndex);
        void AddIndexable(IEnumerable<IIndexable> indexables, IIndexable @new, int newIndex);
    }
}
