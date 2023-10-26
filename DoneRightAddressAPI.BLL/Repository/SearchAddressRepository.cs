using DoneRightAddressAPI.BLL.Repository.Interface;
using DoneRightAddressAPI.DAL.Data_Models;
using DoneRightAddressAPI.DTO.Model;
using System;
using System.Collections.Generic;

namespace DoneRightAddressAPI.BLL.Repository
{
    public class SearchAddressRepository : ISearchAddress
    {
        #region Fields
        private readonly SearchAddress _searchAddress;
        #endregion
        #region Constructors
        public SearchAddressRepository()
        {
            _searchAddress = new SearchAddress();
        }
        #endregion

        public List<SearchAddress_PropertyModel> GetDataFromSearchAddress(string County, string Address, string Zip)
        {
            try
            {
                var obj = _searchAddress.GetDataFromSearchAddress(County, Address, Zip);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SearchAddress_PropertyModel> GetAddressSearchByAPN(string County, string APN)
        {
            try
            {
                var obj = _searchAddress.GetAddressSearchByAPN(County, APN);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public NassauDesignation_PropertyModel GetNassauApnDetailsForAuthDocs(string apn)
        {
            try
            {
                return _searchAddress.GetNassauApnDetailsForAuthDocs(apn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SuffolkDesignation_PropertyModel GetSuffolkApnDetailsForAuthDocs(string apn)
        {
            try
            {
                return _searchAddress.GetSuffolkDetailsForAuthDocs(apn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
