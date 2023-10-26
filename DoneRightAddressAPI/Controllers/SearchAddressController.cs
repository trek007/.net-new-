using DoneRightAddressAPI.BLL.Repository.Interface;
using DoneRightAddressAPI.CustomAttributes;
using DoneRightAddressAPI.DTO.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace DoneRightAddressAPI.Controllers
{
    [Route("api/[controller]")]



    public class SearchAddressController : ControllerBase
    {
        #region Fields
        private readonly ISearchAddress _searchAddress;
        #endregion

        #region Constructor
        public SearchAddressController(ISearchAddress searchAddress)
        {
            _searchAddress = searchAddress;
        }
        #endregion

        #region Methods

        /// <summary>
        /// To Get Data From Search Address
        /// <param name="SearchAddress"/>
        /// <param name="County"/>
        /// <param name="Zip"/>
        /// </summary>
        ///<returns>Returns data from database through SearchAddress</returns>
        [HttpGet("GetAddressSearch")]
        [@AuthorizeRequest]
        public ActionResult<List<SearchAddress_PropertyModel>> GetDataFromSearchAddress([FromQuery] string address, [FromQuery] string County, [FromQuery] string zipcod = "")
        {
            try
            {

                List<SearchAddress_PropertyModel> searchAddress_PropertyModel = new List<SearchAddress_PropertyModel>();
                if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(County))
                {
                    searchAddress_PropertyModel = _searchAddress.GetDataFromSearchAddress(County, address, zipcod);
                }
                return searchAddress_PropertyModel;
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
        /// <summary>
        /// To Get Data From Search Address BY APN
        /// <param name="APN"/>
        /// <param name="County"/>
        /// </summary>
        ///<returns>Returns data from database through APN and COunty</returns>
        [HttpGet("GetAddressSearchByAPN")]
        [@AuthorizeRequest]
        public ActionResult<List<SearchAddress_PropertyModel>> GetAddressSearchByAPN([FromQuery] string APN, [FromQuery] string County)
        {
            try
            {

                List<SearchAddress_PropertyModel> searchAddress_PropertyModel = new List<SearchAddress_PropertyModel>();
                if (!string.IsNullOrEmpty(APN) && !string.IsNullOrEmpty(County))
                {
                    searchAddress_PropertyModel = _searchAddress.GetAddressSearchByAPN(County, APN);
                }
                return searchAddress_PropertyModel;
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
        /// <summary>
        /// To Get Details From Nassau Apn
        /// <param name="Nassau Apn"/>
        /// </summary>
        ///<returns>Returns data from database through SearchAddress</returns>
        [HttpGet]
        [Route("GetDesignationFieldsNassau/{apn}")]
        [@AuthorizeRequest]
        public ActionResult<NassauDesignation_PropertyModel> GetNassauApnDetailsForAuthDocs(string apn)
        {
            try
            {
                return _searchAddress.GetNassauApnDetailsForAuthDocs(apn);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
        /// <summary>
        /// To Get Details From Suffolk Apn
        /// <param name="Suffolk Apn"/>
        /// </summary>
        ///<returns>Returns data from database through SearchAddress</returns>
        [HttpGet]
        [Route("GetDesignationFieldsSuffolk/{apn}")]
        [@AuthorizeRequest]
        public ActionResult<SuffolkDesignation_PropertyModel> GetSuffolkApnDetailsForAuthDocs(string apn)
        {
            try
            {
                return _searchAddress.GetSuffolkApnDetailsForAuthDocs(apn);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
        [Route("/"), HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult APIlandigPage()
        {
            return new EmptyResult();
        }
        private string Clean_SalesValues(string strvalue)
        {
            //***************************************
            //This function removes chars from the passed in number (house or apt)
            //This is needed so the search is consistent with our data.
            //The return string is the cleaned data
            //***************************************

            //REMOVE STAR

            return strvalue.Replace("*", "");
        }
        private string Clean_AddressNumber(string address)
        {
            //***************************************
            //This function removes chars from the passed in number (house or apt)
            //This is needed so the search is consistent with our data.
            //The return string is the cleaned data
            //***************************************

            //REMOVE DASH
            return address.Replace("-", "");
        }
        #endregion
    }
}
