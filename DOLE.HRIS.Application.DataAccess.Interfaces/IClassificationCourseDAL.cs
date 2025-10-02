using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IClassificationCourseDal
    {
        DbaEntity ClassificationCourseAdd(ClassificationCourseEntity entity);
        
        PageHelper<ClassificationCourseEntity> ClassificationCourseByFilter(ClassificationCourseEntity entity, string Lang, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
        
        ClassificationCourseEntity ClassificationCourseById(ClassificationCourseEntity entity);
        
        DbaEntity ClassificationCourseDesactivate(ClassificationCourseEntity entity);
        
        DbaEntity ClassificationCourseEdit(ClassificationCourseEntity entity);
    }
}