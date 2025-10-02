using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAbsenteeismFileBll<T> where T : FileEntity
    {
        /// <summary>
        /// List Files from absenteeism
        /// </summary>
        /// <returns></returns>
        List<T> ListByKey(int adamNumberCase, string geographicDivisionCode, int divisionCode);

        /// <summary>
        /// Add File
        /// </summary>
        /// <param name="entity">The Absenteeism File</param>
        void Add(T entity);

        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="entity">The Absenteeism File</param>
        void Delete(T entity);

        /// <summary>
        /// Get specific File
        /// </summary>
        /// <param name="idFile"></param>
        /// <returns></returns>
        List<FileEntity> GetFileByKey(int idFile);

        /// <summary>
        /// Delete all files by AdamNumber Case
        /// </summary>
        /// <param name="adamNumberCase"></param>
        void DeleteAllByAdamNumberCase(int adamNumberCase);
    }
}
