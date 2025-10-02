using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IChronicDiseasesDAL<T> where T : ChronicDiseasesEntity
    {
        /// <summary>
        /// List the  ChronicDiseasesEntity enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();
    
    }
}
