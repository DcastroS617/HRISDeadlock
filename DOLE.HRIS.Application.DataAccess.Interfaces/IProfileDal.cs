using DOLE.HRIS.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IProfileDal<T> where T : ProfileEntity
    {
        List<T> ProfileListByDivisionCode(ProfileEntity objProfile);

        List<T> ListAll();

        ProfileEntity Single(ProfileEntity serchedProfile);

        void Add(ProfileEntity newProfile, bool isOtherUsers, string userName, string domain);

        void Update(ProfileEntity editedProfile);

        void Delete(ProfileEntity deletedProfile);
    }
}
