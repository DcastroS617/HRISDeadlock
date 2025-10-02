using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Configuration;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class EmployeeWorkBll : IEmployeeWorkBll
    {
        private readonly IEmployeeWorkDal EmployeeWorkDal;

        public EmployeeWorkBll(IEmployeeWorkDal obj)
        {
            EmployeeWorkDal = obj;
        }

        public PageHelper<EmployeeWorksEntity> EmployeeWorksListByFilter(EmployeeWorksEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = EmployeeWorkDal.EmployeeWorksListByFilter(entity
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

        public EmployeeWorksEntity EmployeeWorkDetail(EmployeeWorksEntity entity)
        {
            try
            {
                return EmployeeWorkDal.EmployeeWorkDetail(entity);
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

        public ListItem[] EmployeeWorkListByEnable()
        {
            try
            {
                return EmployeeWorkDal.EmployeeWorkListByEnable();
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

        public DbaEntity EmployeeWorkAdd(EmployeeWorksEntity entity)
        {
            try
            {
                return EmployeeWorkDal.EmployeeWorkAdd(entity);
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

        public DbaEntity EmployeeWorkEdit(EmployeeWorksEntity entity)
        {
            try
            {
                return EmployeeWorkDal.EmployeeWorkEdit(entity);
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

        public DbaEntity EmployeeWorkDesactivate(EmployeeWorksEntity entity)
        {
            try
            {
                return EmployeeWorkDal.EmployeeWorkDesactivate(entity);
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
