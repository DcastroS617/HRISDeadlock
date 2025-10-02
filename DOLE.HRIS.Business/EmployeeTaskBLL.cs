using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class EmployeeTaskBll : IEmployeeTaskBll
    {
        private readonly IEmployeeTaskDal EmployeeTaskDal;

        public EmployeeTaskBll(IEmployeeTaskDal obj)
        {
            EmployeeTaskDal = obj;
        }

        public PageHelper<EmployeeTaskEntity> EmployeeTaskListByFilter(EmployeeTaskEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = EmployeeTaskDal.EmployeeTaskListByFilter(entity
                    , Divisioncode
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
                    , null
                    , pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public EmployeeTaskEntity EmployeeTaskDetail(EmployeeTaskEntity entity)
        {
            try
            {
                return EmployeeTaskDal.EmployeeTaskDetail(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public ListItem[] EmployeeTaskListByEnabled()
        {
            try
            {
                return EmployeeTaskDal.EmployeeTaskListByEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public List<EmployeeTaskEntity> ListEnabled()
        {
            try
            {
                return EmployeeTaskDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public DbaEntity EmployeeTaskAdd(EmployeeTaskEntity entity)
        {
            try
            {
                return EmployeeTaskDal.EmployeeTaskAdd(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public DbaEntity EmployeeTaskEdit(EmployeeTaskEntity entity)
        {
            try
            {
                return EmployeeTaskDal.EmployeeTaskEdit(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public DbaEntity EmployeeTaskDesactivate(EmployeeTaskEntity entity)
        {
            try
            {
                return EmployeeTaskDal.EmployeeTaskDesactivate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
