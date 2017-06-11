using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<T> AsObservableCollection<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }

        public static void UpdateCollection<T>(this ObservableCollection<T> collection, T model, ModelUpdateEvent updateEvent) where T : ModelBase
        {
            T existing = collection.FirstOrDefault(x => x.Id == model.Id);
            switch (updateEvent)
            {
                case ModelUpdateEvent.Created:
                case ModelUpdateEvent.Updated:
                    if (existing != null)
                    {
                        collection.Remove(existing);
                    }
                    collection.Add(model); //NOTE: don't worry about inserting record in original position - caller must filter and order
                    break;

                case ModelUpdateEvent.Deleted:
                    if (existing != null)
                    {
                        collection.Remove(existing);
                    }
                    break;
            }
        }
    }
}