using Mimir.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mimir.Kanban
{
    public class IndexableHelper : IIndexableHelper
    {
        private void RemapIndexes(IEnumerable<IIndexable> indexables, int startingIndex = 0)
        {
            foreach (var indexable in indexables.OrderBy(x => x.Index))
            {
                indexable.Index = startingIndex++;
            }
        }

        public void ReorderIndexable(IEnumerable<IIndexable> indexables, int oldIndex, int newIndex)
        {
            var toMove = indexables.FirstOrDefault(x => x.Index == oldIndex);

            indexables.Where(x => x.Index >= newIndex)
                .ToList()
                .ForEach(x => x.Index++);
            toMove.Index = newIndex;

            RemapIndexes(indexables);
        }

        public void MoveIndexable(IEnumerable<IIndexable> indexables, IIndexable @new, int newIndex)
        {
            indexables.Where(x => x.Index >= newIndex)
           .ToList()
           .ForEach(x => x.Index++);

            @new.Index = newIndex;

            RemapIndexes(indexables.Concat(new[] { @new }));
        }
    }
}
