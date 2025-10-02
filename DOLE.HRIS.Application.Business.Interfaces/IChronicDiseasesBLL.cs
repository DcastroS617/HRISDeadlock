using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IChronicDiseasesBLL<T> where T : ChronicDiseasesEntity
    {
        /// <summary>
        /// List the Chronic Diseases
        /// </summary>
        /// <returns>The ChronicDiseases</returns>
        List<T> ListEnabled();
    }
}
