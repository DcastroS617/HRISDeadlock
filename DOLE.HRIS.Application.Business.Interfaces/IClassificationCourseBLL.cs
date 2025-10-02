using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IClassificationCourseBll
    {
        DbaEntity ClassificationCourseAdd(ClassificationCourseEntity entity);
        
        PageHelper<ClassificationCourseEntity> ClassificationCourseByFilter(ClassificationCourseEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber);
        
        ClassificationCourseEntity ClassificationCourseById(ClassificationCourseEntity entity);
        
        DbaEntity ClassificationCourseDesactivate(ClassificationCourseEntity entity);
        
        DbaEntity ClassificationCourseEdit(ClassificationCourseEntity entity);
    }
}