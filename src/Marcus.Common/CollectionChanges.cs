using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcus.Common
{
    public class CollectionChanges<T>
    {
        public IEnumerable<T> Deleted { get; set; } = new List<T>();
        public IEnumerable<T> Added { get; set; } = new List<T>();
        public IEnumerable<(T, T)> Updated { get; set; } = new List<(T, T)>();

        public static CollectionChanges<T> FindChanges<TId>(IList<T> previous, IList<T> current,
            Func<T, TId> identitySelector)
        {
            var deleted = previous.Where(x => current.All(y =>
            {
                return !EqualityComparer<TId>.Default.Equals(identitySelector(x), identitySelector(y));
            })).ToList();

            var added = current.Where(x => previous.All(y =>
            {
                return !EqualityComparer<TId>.Default.Equals(identitySelector(x), identitySelector(y));
            })).ToList();
            var updatedNew = current.Where(x => previous.Any(y =>
            {
                return EqualityComparer<TId>.Default.Equals(identitySelector(x), identitySelector(y));
            })).ToList();
            var updatedPrev = previous.Where(x => current.Any(y =>
            {
                return EqualityComparer<TId>.Default.Equals(identitySelector(x), identitySelector(y));
            })).ToList();

            var changes = new CollectionChanges<T>
            {
                Deleted = deleted,
                Added = added,
                Updated = updatedPrev.Select(prev => (prev,
                    updatedNew.Single(newo =>
                    {
                        return EqualityComparer<TId>.Default.Equals(identitySelector(newo), identitySelector(prev));
                    })))
            };

            return changes;
        }
    }
}