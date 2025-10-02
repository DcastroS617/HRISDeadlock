using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class TransportsBll : ITransportsBll<TransportEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ITransportsDal<TransportEntity> TransportsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public TransportsBll(ITransportsDal<TransportEntity> objDal)
        {
            TransportsDal = objDal;
        }

        /// <summary>
        /// List the Transports enabled
        /// </summary>
        /// <returns>The Transports</returns>
        public List<TransportEntity> ListEnabled()
        {
            try
            {
                return TransportsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        /// <returns>Tuple: En the first item a bool: true if Transport successfully added. False otherwise
        /// Second item: the Transport added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, TransportEntity> Add(TransportEntity entity)
        {
            try
            {
                TransportEntity previousEntity = TransportsDal.ListByDescription(entity.TransportDescriptionSpanish, entity.TransportDescriptionEnglish);

                if (previousEntity == null)
                {
                    short TransportCode = TransportsDal.Add(entity);
                    entity.TransportCode = TransportCode;
                    return new Tuple<bool, TransportEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, TransportEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>       
        public Tuple<bool, TransportEntity> Edit(TransportEntity entity)
        {
            try
            {
                TransportEntity previousEntity = TransportsDal.ListByDescription(entity.TransportDescriptionSpanish, entity.TransportDescriptionEnglish);

                if (previousEntity == null || previousEntity?.TransportCode==entity.TransportCode || previousEntity?.Deleted==false)
                {
                    TransportsDal.Edit(entity);
                    return new Tuple<bool, TransportEntity>(true, null);
                }
                else
                {
                    return new Tuple<bool, TransportEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        public void Delete(TransportEntity entity)
        {
            try
            {
                TransportsDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        public void Activate(TransportEntity entity)
        {
            try
            {
                TransportsDal.Activate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the Transport By key
        /// </summary>
        /// <param name="TransportCode">The Transport code</param>
        /// <returns>The transport</returns>
        public TransportEntity ListByKey(short TransportCode)
        {
            try
            {
                return TransportsDal.ListByKey(TransportCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the Transport by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="transportDescriptionSpanish">The Transport description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport description english</param>
        /// <param name="transportTypeCode">The transport type code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The Transport meeting the given filters and page config</returns>
        public PageHelper<TransportEntity> ListByFilters(int divisionCode, string transportDescriptionSpanish, string transportDescriptionEnglish, byte? transportTypeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<TransportEntity> pageHelper = TransportsDal.ListByFilters(divisionCode
                    , transportDescriptionSpanish
                    , transportDescriptionEnglish
                    , transportTypeCode
                    , sortExpression
                    , sortDirection
                    , pageNumber
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
                    throw new BusinessException(msgTransportsList, ex);
                }
            }
        }

        /// <summary>
        /// List the Transport By the spanish o english Description
        /// </summary>
        /// <param name="transportDescriptionSpanish">The Transport Description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport Description english</param>
        /// <returns>The Transport </returns>
        public TransportEntity ListByDescription(string transportDescriptionSpanish, string transportDescriptionEnglish)
        {
            try
            {
                return TransportsDal.ListByDescription(transportDescriptionSpanish, transportDescriptionEnglish);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgTransportsListByKey, ex);
                }
            }
        }
    }
}