using DoneRightAddressAPI.DTO.Model;
using System.Collections.Generic;

namespace DoneRightAddressAPI.BLL.Repository.Interface
{
    public interface ISearchAddress
    {
        List<SearchAddress_PropertyModel> GetDataFromSearchAddress(string County, string Address, string Zip);
        List<SearchAddress_PropertyModel> GetAddressSearchByAPN(string County, string APN);
        NassauDesignation_PropertyModel GetNassauApnDetailsForAuthDocs(string apn);
        SuffolkDesignation_PropertyModel GetSuffolkApnDetailsForAuthDocs(string apn);
    }
}
