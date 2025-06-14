using System;

namespace Teklif_Hazırlayıcı.DataAccess
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Inserts a new entity and returns its generated identifier.
        /// </summary>
        int Insert(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        void Update(T entity);
    }
}
