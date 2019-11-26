using Mimir.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mimir.Kanban
{
    public class IndexableHelper : IIndexableHelper
    {
        public void RemapIndexes(IEnumerable<IIndexable> indexables, int startingIndex = 0)
        {
            foreach (var indexable in indexables.OrderBy(x => x.Index))
            {
                indexable.Index = startingIndex++;
            }
        }

        public void MoveIndexable(IEnumerable<IIndexable> indexables, int oldIndex, int newIndex)
        {
            var toMove = indexables.FirstOrDefault(x => x.Index == oldIndex);
            if (oldIndex > newIndex)
            {
                indexables.Where(x => x.Index >= newIndex)
                        .ToList()
                        .ForEach(x => x.Index++);
            }
            else if (oldIndex < newIndex)
            {
                indexables.Where(x => x.Index <= newIndex)
                        .ToList()
                        .ForEach(x => x.Index--);
            }
            else
            {
                return;
            }

            toMove.Index = newIndex;

            RemapIndexes(indexables);
        }

        public void AddIndexable(IEnumerable<IIndexable> indexables, IIndexable @new, int newIndex)
        {
            indexables.Where(x => x.Index >= newIndex)
           .ToList()
           .ForEach(x => x.Index++);

            @new.Index = newIndex;

            RemapIndexes(indexables.Concat(new[] { @new }));
        }

        public void ReorderIndexable(IEnumerable<IIndexable> indexables)
        {
            throw new System.NotImplementedException();
        }
    }
}
