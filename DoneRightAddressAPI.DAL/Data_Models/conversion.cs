using System;
using System.Collections;
using System.Data;

namespace DoneRightAddressAPI.DAL.Data_Models
{
    public class conversion
    {
        ClsUsers SharedMethods = new ClsUsers();
        public enum ConvertDataFormat
        {
            AbbreviatedName,
            FullName
        }
        static internal string Clean_DirectionSuffix(string streetData, conversion.ConvertDataFormat outputFormat)
        {

            //basically one function todo work of both clean direct and suffix.
            //used for one input field of street data where directio and suffi are not separated out.
            string cleanOutput = "";
            string Clean_DirectionSuffix = "";
            try
            {
                //check for enclosed quotes (exact search)
                bool enclosedInQuotes = false;
                if (Check_EnclosedInQuotes(streetData))
                {
                    //remember it was enclosed
                    enclosedInQuotes = true;
                    //remove enclosed quotes
                    // streetData = Strings.Mid(streetData, 2, Strings.Len(streetData) - 2);
                    streetData = streetData.Substring(1, streetData.Length - 2);
                }

                //clean the data
                // Clean_DirectionForUpperBound
                string[] streetParts = streetData.Split(' ');
                Int16 streetPartsUB = Convert.ToInt16(streetParts.GetUpperBound(0));
                Boolean Checkdirforlastchar = CheckdirectionforLast(streetParts[streetPartsUB]);
                if (Checkdirforlastchar)
                {
                    string strvalues = Clean_DirectionForUpperBound("DIRECTION", streetData, outputFormat);
                    string lastdirchar = strvalues.Split(' ')[strvalues.Split(' ').Length - 1];
                    string dirvalue = strvalues.Substring(0, strvalues.LastIndexOf(' '));
                    cleanOutput = Clean_Suffix(dirvalue, "SUFFIX", outputFormat);
                    cleanOutput = cleanOutput + " " + lastdirchar;
                }
                else
                {
                    cleanOutput = Clean_Suffix(Clean_Direction("DIRECTION", streetData, outputFormat), "SUFFIX", outputFormat);
                }
                //reapply enclosed quotes (if needed)
                if (enclosedInQuotes)
                {
                    cleanOutput = "\"" + cleanOutput + "\"";
                }

                //send result
                Clean_DirectionSuffix = cleanOutput;
            }
            catch (Exception)
            {

            }
            return Clean_DirectionSuffix;
        }
        private static bool Check_EnclosedInQuotes(string data)
        {
            bool Check_EnclosedInQuotes;
            //This function will check to see if the data passed in is enclosed in quotes.
            //For example: "court st"
            //It will return true if it is enclosed and false if not.


            if (data.Substring(0, 1) == "\"" && data.Substring(data.Length - 1) == "\"")
            {
                Check_EnclosedInQuotes = true;
            }
            else
            {
                Check_EnclosedInQuotes = false;
            }
            return Check_EnclosedInQuotes;
        }
        public string Clean_AddressNumber(string address)
        {
            //***************************************
            //This function removes chars from the passed in number (house or apt)
            //This is needed so the search is consistent with our data.
            //The return string is the cleaned data
            //***************************************

            //REMOVE DASH
            return address.Replace("-", "");
        }
        public string Clean_SalesValues(string strvalue)
        {
            //***************************************
            //This function removes chars from the passed in number (house or apt)
            //This is needed so the search is consistent with our data.
            //The return string is the cleaned data
            //***************************************

            //REMOVE DASH

            return strvalue.Replace("*", "");
        }
        static internal string Clean_DirectionStreetSuffix(string streetData, conversion.ConvertDataFormat outputFormat, bool issuffix = false)
        {
            string Clean_DirectionStreetSuffix = "";
            //basically one function todo work of both clean direct and suffix and street.
            //used for one input field of street data where directio and suffi are not separated out.

            try
            {
                // clean the data
                if (issuffix)
                {
                    return Clean_DirectionSuffix(streetData, outputFormat);
                }
                else
                {
                    return Clean_Street(Clean_DirectionSuffix(streetData, outputFormat));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Clean_DirectionStreetSuffix;

        }

        private static string Clean_Direction(string directionData, string streetData, ConvertDataFormat outputFormat)
        {
            string Clean_Direction = null;
            //***************************************
            //This function removes/adds/changes direction from the passed in street
            //This is needed so the search is flexible for users needs because of difference between user enter and DB
            //The return string is the cleaned data
            //***************************************

            try
            {
                Boolean a = true;
                //check how many parts are entered
                string[] streetParts = streetData.Split(' ');
                Int16 streetPartsLB = Convert.ToInt16(streetParts.GetLowerBound(0));
                //2 parts
                if (streetParts.Length >= 2)
                {
                    //check direction

                    switch ((streetParts[streetPartsLB]).ToUpper())
                    {
                        case "EAST":
                        case "EAST.":
                        case "E":
                        case "E.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "EAST" : "E");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        case "WEST":
                        case "WEST.":
                        case "W":
                        case "W.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "WEST" : "W");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        case "NORTH":
                        case "NORTH.":
                        case "N":
                        case "N.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "NORTH" : "N");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        case "SOUTH":
                        case "SOUTH.":
                        case "S":
                        case "S.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "SOUTH" : "S");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        default:

                            if (directionData == "DIRECTION")
                            {
                                directionData = "";
                            }

                            Clean_Direction = (directionData + " " + streetData).Trim();
                            a = false;
                            break;
                    }

                    //put parts back togther
                    if (a == true)
                    {
                        for (Int16 i = 1; i <= streetParts.GetUpperBound(0); i++)
                        {


                            Clean_Direction = (Clean_Direction + " " + streetParts[i]).Trim();
                        }
                    }

                    Clean_Direction = (directionData + " " + Clean_Direction).Trim();
                    //Clean_DirectionStreet.Join(" ", streetParts)
                }
                else
                {
                    if (directionData == "DIRECTION")
                    {
                        directionData = "";
                    }

                    Clean_Direction = (directionData + " " + streetData).Trim();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Clean_Direction;

        }
        private static string Clean_DirectionForUpperBound(string directionData, string streetData, ConvertDataFormat outputFormat)
        {
            string Clean_Direction = null;
            //***************************************
            //This function removes/adds/changes direction from the passed in street
            //This is needed so the search is flexible for users needs because of difference between user enter and DB
            //The return string is the cleaned data
            //***************************************

            try
            {
                Boolean a = true;
                //check how many parts are entered
                string[] streetParts = streetData.Split(' ');
                Int16 streetPartsLB = Convert.ToInt16(streetParts.GetUpperBound(0));
                //2 parts
                if (streetParts.Length >= 2)
                {
                    //check direction

                    switch ((streetParts[streetPartsLB]).ToUpper())
                    {
                        case "EAST":
                        case "EAST.":
                        case "E":
                        case "E.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "EAST" : "E");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        case "WEST":
                        case "WEST.":
                        case "W":
                        case "W.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "WEST" : "W");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        case "NORTH":
                        case "NORTH.":
                        case "N":
                        case "N.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "NORTH" : "N");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        case "SOUTH":
                        case "SOUTH.":
                        case "S":
                        case "S.":
                            if (directionData == "DIRECTION")
                            {
                                directionData = (outputFormat == ConvertDataFormat.FullName ? "SOUTH" : "S");
                            }

                            streetParts[streetPartsLB] = "";

                            break;
                        default:

                            if (directionData == "DIRECTION")
                            {
                                directionData = "";
                            }

                            Clean_Direction = (directionData + " " + streetData).Trim();
                            a = false;
                            break;
                    }

                    //put parts back togther
                    if (a == true)
                    {
                        for (Int16 i = 0; i <= streetParts.GetUpperBound(0); i++)
                        {


                            Clean_Direction += (streetParts[i]).Trim() + " ";
                        }
                    }

                    Clean_Direction = (Clean_Direction.Trim() + " " + directionData).Trim();
                    //Clean_DirectionStreet.Join(" ", streetParts)
                }
                else
                {
                    if (directionData == "DIRECTION")
                    {
                        directionData = "";
                    }

                    Clean_Direction = (directionData + " " + streetData).Trim();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Clean_Direction;

        }

        private static string Clean_Suffix(string streetData, string suffixData, ConvertDataFormat outputFormat)
        {
            //***************************************
            //This function removes suffix from the passed in street
            //This is needed so the search is flexible for users needs
            //The return string is the cleaned data
            //***************************************


            string Clean_Suffix = "";
            Boolean a = true;
            try
            {
                //check how many parts are entered
                string[] streetParts = streetData.Split(' ');
                //2 parts
                if (streetParts.Length >= 2)
                {
                    //check direction

                    Int16 streetPartsUB = Convert.ToInt16(streetParts.GetUpperBound(0));
                    switch (streetParts[streetPartsUB].ToUpper())
                    {
                        case "AVENUE":
                        case "AVENUE.":
                        case "AVE":
                        case "AVE.":
                        case "AV":
                        case "AV.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "AVENUE" : "AVE");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "BEND":
                        case "BEND.":
                        case "BND":
                        case "BND.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "BEND" : "BND");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "BOULEVARD":
                        case "BOULEVARD.":
                        case "BLVD":
                        case "BLVD.":
                        case "BOOL":
                        case "BOOL.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "BOULEVARD" : "BLVD");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "CAUSEWAY":
                        case "CAUSEWAY.":
                        case "CSWY":
                        case "CSWY.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "CAUSEWAY" : "CSWY");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "CIRCLE":
                        case "CIRCLE.":
                        case "CIR":
                        case "CIR.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "CIRCLE" : "CIR");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "COURT":
                        case "COURT.":
                        case "CT":
                        case "CT.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "COURT" : "CT");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "COVE":
                        case "COVE.":
                        case "CV":
                        case "CV.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "COVE" : "COVE");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "CRESCENT":
                        case "CRESCENT.":
                        case "CRES":
                        case "CRES.":
                        case "CRESC":
                        case "CRESC.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "CRESCENT" : "CRES");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "DRIVE":
                        case "DRIVE.":
                        case "DR":
                        case "DR.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "DRIVE" : "DR");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "EXPRESSWAY":
                        case "EXPRESSWAY.":
                        case "EXP":
                        case "EXP.":
                        case "EXPR":
                        case "EXPR.":
                        case "EXPRESS":
                        case "EXPRESS.":
                        case "EXPW":
                        case "EXPW.":
                        case "EXPWY":
                        case "EXPWY.":
                        case "EXPY":
                        case "EXPY.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "EXPRESSWAY" : "EXPY");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "HEIGHTS":
                        case "HEIGHTS.":
                        case "HTS":
                        case "HTS.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "HEIGHTS" : "HTS");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "HIGHWAY":
                        case "HIGHWAY.":
                        case "HWY":
                        case "HWY.":
                        case "HWAY":
                        case "HWAY.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "HIGHWAY" : "HWY");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "HILL":
                        case "HILL.":
                        case "HL":
                        case "HL.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "HILL" : "HL");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "LANE":
                        case "LANE.":
                        case "LA":
                        case "LA.":
                        case "LN":
                        case "LN.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "LANE" : "LN");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "LOOP":
                        case "LOOP.":
                        case "LOOPS":
                        case "LOOPS.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "LOOP" : "LOOP");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "OVAL":
                        case "OVAL.":
                        case "OVL":
                        case "OVL.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "OVAL" : "OVAL");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "PARK":
                        case "PARK.":
                        case "PK":
                        case "PK.":
                        case "PRK":
                        case "PRK.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "PARK" : "PARK");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "PARKWAY":
                        case "PARKWAY.":
                        case "Pk":
                        case "Pk.":
                        case "PKWY":
                        case "PKWY.":
                        case "PKY":
                        case "PKY.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "PARKWAY" : "PKWY");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "PATH":
                        case "PATH.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "PATH" : "PATH");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "PIKE":
                        case "PIKE.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "PIKE" : "PIKE");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "PLACE":
                        case "PLACE.":
                        case "PL":
                        case "PL.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "PLACE" : "PL");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "PLAZA":
                        case "PLAZA.":
                        case "PLZ":
                        case "PLZ.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "PLAZA" : "PLZ");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        //Case "POINT", "POINT.", "PT", "PT."						
                        //	If suffixData = "SUFFIX" Then suffixData = IIf(outputFormat = FullName, "POINT", "PT")
                        //	streetParts(streetPartsUB) = ""
                        case "RIDGE":
                        case "RIDGE.":
                        case "RDG":
                        case "RDG.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "RIDGE" : "RDG");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "ROAD":
                        case "ROAD.":
                        case "RD":
                        case "RD.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "ROAD" : "RD");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "ROW":
                        case "ROW.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "ROW" : "ROW");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "RUN":
                        case "RUN.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "RUN" : "RUN");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "SQUARE":
                        case "SQUARE.":
                        case "SQ":
                        case "SQ.":
                        case "SQU":
                        case "SQU.":
                        case "SQA":
                        case "SQA.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "SQUARE" : "SQ");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "STREET":
                        case "STREET.":
                        case "ST":
                        case "ST.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "STREET" : "ST");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "TERRACE":
                        case "TERRACE.":
                        case "TERR":
                        case "TERR.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "TERRACE" : "TERR");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "TRAIL":
                        case "TRAIL.":
                        case "TRL":
                        case "TRL.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = outputFormat == ConvertDataFormat.FullName ? "TRAIL" : "TRL";
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "TURNPIKE":
                        case "TURNPIKE.":
                        case "TPK":
                        case "TPK.":
                        case "TPKE":
                        case "TPKE.":
                        case "TNPKE":
                        case "TNPKE.":
                        case "TNPK":
                        case "TNPK.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "TURNPIKE" : "TPKE");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "WAY":
                        case "WAY.":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "WAY" : "WAY");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        case "LAKE":
                        case "LK":
                        case "LKE":
                        case "LAK":
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = (outputFormat == ConvertDataFormat.FullName ? "LAKE" : "LK");
                            }

                            streetParts[streetPartsUB] = "";
                            break;
                        default:
                            if (suffixData == "SUFFIX")
                            {
                                suffixData = "";
                            }

                            Clean_Suffix = (streetData + " " + suffixData).Trim();
                            a = false;
                            break;
                    }

                    //put parts back togther
                    if (a == true)
                    {
                        for (Int16 i = 0; i <= streetParts.GetUpperBound(0) - 1; i++)
                        {
                            Clean_Suffix = (Clean_Suffix + " " + streetParts[i]).Trim();
                        }
                    }
                    Clean_Suffix = (Clean_Suffix + " " + suffixData).Trim();
                }
                else
                {
                    if (suffixData == "SUFFIX")
                    {
                        suffixData = "";
                    }

                    Clean_Suffix = (streetData + " " + suffixData).Trim();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Clean_Suffix;

        }
        private static string Clean_Street(string streetData)
        {
            // This function removes/adds/changes possible streets with numbers
            // For example, 23rd street would become 23 street to match the db.
            // This is needed so the search is flexible for users needs because of difference between user enter and DB
            // The return string is the cleaned data
            string[] streetNumberSuffixes = { "st", "nd", "rd", "th", "TH" };
            foreach (string suffix in streetNumberSuffixes)
            {
                int suffixPosition = streetData.IndexOf(suffix, StringComparison.OrdinalIgnoreCase);
                if (suffixPosition > 0 && suffixPosition == streetData.Length - suffix.Length)
                {
                    // Check if the character before the suffix is a digit
                    if (char.IsDigit(streetData[suffixPosition - 1]))
                    {
                        streetData = streetData.Substring(0, suffixPosition) + suffix.ToLowerInvariant();
                        break;
                    }
                }
            }
            return streetData;
        }
        public string Get_StreetSuffix(string street, string County)
        {
            string Get_StreetSuffix = "";
            dynamic backupstreet = street;
            street = street.Replace(".", "").Replace("'", "''''");
            //for removing Period(.)
            string[] arr = null;
            string[] rec = null;
            ArrayList result = new ArrayList();
            DataTable dt = new DataTable();
            DataTable dt_prefix = new DataTable();
            string rtWild = "%";
            dynamic strt = "";
            dynamic j = 0;
            String flag = "";
            if (string.IsNullOrEmpty(County))
            {
                County = "NJ";
            }
            switch ((County.ToUpper()))
            {
                case "MANHATTAN":
                case "BRONX":
                case "BROOKLYN":
                case "QUEENS":
                case "STATEN ISLAND":
                case "STATENISLAND":
                    street = Clean_DirectionStreetSuffix(Clean_SalesValues(street), ConvertDataFormat.FullName);
                    break;
                case "NASSAU":
                    street = Clean_DirectionStreetSuffix(Clean_SalesValues(street), ConvertDataFormat.AbbreviatedName, true);
                    break;
                case "SUFFOLK":
                    street = Clean_DirectionStreetSuffix(Clean_SalesValues(street), ConvertDataFormat.AbbreviatedName, true);
                    break;
                case "NJ":
                    street = Clean_DirectionStreetSuffix(Clean_SalesValues(street), ConvertDataFormat.AbbreviatedName, true);
                    break;
                default:
                    street = Clean_DirectionStreetSuffix(Clean_SalesValues(street), ConvertDataFormat.AbbreviatedName, true);
                    break;
            }
            street = street.Replace(",", "");
            rec = street.Split(',');
            foreach (string st in rec)
            {
                j += 1;
                //st = Clean_Street(st.Trim());
                //arr = st.Split(" ");

                string st1 = "";
                st1 = Clean_Street(st.Trim());
                arr = st1.Split(' ');
                if (arr.Length > 1)
                {
                    if ((Checkdirection(arr[arr.Length - 1]) == "%"))
                    {
                        string sql = "select * from [Public_Record].[dbo].[Street_Suffixes] where suffix_group=(select top 1 suffix_group from [Public_Record].[dbo].[Street_Suffixes] where suffix='" + arr[arr.Length - 1] + "')";
                        dt = SharedMethods.Get_DataFromDatabaseInDT(sql);
                        if (dt.Rows.Count <= 0)
                        {
                            flag = "1";
                            sql = "select * from [Public_Record].[dbo].[Street_Suffixes] where suffix_group=(select top 1 suffix_group from [Public_Record].[dbo].[Street_Suffixes] where suffix='" + arr[arr.Length - 2] + "')";
                            dt = SharedMethods.Get_DataFromDatabaseInDT(sql);
                        }

                        string sql1 = "select * from [Public_Record].[dbo].[Street_Numbered_Suffixes] where Number='" + arr[0] + "'";

                        DataTable dt1 = SharedMethods.Get_DataFromDatabaseInDT(sql1);
                        if ((dt1.Rows.Count == 0))
                        {
                            string sqls = "select * from [Public_Record].[dbo].[Street_Numbered_Suffixes] where Number='" + arr[1] + "'";
                            dt1 = SharedMethods.Get_DataFromDatabaseInDT(sqls);
                        }
                        //prefix matching
                        string sql10 = "select prefix_group from [Public_Record].[dbo].[Street_Prefixes] where prefix='" + arr[0] + "'";
                        dt_prefix = SharedMethods.Get_DataFromDatabaseInDT(sql10);
                        if ((dt_prefix.Rows.Count > 0))
                        {
                            string pre = dt_prefix.Rows[0][0].ToString();
                            string sql3 = "select prefix_group,prefix from [Public_Record].[dbo].[Street_Prefixes] where prefix_group='" + pre + "'and " + "prefix <>'" + arr[0] + "'";
                            dt_prefix = SharedMethods.Get_DataFromDatabaseInDT(sql3);
                        }

                        if (dt.Rows.Count > 1)
                        {
                            street = " and(";
                            string stin = "";

                            if (arr.Length == 2)
                            {
                                string wild = "%";
                                wild = Checkdirection(arr[0]);

                                if (flag == "1")
                                {
                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                    {
                                        stin = stin + "st_name like '" + wild + dt.Rows[i][0] + " " + arr[arr.Length - 1] + rtWild + "' or ";
                                    }
                                }
                                else
                                {
                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                    {
                                        stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                    }
                                }

                                //Numerical Suffix
                                if (dt1.Rows.Count > 0)
                                {
                                    string number = arr[0].ToString();
                                    string numbersurfix = dt1.Rows[0][0].ToString();
                                    if ((number == numbersurfix))
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                }
                                //prefix matching

                                if (dt_prefix.Rows.Count > 0)
                                {
                                    for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                }
                            }

                            if (arr.Length == 3)
                            {
                                string wild = "%";
                                wild = Checkdirection(arr[0]);

                                if (flag == "1")
                                {
                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                    {
                                        stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " " + arr[arr.Length - 1] + rtWild + "' or ";
                                    }
                                }
                                else
                                {
                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                    {
                                        stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                    }
                                }


                                //prefix matching

                                if (dt_prefix.Rows.Count > 0)
                                {
                                    for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                }
                                //Numerical Suffix
                                if (dt1.Rows.Count > 0)
                                {
                                    string number = arr[1].ToString();
                                    string numbersurfix = dt1.Rows[0][0].ToString();
                                    if ((number == numbersurfix))
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                }
                                //prefix suffix
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                        {
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }
                                        }
                                    }
                                }
                            }

                            if (arr.Length == 4)
                            {
                                string wild = "%";
                                wild = Checkdirection(arr[0]);
                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                {
                                    stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                    //stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                }
                                //prefix suffix
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    string number = arr[0].ToString();
                                    string numbersurfix = dt1.Rows[0][0].ToString();
                                    if ((number == numbersurfix))
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + dt1.Rows[0][1] + " " + arr[1] + " " + arr[2] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + arr[0] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                        }
                                    }
                                }
                                //prefix suffix
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                        {
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '%" + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }
                                        }
                                    }
                                }
                                string sql2 = "select * from [Public_Record].[dbo].[Street_Suffixes] where suffix_group=(select top 1 suffix_group from [Public_Record].[dbo].[Street_Suffixes] where suffix='" + arr[1] + "')";
                                DataTable dt2 = SharedMethods.Get_DataFromDatabaseInDT(sql2);
                                if (dt2.Rows.Count > 0)
                                {
                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                    {
                                        if (dt2.Rows[i][0].ToString().ToLower() != arr[1].ToString().ToLower())
                                        {
                                            if (dt.Rows.Count > 0)
                                            {
                                                for (Int16 i2 = 0; i2 <= dt.Rows.Count - 1; i2++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + dt.Rows[i2][0] + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                stin = stin + "st_name like '%" + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + arr[3] + rtWild + "' or ";
                                            }
                                        }
                                    }

                                    //prefix suffix
                                    if (dt_prefix.Rows.Count > 0)
                                    {

                                        for (Int16 i1 = 0; i1 <= dt2.Rows.Count - 1; i1++)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                if (dt2.Rows[i1][0].ToString().ToLower() != arr[1].ToString().ToLower())
                                                {
                                                    //if (dt_prefix.Rows[z][1].ToString().ToLower() != arr[0].ToString().ToLower())
                                                    //{
                                                    for (Int16 i2 = 0; i2 <= dt.Rows.Count - 1; i2++)
                                                    {
                                                        stin = stin + "st_name like '%" + dt_prefix.Rows[z][1] + " " + dt2.Rows[i1][0] + " " + arr[2] + " " + dt.Rows[i2][0] + rtWild + "' or ";
                                                        //}   
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (dt_prefix.Rows.Count > 0)
                            {
                                if (arr.Length > 4)
                                {
                                    street = "and st_name like'%" + backupstreet + "%'";
                                    break;
                                }
                            }
                            else
                            {
                                if (arr.Length == 2)
                                {
                                    //Directional
                                    switch (arr[0].ToUpper())//Strings.UCase(arr[0].ToString())
                                    {
                                        case "EAST":
                                        case "EAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "E " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "E " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "E " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "E":
                                        case "E.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "EAST " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "EAST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "EAST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "WEST":
                                        case "WEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "W " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "W " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "W " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "W":
                                        case "W.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "WEST " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "WEST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "WEST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTH":
                                        case "NORTH.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "N " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "N " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "N " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "N":
                                        case "N.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NORTH " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTH " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTH " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTH":
                                        case "SOUTH.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "S " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "S " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "S " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "S":
                                        case "S.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SOUTH " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTH " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTH " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTHWEST":
                                        case "NORTHWEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NW " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NW " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NW " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NW":
                                        case "NW.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NORTHWEST " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHWEST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHWEST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTHEAST":
                                        case "NORTHEAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NE " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NE " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NE " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NE":
                                        case "NE.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NORTHEAST " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHEAST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHEAST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTHWEST":
                                        case "SOUTHWEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SW " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SW " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SW " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SW":
                                        case "SW.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SOUTHWEST " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHWEST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHWEST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTHEAST":
                                        case "SOUTHEAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SE " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SE " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SE " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SE":
                                        case "SE.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SOUTHEAST " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHEAST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHEAST " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        default:

                                            break;
                                    }

                                }
                                else if (arr.Length == 3 | arr.Length == 4)
                                {
                                    //Directional
                                    switch (arr[0].ToUpper())//Strings.UCase(arr[0].ToString())
                                    {
                                        case "EAST":
                                        case "EAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "E " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "E " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "E " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "E":
                                        case "E.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "EAST " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "EAST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "EAST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "WEST":
                                        case "WEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "W " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "W " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "W " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "W":
                                        case "W.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "WEST " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "WEST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "WEST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTH":
                                        case "NORTH.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "N " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "N " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "N " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "N":
                                        case "N.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NORTH " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTH " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTH " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTH":
                                        case "SOUTH.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "S " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "S " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "S " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "S":
                                        case "S.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SOUTH " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTH " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTH " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }

                                            break;
                                        case "NORTHWEST":
                                        case "NORTHWEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NW " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NW " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NW " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NW":
                                        case "NW.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NORTHWEST " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHWEST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHWEST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTHEAST":
                                        case "NORTHEAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NE " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NE " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NE " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "NE":
                                        case "NE.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "NORTHEAST " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHEAST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "NORTHEAST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTHWEST":
                                        case "SOUTHWEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SW " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SW " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SW " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SW":
                                        case "SW.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SOUTHWEST " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHWEST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHWEST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTHEAST":
                                        case "SOUTHEAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SE " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SE " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SE " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        case "SE":
                                        case "SE.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + "SOUTHEAST " + arr[1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                            }

                                            if (dt1.Rows.Count > 0)
                                            {
                                                string number = arr[1].ToString();
                                                string numbersurfix = dt1.Rows[0][0].ToString();
                                                if ((number == numbersurfix))
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHEAST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                                else
                                                {
                                                    for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                    {
                                                        stin = stin + "st_name like '" + "SOUTHEAST " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                            break;
                                        default:

                                            break;
                                    }
                                }
                                else
                                {
                                    street = "and st_name like'%" + backupstreet + "%'";
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                            result.Add(stin);
                            //street = street + stin.Substring(0, stin.Length - 1) + ")"                                              
                        }
                        else
                        {
                            street = "and st_name like'%" + street + "%'";
                        }
                    }
                    else
                    {
                        string sql = "select * from [Public_Record].[dbo].[Street_Suffixes] where suffix_group=(select top 1 suffix_group from [Public_Record].[dbo].[Street_Suffixes] where suffix='" + arr[arr.Length - 2] + "')";
                        dt = SharedMethods.Get_DataFromDatabaseInDT(sql);
                        string sql1 = "select * from [Public_Record].[dbo].[Street_Numbered_Suffixes] where Number='" + arr[0] + "'";
                        DataTable dt1 = SharedMethods.Get_DataFromDatabaseInDT(sql1);
                        if ((dt1.Rows.Count == 0))
                        {
                            string sqls = "select * from [Public_Record].[dbo].[Street_Numbered_Suffixes] where Number='" + arr[1] + "'";
                            dt1 = SharedMethods.Get_DataFromDatabaseInDT(sqls);
                        }
                        string sql10 = "select prefix_group from [Public_Record].[dbo].[Street_Prefixes] where prefix='" + arr[0] + "'";
                        //Prefix suffix
                        dt_prefix = SharedMethods.Get_DataFromDatabaseInDT(sql10);
                        if ((dt_prefix.Rows.Count > 0))
                        {
                            string pre = dt_prefix.Rows[0][0].ToString();
                            string sql3 = "select prefix_group,prefix from [Public_Record].[dbo].[Street_Prefixes] where prefix_group='" + pre + "'and " + "prefix <>'" + arr[0] + "'";
                            dt_prefix = SharedMethods.Get_DataFromDatabaseInDT(sql3);
                        }
                        if (dt.Rows.Count > 1)
                        {
                            street = " and(";
                            string stin = "";

                            if (arr.Length == 2)
                            {
                                string wild = "%";
                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                {
                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + " " + arr[arr.Length - 1] + rtWild + "' or ";
                                }
                                wild = Checkdirection(arr[0]);
                                switch (arr[arr.Length - 1].ToUpper())//Strings.UCase(arr[arr.Length - 1].ToString())
                                {
                                    case "EAST":
                                    case "EAST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "E" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "E " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "E " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "E " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "E":
                                    case "E.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "EAST " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "EAST " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "EAST " + rtWild + "' or ";
                                                }
                                            }
                                        }

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "EAST " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "WEST":
                                    case "WEST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "W " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "W " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "W " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "W " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "W":
                                    case "W.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "WEST " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "WEST " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "WEST " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "WEST " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "NORTH":
                                    case "NORTH.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "N " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "N " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "N " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "N " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "N":
                                    case "N.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTH " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTH " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTH " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "NORTH " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "SOUTH":
                                    case "SOUTH.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "S " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "S " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "S " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "S " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "S":
                                    case "S.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTH " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTH " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTH " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "SOUTH " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "NORTHWEST":
                                    case "NORTHWEST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NW " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NW " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NW " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "NW " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "NW":
                                    case "NW.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTHWEST " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTHWEST " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTHWEST " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix

                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "NORTHWEST " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "NORTHEAST":
                                    case "NORTHEAST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NE " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NE " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NE " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "NE " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "NE":
                                    case "NE.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTHEAST " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTHEAST " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "NORTHEAST " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "NORTHEAST " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "SOUTHWEST":
                                    case "SOUTHWEST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SW " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SW " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SW " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "SW " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "SW":
                                    case "SW.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTHWEST " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTHWEST " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTHWEST " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "SOUTHWEST " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "SOUTHEAST":
                                    case "SOUTHEAST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SE " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SE " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SE " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "SE " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    case "SE":
                                    case "SE.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTHEAST " + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTHEAST " + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + dt.Rows[i][0] + "SOUTHEAST " + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + "SOUTHEAST " + rtWild + "' or ";
                                            }
                                        }
                                        break;
                                    default:

                                        break;
                                }
                            }

                            else if (arr.Length == 3)
                            {
                                string wild = "%";
                                wild = Checkdirection(arr[0]);
                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                {
                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " " + arr[2] + rtWild + "' or ";
                                }
                                //Prefix suffix
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " " + arr[2] + rtWild + "' or ";
                                        }
                                    }
                                }
                                //Numerical Suffix
                                if (dt1.Rows.Count > 0)
                                {
                                    string number = arr[1].ToString();
                                    string numbersurfix = dt1.Rows[0][0].ToString();
                                    if ((number == numbersurfix))
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt1.Rows[0][1] + " " + dt.Rows[i][0] + " " + arr[2] + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt1.Rows[0][1] + " " + dt.Rows[i][0] + " " + arr[2] + rtWild + "' or ";
                                        }
                                    }
                                }
                                //Prefix suffix
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                        {
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + rtWild + "' or ";
                                            }
                                        }
                                    }
                                }
                                switch (arr[arr.Length - 1].ToUpper()) //Strings.UCase(arr[arr.Length - 1].ToString())
                                {
                                    case "EAST":
                                    case "EAST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " E" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " E" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + "E" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " E" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " E" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "E":
                                    case "E.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " EAST" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " EAST" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " EAST" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " EAST" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " EAST" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "WEST":
                                    case "WEST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " W" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " W" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " W" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " W" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " W" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "W":
                                    case "W.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " WEST" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " WEST" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " WEST" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " WEST" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " WEST" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "NORTH":
                                    case "NORTH.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " N" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " N" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " N" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " N" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " N" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "N":
                                    case "N.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTH" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTH" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTH" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " NORTH" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " NORTH" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "SOUTH":
                                    case "SOUTH.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " S" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " S" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " S" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " S" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " S" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "S":
                                    case "S.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTH" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTH" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTH" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " SOUTH" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " SOUTH" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "NORTHWEST":
                                    case "NORTHWEST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NW" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NW" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NW" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " NW" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " NW" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "NW":
                                    case "NW.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTHWEST" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTHWEST" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTHWEST" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " NORTHWEST" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " NORTHWEST" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "NORTHEAST":
                                    case "NORTHEAST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NE" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NE" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NE" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " NE" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " NE" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "NE":
                                    case "NE.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTHEAST" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTHEAST" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " NORTHEAST" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " NORTHEAST" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " NORTHEAST" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "SOUTHWEST":
                                    case "SOUTHWEST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SW" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SW" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SW" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " SW" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " SW" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "SW":
                                    case "SW.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTHWEST" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTHWEST" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTHWEST" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " SOUTHWEST" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " SOUTHWEST" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "SOUTHEAST":
                                    case "SOUTHEAST.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SE" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SE" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SE" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " SE" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " SE" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "SE":
                                    case "SE.":
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTHEAST" + rtWild + "' or ";
                                        }

                                        if (dt1.Rows.Count > 0)
                                        {
                                            string number = arr[1].ToString();
                                            string numbersurfix = dt1.Rows[0][0].ToString();
                                            if ((number == numbersurfix))
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTHEAST" + rtWild + "' or ";
                                                }
                                            }
                                            else
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    stin = stin + "st_name like '" + wild + arr[0] + " " + dt.Rows[i][0] + " SOUTHEAST" + rtWild + "' or ";
                                                }
                                            }
                                        }
                                        //Prefix suffix
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                                {
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " SOUTHEAST" + rtWild + "' or ";
                                                    }
                                                    else
                                                    {
                                                        stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt.Rows[i][0] + " SOUTHEAST" + rtWild + "' or ";
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            else if (arr.Length == 4)
                            {
                                string wild = "%";
                                wild = Checkdirection(arr[0]);
                                for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                {
                                    stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                }
                                //Prefix suffix
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                        }
                                    }
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    string number = arr[0].ToString();
                                    string numbersurfix = dt1.Rows[0][0].ToString();
                                    if ((number == numbersurfix))
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + dt1.Rows[0][1] + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            stin = stin + "st_name like '%" + arr[0] + " " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                        }
                                    }
                                }
                                if (dt_prefix.Rows.Count > 0)
                                {
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                        {
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }
                                        }
                                    }
                                }
                                string sql2 = "select * from [Public_Record].[dbo].[Street_Suffixes] where suffix_group=(select top 1 suffix_group from [Public_Record].[dbo].[Street_Suffixes] where suffix='" + arr[1] + "')";
                                DataTable dt2 = SharedMethods.Get_DataFromDatabaseInDT(sql2);

                                if (dt2.Rows.Count > 0)
                                {
                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                    {
                                        stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + arr[3] + rtWild + "' or ";
                                    }
                                    switch (arr[arr.Length - 1].ToUpper())//Strings.UCase(arr[arr.Length - 1].ToString())
                                    {
                                        case "EAST":
                                        case "EAST.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "E" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "E" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "E" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "E":
                                        case "E.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "EAST" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "EAST" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "EAST" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "WEST":
                                        case "WEST.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "W" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "W" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "W" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "W":
                                        case "W.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "WEST" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "WEST" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "WEST" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTH":
                                        case "NORTH.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "N" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "N" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "N" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "N":
                                        case "N.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NORTH" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "NORTH" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NORTH" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTH":
                                        case "SOUTH.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "S" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "S" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "S" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "S":
                                        case "S.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SOUTH" + rtWild + "' or ";
                                            }

                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "SOUTH" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SOUTH" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTHWEST":
                                        case "NORTHWEST.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NW" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "NW" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NW" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "NW":
                                        case "NW.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NORTHWEST" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "NORTHWEST" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NORTHWEST" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "NORTHEAST":
                                        case "NORTHEAST.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NE" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "NE" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NE" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "NE":
                                        case "NE.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NORTHEAST" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "NORTHEAST" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "NORTHEAST" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTHWEST":
                                        case "SOUTHWEST.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SW" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "SW" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SW" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "SW":
                                        case "SW.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SOUTHWEST" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "SOUTHWEST" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SOUTHWEST" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "SOUTHEAST":
                                        case "SOUTHEAST.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SE" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "SE" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SE" + rtWild + "' or ";
                                                        }

                                                    }
                                                }
                                            }
                                            break;
                                        case "SE":
                                        case "SE.":
                                            for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + arr[0] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SOUTHEAST" + rtWild + "' or ";
                                            }


                                            if (dt_prefix.Rows.Count > 0)
                                            {
                                                for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                                {
                                                    for (Int16 i = 0; i <= dt2.Rows.Count - 1; i++)
                                                    {
                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt1.Rows[0][1] + " " + arr[2] + " " + "SOUTHEAST" + rtWild + "' or ";
                                                        }
                                                        else
                                                        {
                                                            stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + dt2.Rows[i][0] + " " + arr[2] + " " + "SOUTHEAST" + rtWild + "' or ";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        default:

                                            break;
                                    }
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    switch (arr[0].ToUpper()) //Strings.UCase(arr[0].ToString())
                                    {
                                        case "EAST":
                                        case "EAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "E" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "E":
                                        case "E.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "EAST" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "WEST":
                                        case "WEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "W" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "W":
                                        case "W.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "WEST" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "NORTH":
                                        case "NORTH.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "N" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "N":
                                        case "N.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "NORTH" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "SOUTH":
                                        case "SOUTH.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "S" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "S":
                                        case "S.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "SOUTH" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "NORTHWEST":
                                        case "NORTHWEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "NW" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "NW":
                                        case "NW.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "NORTHWEST" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "NORTHEAST":
                                        case "NORTHEAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "NE" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "NE":
                                        case "NE.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "NORTHEAST" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "SOUTHWEST":
                                        case "SOUTHWEST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "SW" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "SW":
                                        case "SW.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "SOUTHWEST" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "SOUTHEAST":
                                        case "SOUTHEAST.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "SE" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        case "SE":
                                        case "SE.":
                                            for (Int16 i = 0; i <= dt.Rows.Count - 1; i++)
                                            {
                                                stin = stin + "st_name like '" + wild + "SOUTHEAST" + " " + arr[1] + " " + dt.Rows[i][0] + " " + arr[3] + rtWild + "' or ";
                                            }

                                            break;
                                        default:

                                            break;
                                    }
                                }
                            }
                            else
                            {
                                street = "and st_name like'%" + backupstreet + "%'";
                                break; // TODO: might not be correct. Was : Exit For
                            }
                            result.Add(stin);
                            //street = street + stin.Substring(0, stin.Length - 1) + ")"
                        }
                        else
                        {
                            string stin = "";
                            string wild = "%";
                            wild = Checkdirection(arr[0]);
                            switch (arr[arr.Length - 1].ToUpper())//Strings.UCase(arr[arr.Length - 1].ToString())
                            {
                                case "EAST":
                                case "EAST.":
                                case "E":
                                case "E.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "E" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "EAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "E" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "EAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "E" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "EAST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "E" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "EAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "E" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "EAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "E" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "EAST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "E" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "EAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "E" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "EAST" + rtWild + "' or ";

                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "E" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "EAST" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                                case "WEST":
                                case "WEST.":
                                case "W":
                                case "W.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "W" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "WEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "W" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "WEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "W" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "WEST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "W" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "WEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "W" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "WEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "W" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "WEST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "W" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "WEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "W" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "WEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "W" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "WEST" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                                case "NORTH":
                                case "NORTH.":
                                case "N":
                                case "N.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "N" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "NORTH" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "N" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NORTH" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "N" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NORTH" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "N" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "NORTH" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "N" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NORTH" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "N" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NORTH" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "N" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "NORTH" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "N" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NORTH" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "N" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NORTH" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }

                                    break;
                                case "SOUTH":
                                case "SOUTH.":
                                case "S":
                                case "S.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "S" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "SOUTH" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "S" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SOUTH" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "S" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SOUTH" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "S" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "SOUTH" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "S" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SOUTH" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "S" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SOUTH" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "S" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "SOUTH" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "S" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SOUTH" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "S" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SOUTH" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                                case "NORTHWEST":
                                case "NORTHWEST.":
                                case "NW":
                                case "NW.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "NW" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "NORTHWEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NORTHWEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NORTHWEST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "NW" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "NORTHWEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NORTHWEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NORTHWEST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "NW" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "NORTHWEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NORTHWEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NORTHWEST" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                                case "NORTHEAST":
                                case "NORTHEAST.":
                                case "NE":
                                case "NE.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "NE" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "NORTHEAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NORTHEAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "NORTHEAST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "NE" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "NORTHEAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NORTHEAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "NORTHEAST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "NE" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "NORTHEAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NORTHEAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "NORTHEAST" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                                case "SOUTHWEST":
                                case "SOUTHWEST.":
                                case "SW":
                                case "SW.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "SW" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "SOUTHWEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SOUTHWEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SOUTHWEST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "SW" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "SOUTHWEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SOUTHWEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SOUTHWEST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "SW" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "SOUTHWEST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SOUTHWEST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SW" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SOUTHWEST" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                                case "SOUTHEAST":
                                case "SOUTHEAST.":
                                case "SE":
                                case "SE.":
                                    if (arr.Length == 2)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "SE" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + "SOUTHEAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SOUTHEAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + "SOUTHEAST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 3)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "SE" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + "SOUTHEAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SOUTHEAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + "SOUTHEAST" + rtWild + "' or ";
                                        }
                                    }
                                    else if (arr.Length == 4)
                                    {
                                        street = " and(";
                                        if (dt_prefix.Rows.Count > 0)
                                        {
                                            for (Int32 z = 0; z <= dt_prefix.Rows.Count - 1; z++)
                                            {
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "SE" + rtWild + "' or ";
                                                stin = stin + "st_name like '" + wild + dt_prefix.Rows[z][1] + " " + arr[1] + " " + arr[2] + " " + "SOUTHEAST" + rtWild + "' or ";
                                            }
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SOUTHEAST" + rtWild + "' or ";
                                        }
                                        else
                                        {
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SE" + rtWild + "' or ";
                                            stin = stin + "st_name like '" + wild + arr[0] + " " + arr[1] + " " + arr[2] + " " + "SOUTHEAST" + rtWild + "' or ";
                                        }
                                    }
                                    else
                                    {
                                        street = "and st_name like'%" + street + "%'";
                                        goto STR;
                                    }
                                    break;
                            }
                            result.Add(stin);
                            string str = null;
                            try
                            {
                                foreach (string res in result)
                                {
                                    str = str + res;
                                }
                                if (str.Length > 0)
                                {
                                    street = street + str.Substring(0, str.Length - 4) + ")";
                                }

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }

                }
                else
                {
                    if (rec.Length > 1)
                    {
                        if (j == 1)
                        {
                            strt = "and st_name like'%" + st + "%'";
                        }
                        else
                        {
                            strt += "or st_name like'%" + st + "%'";
                        }
                        street = strt;
                    }
                    else
                    {
                        street = "and st_name like'%" + street + "%'";
                    }

                }

            }

            if (dt.Rows.Count > 1)
            {

                string str = "";
                try
                {
                    foreach (string res in result)
                    {
                        str = str + res;
                    }
                    if (str.Length > 0)
                    {
                        street = street + str.Substring(0, str.Length - 4) + ")";
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        STR:
            Get_StreetSuffix = street;
            return Get_StreetSuffix;
        }
        public static string Checkdirection(string value)
        {
            string wild = "%";
            switch (value.ToUpper())//Strings.UCase(value)
            {
                case "EAST":
                case "EAST.":
                case "E":
                case "E.":
                    wild = "%";
                    break;
                case "WEST":
                case "WEST.":
                case "W":
                case "W.":
                    wild = "%";
                    break;
                case "NORTH":
                case "NORTH.":
                case "N":
                case "N.":
                    wild = "%";
                    break;
                case "SOUTH":
                case "SOUTH.":
                case "S":
                case "S.":
                    wild = "";
                    break;
                case "SOUTHWEST":
                case "SOUTHWEST.":
                case "SOUTHEAST":
                case "SOUTHEAST.":
                case "SW":
                case "SW.":
                case "SE":
                case "SE.":
                    wild = "%";
                    break;
                case "NORTHWEST":
                case "NORTHWEST.":
                case "NORTHEAST":
                case "NORTHEAST.":
                case "NW":
                case "NW.":
                case "NE":
                case "NE.":
                    wild = "%";
                    break;
                default:
                    break;
            }
            return wild;
        }
        public static Boolean CheckdirectionforLast(string value)
        {

            switch (value.ToUpper())//Strings.UCase(value)
            {
                case "EAST":
                case "EAST.":
                case "E":
                case "E.":
                    return true;
                case "WEST":
                case "WEST.":
                case "W":
                case "W.":
                    return true;
                case "NORTH":
                case "NORTH.":
                case "N":
                case "N.":
                    return true;
                case "SOUTH":
                case "SOUTH.":
                case "S":
                case "S.":
                    return true;
                case "SOUTHWEST":
                case "SOUTHWEST.":
                case "SOUTHEAST":
                case "SOUTHEAST.":
                case "SW":
                case "SW.":
                case "SE":
                case "SE.":
                    return true;
                case "NORTHWEST":
                case "NORTHWEST.":
                case "NORTHEAST":
                case "NORTHEAST.":
                case "NW":
                case "NW.":
                case "NE":
                case "NE.":
                    return true;
                default:
                    return false;
            }

        }
    }
}
