using DoneRightAddressAPI.DAL.DBInteractions;
using DoneRightAddressAPI.DAL.Error_Logs;
using DoneRightAddressAPI.DTO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DoneRightAddressAPI.DAL.Data_Models
{
    public class SearchAddress
    {
        #region
        public static ExecuteProcedures _ExecuteProcedures;
        #endregion
        #region Constructor
        public SearchAddress()
        {
            _ExecuteProcedures = new ExecuteProcedures();
        }
        #endregion
        #region Methods

        /// <summary>
        /// To Get Data From Search Address.
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Street"></param>
        /// <param name="Zip"></param>
        /// <returns></returns>
        public List<SearchAddress_PropertyModel> GetDataFromSearchAddress(string County, string Address, string Zip)
        {

            ClsUsers objUser = new ClsUsers();
            conversion convrsn = new conversion();
            List<SearchAddress_PropertyModel> searchAddressObj = new List<SearchAddress_PropertyModel>();
            try
            {
                Tuple<string, string, string> unparsedValues = Unparsed_process(Address);
                string zippy = Zip;
                string num = unparsedValues.Item1;
                string street = unparsedValues.Item2;
                num = convrsn.Clean_AddressNumber(convrsn.Clean_SalesValues(num));
                street = convrsn.Clean_AddressNumber(convrsn.Clean_SalesValues(street));
                street = convrsn.Get_StreetSuffix(street.Replace(",", ""), County);
                if (County.ToLower() == "suffolk")
                {
                    street = street.Replace("st_name", "PropAddressStreet");
                    searchAddressObj = GetDataForSuffolkRegion(num, street, Zip);
                    return searchAddressObj;
                }
                else if (County.ToLower() == "nassau")
                {
                    street = street.Replace("st_name", "PropAddressStreet");
                    searchAddressObj = GetDataForNassauRegion(num, street, Zip);
                    return searchAddressObj;
                }
                return searchAddressObj;
            }
            catch (Exception ex)
            {
                Generate_Exception.WriteErrorLog(ex);
                return new List<SearchAddress_PropertyModel>();
            }
        }
        /// <summary>
        /// To Get Data From Search Address.
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Street"></param>
        /// <param name="Zip"></param>
        /// <returns></returns>
        public List<SearchAddress_PropertyModel> GetAddressSearchByAPN(string County, string APN)
        {
            try
            {
                var dt = _ExecuteProcedures.ReturnDataTable("GetAddressByApnAndCounty",
               new string[] { "@APN", "@County" },
               new string[] { Check_If_NullOrEmpty(APN), Check_If_NullOrEmpty(County) });
                List<SearchAddress_PropertyModel> searchAddress_PropertyModel = new List<SearchAddress_PropertyModel>();
                if (dt.Rows.Count > 0)
                {
                    searchAddress_PropertyModel = (from DataRow row in dt.Rows
                                                   select new SearchAddress_PropertyModel
                                                   {
                                                       County = row["County"] == DBNull.Value ? "" : row["County"].ToString(),
                                                       APN = row["APN"] == DBNull.Value ? "" : row["APN"].ToString(),
                                                       Number = row["PropAddressNumber"] == DBNull.Value ? "" : row["PropAddressNumber"].ToString(),
                                                       OwnerName = row["PropertyOwner"] == DBNull.Value ? "" : row["PropertyOwner"].ToString(),
                                                       Street = row["PropAddressStreet"] == DBNull.Value ? "" : row["PropAddressStreet"].ToString(),
                                                       PropAddress = row["PropAddress"] == DBNull.Value ? "" : row["PropAddress"].ToString(),
                                                       PropAddressFull = row["PropAddressFull"] == DBNull.Value ? "" : row["PropAddressFull"].ToString(),
                                                       Taxes = string.Format("{0:0.##}", row["Taxes"] == DBNull.Value ? "0.00" : row["Taxes"]).ToString(),
                                                       MarketValue = string.Format("{0:0.##}", row["MarketValue"] == DBNull.Value ? "0.00" : row["MarketValue"]).ToString(),
                                                   }).ToList();
                }
                return searchAddress_PropertyModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// To Get Data From Nassau Search Address.
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Street"></param>
        /// <param name="Zip"></param>
        /// <returns></returns>
        public List<SearchAddress_PropertyModel> GetDataForNassauRegion(string Number, string Street, string Zip)
        {
            try
            {
                var dt = _ExecuteProcedures.ReturnDataTable("GetAddressNassau",
               new string[] { "@Number", "@Street", "@Zip" },
               new string[] { Check_If_NullOrEmpty(Number), Check_If_NullOrEmpty(Street), Check_If_NullOrEmpty(Zip) });
                List<SearchAddress_PropertyModel> searchAddress_PropertyModel = new List<SearchAddress_PropertyModel>();
                if (dt.Rows.Count > 0)
                {
                    searchAddress_PropertyModel = (from DataRow row in dt.Rows
                                                   select new SearchAddress_PropertyModel
                                                   {
                                                       County = row["County"] == DBNull.Value ? "" : row["County"].ToString(),
                                                       APN = row["APN"] == DBNull.Value ? "" : row["APN"].ToString(),
                                                       Number = row["PropAddressNumber"] == DBNull.Value ? "" : row["PropAddressNumber"].ToString(),
                                                       OwnerName = row["PropertyOwner"] == DBNull.Value ? "" : row["PropertyOwner"].ToString(),
                                                       Street = row["PropAddressStreet"] == DBNull.Value ? "" : row["PropAddressStreet"].ToString(),
                                                       PropAddressFull = row["PropAddressFull"] == DBNull.Value ? "" : row["PropAddressFull"].ToString(),
                                                       PropAddress = row["PropAddress"] == DBNull.Value ? "" : row["PropAddress"].ToString(),
                                                       Taxes = string.Format("{0:0.##}", row["Taxes"] == DBNull.Value ? "0.00" : row["Taxes"]).ToString(),
                                                       MarketValue = string.Format("{0:0.##}", row["MarketValue"] == DBNull.Value ? "0.00" : row["MarketValue"]).ToString(),
                                                   }).ToList();
                }
                return searchAddress_PropertyModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Get Nassau Apn Details For Auth Docs
        /// </summary>
        /// <param name="apn"></param>
        /// <returns>true if apn matched from DB else return false</returns> 
        public NassauDesignation_PropertyModel GetNassauApnDetailsForAuthDocs(string apn)
        {
            try
            {
                var dt = _ExecuteProcedures.ReturnDataTable("GetDesignationFieldsNassau",
                    new string[] { "@APN" },
                    new string[] { Check_If_NullOrEmpty(apn) });
                NassauDesignation_PropertyModel nassauDesignation_PropertyModel = new NassauDesignation_PropertyModel();
                if (dt.Rows.Count > 0)
                {
                    nassauDesignation_PropertyModel = (from DataRow row in dt.Rows
                                                       select new NassauDesignation_PropertyModel
                                                       {
                                                           Parid = row["APN"] == DBNull.Value ? "" : row["APN"].ToString(),
                                                           Township = row["Township"] == DBNull.Value ? "" : row["Township"].ToString(),
                                                           Schoolformatted = row["SchoolFormatted"] == DBNull.Value ? "" : row["SchoolFormatted"].ToString(),
                                                           SBLString = row["SBLString"] == DBNull.Value ? "" : row["SBLString"].ToString()
                                                       }).FirstOrDefault();
                }
                return nassauDesignation_PropertyModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get Suffolk Apn Details For Auth Docs
        /// </summary>
        /// <param name="apn"></param>
        /// <returns>true if apn matched from DB else return false</returns> 

        public SuffolkDesignation_PropertyModel GetSuffolkDetailsForAuthDocs(string apn)
        {
            try
            {
                var dt = _ExecuteProcedures.ReturnDataTable("GetDesignationFieldsSuffolk",
                    new string[] { "@APN" },
                    new string[] { Check_If_NullOrEmpty(apn) });
                SuffolkDesignation_PropertyModel suffolkDesignation_PropertyModel = new SuffolkDesignation_PropertyModel();
                if (dt.Rows.Count > 0)
                {
                    suffolkDesignation_PropertyModel = (from DataRow row in dt.Rows
                                                        select new SuffolkDesignation_PropertyModel
                                                        {
                                                            APN = row["APN"] == DBNull.Value ? "" : row["APN"].ToString(),
                                                            District = row["District"] == DBNull.Value ? "" : row["District"].ToString(),
                                                            Section = row["Section"] == DBNull.Value ? "" : row["Section"].ToString(),
                                                            Block = row["block"] == DBNull.Value ? "" : row["block"].ToString(),
                                                            Lot = row["lot"] == DBNull.Value ? "" : row["lot"].ToString(),
                                                            SchoolDistrict = row["SchoolDistrictName"] == DBNull.Value ? "" : row["SchoolDistrictName"].ToString(),
                                                            Town = row["Township"] == DBNull.Value ? "" : row["Township"].ToString(),
                                                        }).FirstOrDefault();
                }
                return suffolkDesignation_PropertyModel;
            }
            catch (Exception ex)
            {
                throw Generate_Exception.WriteErrorLog(ex);
            }
        }
        /// <summary>
        /// To Get Data From Suffolk Search Address.
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Street"></param>
        /// <param name="Zip"></param>
        /// <returns></returns>
        public List<SearchAddress_PropertyModel> GetDataForSuffolkRegion(string Number, string Street, string Zip = "")
        {
            List<SearchAddress_PropertyModel> searchAddress_PropertyModel = new List<SearchAddress_PropertyModel>();
            try
            {
                var dt = _ExecuteProcedures.ReturnDataTable("GetAddressSuffolk",
                    new string[] { "@Number", "@Street", "@Zip" },
                    new string[] { Check_If_NullOrEmpty(Number), Check_If_NullOrEmpty(Street), Check_If_NullOrEmpty(Zip) });
                if (dt.Rows.Count > 0)
                {
                    searchAddress_PropertyModel = (from DataRow row in dt.Rows
                                                   select new SearchAddress_PropertyModel
                                                   {
                                                       County = row["County"] == DBNull.Value ? "" : row["County"].ToString(),
                                                       APN = row["APN"] == DBNull.Value ? "" : row["APN"].ToString(),
                                                       Number = row["PropAddressNumber"] == DBNull.Value ? "" : row["PropAddressNumber"].ToString(),
                                                       Street = row["PropAddressStreet"] == DBNull.Value ? "" : row["PropAddressStreet"].ToString(),
                                                       PropAddressFull = row["PropAddressFull"] == DBNull.Value ? "" : row["PropAddressFull"].ToString(),
                                                       PropAddress = row["PropAddress"] == DBNull.Value ? "" : row["PropAddress"].ToString(),
                                                       OwnerName = row["PropertyOwner"] == DBNull.Value ? "" : row["PropertyOwner"].ToString(),
                                                       Taxes = string.Format("{0:0.##}", row["Taxes"] == DBNull.Value ? "0.00" : row["Taxes"]).ToString(),
                                                       MarketValue = string.Format("{0:0.##}", row["MarketValue"] == DBNull.Value ? "0.00" : row["MarketValue"]).ToString(),
                                                   }).ToList();
                }
                return searchAddress_PropertyModel;
            }
            catch (Exception ex)
            {
                Generate_Exception.WriteErrorLog(ex);
                return new List<SearchAddress_PropertyModel>();
            }

        }
        /// <summary>
        /// To Unparse Property Address
        /// </summary>
        /// <param name="unpassAddress">value as string</param>
        /// <returns>return Tuple as item1, item2, item3</returns>
        private Tuple<string, string, string> Unparsed_process(string unpassAddress)
        {

            ClsUsers objUser = new ClsUsers();
            conversion convrsn = new conversion();
            var Number = "";
            var Street = "";
            string street = "";
            string unit = "", units = "";
            string no = "";
            string strti = "";
            string unitvalue = "";


            unpassAddress = Regex.Replace(unpassAddress, @"[ ]{2,}", " ");
            unpassAddress = unpassAddress.Replace(" -", "");
            unpassAddress = unpassAddress.Replace("- ", "");
            unpassAddress = unpassAddress.Replace("(", " ");
            unpassAddress = unpassAddress.Replace(")", " ");
            unpassAddress = unpassAddress.Trim();
            unpassAddress = unpassAddress.Replace("-", "");
            unpassAddress = unpassAddress.Replace(",", "");


            string[] unpass = unpassAddress.Split(' ');
            Number = unpass[0];

            char Num;
            Num = Convert.ToChar(Number.Substring(0, 1));
            if (char.IsDigit(Num) == false)
            {
                Number = "";
            }
            if (Number != "")
            {
                //Num= Convert.ToInt32(Number.Substring(Number))
                string[] streetNumberSuffix = {
            "st",
            "nd",
            "rd",
            "th",
            "TH"
        };
                Int32 suffixPosition;
                //go through possible suffixes
                // for (Int16 i = 0; i <= Information.UBound(streetNumberSuffix); i++)
                for (Int16 i = 0; i < streetNumberSuffix.Length; i++)
                {
                    //  suffixPosition = Strings.InStr(Number, streetNumberSuffix[i]);
                    suffixPosition = Number.IndexOf(streetNumberSuffix[i]);
                    //check to see if a possible street number exists
                    if (suffixPosition > 1)
                    {
                        //check if char before is numeric
                        var num = Number.Substring(suffixPosition - 1, 1);
                        int n;
                        bool isNumeric = int.TryParse(num, out n);

                        if (isNumeric == true)

                        {
                            //strip out suffix
                            Number = "";
                        }
                    }
                }
            }
            /*HttpContext. Current.Session["no"]*/
            no = Number;
            string newString = "";
            if (Number == "")
            {
                newString = unpassAddress;
            }
            else
            {
                newString = unpassAddress.Substring(unpassAddress.IndexOf(' ') + 1);
            }
            Street = newString;
            /*HttpContext.Current.Session["strti"]*/
            strti = Street.Trim();
            string[] nwstreet = Street.Split(' ');
            int len = nwstreet.Length;
            DataSet suffixvalue = new DataSet();
            for (int up = len - 1; up >= 0; up--)
            {
                string last = nwstreet[up];
                last = last.Replace(".", "");
                suffixvalue = objUser.AddressMatcher_Suffixs(last.Trim());
                if (suffixvalue.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i <= up; i++)
                    {
                        street += nwstreet[i] + " ";
                    }

                    for (int i = up + 1; i < len; i++)
                    {
                        unit += nwstreet[i] + " ";
                    }
                    street = street.Substring(0, street.Length - 1);
                    /* HttpContext.Current.Session["strti"]*/
                    strti = street;
                    if (unit != "")
                    {
                        unit.Trim();
                        string[] dir = unit.Split(' ');
                        string dic = dir[0];

                        switch (dic.ToUpper())
                        {
                            case "EAST":
                            case "EAST.":
                            case "E":
                            case "E.":
                                /* HttpContext.Current.Session["strti"] = *//*HttpContext.Current.Session["strti"]*/
                                strti = strti + " " + dic;
                                unit = "";
                                break;
                            case "WEST":
                            case "WEST.":
                            case "W":
                            case "W.":
                                /*HttpContext.Current.Session["strti"] = HttpContext.Current.Session["strti"]*/
                                strti = strti + " " + dic;
                                unit = "";
                                break;
                            case "NORTH":
                            case "NORTH.":
                            case "N":
                            case "N.":
                                /*HttpContext.Current.Session["strti"] = HttpContext.Current.Session["strti"]*/
                                strti = strti + " " + dic;
                                unit = "";
                                break;
                            case "SOUTH":
                            case "SOUTH.":
                            case "S":
                            case "S.":
                                /*HttpContext.Current.Session["strti"] = HttpContext.Current.Session["strti"]*/
                                strti = strti + " " + dic;
                                unit = "";
                                break;
                            case "SOUTHWEST":
                            case "SOUTHWEST.":
                            case "SOUTHEAST":
                            case "SOUTHEAST.":
                            case "SW":
                            case "SW.":
                            case "SE":
                            case "SE.":
                                /*HttpContext.Current.Session["strti"] = HttpContext.Current.Session["strti"]*/
                                strti = strti + " " + dic;
                                unit = "";
                                break;
                            case "NORTHWEST":
                            case "NORTHWEST.":
                            case "NORTHEAST":
                            case "NORTHEAST.":
                            case "NW":
                            case "NW.":
                            case "NE":
                            case "NE.":
                                /*HttpContext.Current.Session["strti"] = HttpContext.Current.Session["strti"]*/
                                strti = strti + " " + dic;
                                unit = "";
                                break;


                        }


                    }
                    goto un;
                }
            }
        un:
            if (unit != "")
            {
                unit = unit.Substring(0, unit.Length - 1);
                unit = unit.Replace("UNIT", "");
                unit = unit.Replace("Unit", "");
                unit = unit.Replace("unit", "");
                unit = unit.Replace("  ", " ");

                unit = unit.Trim();
                string[] unitval = unit.Split(' ');
                int length = unitval.Length;
                for (int u = 0; u < length; u++)
                {
                    string w = unitval[u];
                    //  if (w.IndexOf("#")!=w.Length-1)
                    if (!(w.Substring(w.Length - 1, 1) == "#"))
                    {
                        units += unitval[u] + " ";
                    }


                }
                units = units.Substring(0, units.Length - 1);
                unit = units;
                DataSet unitvalues = objUser.Addressmatcher_Unitvalue();
                string[] stringunit = new string[unitvalues.Tables[0].Rows.Count];
                string[] unitvals = unit.Split(' ');
                DataView unitview = unitvalues.Tables[0].DefaultView;
                string uvalues = "";
                int unitlength = unitvals.Length;
                for (int l = 0; l < unitlength; l++)
                {
                    string unitcompare = unitvals[l];
                    unitview.RowFilter = "UnitValue='" + unitcompare + "'";
                    if (unitview.Count == 0)
                    {
                        uvalues = uvalues += unitvals[l] + " ";

                    }
                }
                unit = uvalues.Substring(0, uvalues.Length - 1);
                /*HttpContext.Current.Session["unit"]*/
                unitvalue = unit;
            }
            //direction
            return new Tuple<string, string, string>(no, strti, unitvalue);
        }
        #endregion
        #region Utility Functions
        /// <summary>
        /// To check value if empty or not
        /// </summary>
        /// <param name="value">value as object</param>
        /// <returns>return object is null or not</returns>
        private string Check_If_NullOrEmpty(object value)
        {
            if (value != null)
            {
                return string.IsNullOrEmpty(value.ToString()) ? "" : value.ToString();
            }
            return null;
        }
        #endregion
    }
}
