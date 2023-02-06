using System.Collections.Generic;

namespace Implementation.Data
{
    public interface ISaveAndLoadData<TData>
    {
        /// <summary>
        /// Saves data to data context.
        /// </summary>
        void SaveData(TData data);

        /// <summary>
        /// Load data from data context.
        /// </summary>
        IList<TData> LoadAllData();

        /// <summary>
        /// Loads specific data.
        /// </summary>
        /// <param name="id">Unique indexer.</param>
        /// <returns></returns>
        TData LoadSpecificData(string id);
    }
}
