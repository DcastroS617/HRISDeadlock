using DOLE.HRIS.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IProfileBll<T> where T : ProfileEntity
    {
        List<ProfileEntity> ProfileListByDivisionCode(int divisionCode);

        List<ProfileEntity> ListAll();

        ProfileEntity Single(byte profileId);

        void Add(string userName, string domain, int divisionCode, string profileDescription, bool isOtherUsers, string lastModifiedUser);

        void Update(byte profileId, int divisionCode, string profileDescription, string lastModifiedUser);

        void Delete(byte profileId);
    }
}

