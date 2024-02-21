using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FHP.utilities
{
    
        public class ActivityMsgResponse
        {
            public string title { get; set; }
            public string desc { get; set; }
        }
        public class Utility
        {

            public static (DateTime startDate, DateTime endDate) GetSwitchDateRange(Constants.DaysType daysType, TimeZoneInfo timeZone)
            {
                DateTime utcNow = DateTime.UtcNow;

                DateTime currentDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);

                DateTime startDate;
                DateTime endDate;

                switch (daysType)
                {
                    case Constants.DaysType.Today:
                        startDate = currentDate;
                        endDate = currentDate.AddDays(1).AddTicks(-1);
                        break;

                    case Constants.DaysType.ThisWeek:
                        startDate = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek);
                        endDate = startDate.AddDays(7).AddTicks(-1);
                        break;

                    case Constants.DaysType.ThisMonth:
                        startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        endDate = startDate.AddMonths(1).AddTicks(-1);
                        break;

                    case Constants.DaysType.LastMonth:
                        startDate = currentDate.AddMonths(-1);
                        startDate = new DateTime(startDate.Year, startDate.Month, 1);
                        endDate = startDate.AddMonths(1).AddTicks(-1);
                        break;

                    case Constants.DaysType.LastThreeMonths:
                        startDate = currentDate.AddMonths(-3);
                        startDate = new DateTime(startDate.Year, startDate.Month, 1);
                        endDate = currentDate.AddTicks(-1);
                        break;

                    default:
                        startDate = currentDate;
                        endDate = currentDate.AddDays(1).AddTicks(-1);
                        break;
                }

                return (startDate, endDate);
            }
            public static (DateTime from, DateTime to) SetTimeAndConvertToUtc(DateTime from, DateTime to, TimeZoneInfo timeZone)
            {
                // Set the time component of 'from' to 00:00:00 (midnight) and 'to' to 23:59:59.

                DateTime adjustedFromDateTime = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0, DateTimeKind.Unspecified);
                adjustedFromDateTime = TimeZoneInfo.ConvertTimeToUtc(adjustedFromDateTime, timeZone);


                DateTime adjustedToDateTime = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59, DateTimeKind.Unspecified);
                adjustedToDateTime = TimeZoneInfo.ConvertTimeToUtc(adjustedToDateTime, timeZone);

                return (adjustedFromDateTime, adjustedToDateTime);
            }

            private static readonly Random random = new Random();
            public static TimeZoneInfo GetTimeZone(string timeZoneName)
            {
                return TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(x => x.StandardName.Equals(timeZoneName));
            }

            public static DateTime GetDateTime()
            {
                return DateTime.UtcNow;
            }

            public static string GetUnixDateTimeString()
            {
                return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            }

            public static DateTime GetDateTime(DateTime dateTime, string timeZoneName)
            {
                //return if timezone name is empty
                if (string.IsNullOrEmpty(timeZoneName))
                {
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

                    return dateTime.ToLocalTime();
                }

                //return converted date time
                var timeZone = GetTimeZone(timeZoneName);
                return timeZone != null
                    ? TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, timeZone)
                    : dateTime.ToLocalTime();
            }

            public static string GetFormattedDate(DateTime? dateTime)
            {
                return dateTime?.ToString(Constants.DateFormat);
            }

            public static string DefaultErrorMessage()
            {
                return "Something went wrong. Please try again after some time";
            }

            public static string GetUniqueFileName(string originalFileName)
            {
                return $"{Guid.NewGuid():N}{Path.GetExtension(originalFileName)}";
            }

            public static void CreateFolder(string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            public static string GetTempFolder(string basePath)
            {
                var path = $"{basePath}\\temp\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }


            public static string GetInvoiceFolder(string basePath)
            {
                var path = $"{basePath}\\invoice\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }

            public static string GetTempFileUrl(string baseUrl, string fileName)
            {
                return $"{baseUrl}/temp/{fileName}";
            }

            public static string GetEmailTemplateFolder(string basePath, string templateName)
            {
                return $"{basePath}\\templates\\email\\{templateName}";
            }

            public static bool IsValidImage(string extension)
            {
                var imageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };
                return imageExtensions.Contains(extension.ToUpper());
            }

            public static string Encrypt(string plainText)
            {
            if (plainText == null) throw new ArgumentNullException("plainText");

                //encrypt data
                //var data = Encoding.Unicode.GetBytes(plainText);
                //byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                const int WorkFactor = 10;
                var HashedPassword = BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);
                //return as base64 string
                return HashedPassword;
            }
            public static bool IsBase64String(string s)
            {
                s = s.Trim();
                return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

            }

            public static bool Decrypt(string password, string cipher)
            {
                try
                {
                    if (cipher == null) throw new ArgumentNullException("cipher");

                    const int WorkFactor = 10;
                    var HashedPassword = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
                    if (BCrypt.Net.BCrypt.Verify(password, cipher) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //parse base64 string
                //byte[] data = Convert.FromBase64String(cipher);

                //decrypt data
                // byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                //return Encoding.Unicode.GetString(decrypted);
            }

            public static List<string> GetSrcInHTMLImgString(string htmlString)
            {
                List<string> srcs = new List<string>();
                string pattern = @"(?<=src="").*?(?="")";

                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(htmlString);

                for (int i = 0, l = matches.Count; i < l; i++)
                {
                    string d = matches[i].Value;
                    srcs.Add(d);
                }
                return srcs;
            }

            public static ActivityMsgResponse NotiticationBody(string Type, string ContactName, DateTime date, string activityTitle)
            {
                ActivityMsgResponse response = new ActivityMsgResponse();
                switch (Type)
                {
                    case "Call":
                        response.title = "Call Reminder";
                        response.desc = "Your have call with" + " " + ContactName + " at " + " " + date;
                        break;

                    case "Appointment":
                        response.title = "Appointment Reminder";
                        response.desc = "Your have Appointment with" + " " + ContactName + " at " + " " + date;
                        break;

                    case "Task":
                        response.title = "Task Reminder";
                        response.desc = activityTitle;
                        break;

                    case "Email":
                        response.title = "Email Reminder";
                        response.desc = activityTitle;
                        break;


                    case "FollowUp":
                        response.title = "FollowUp Reminder";
                        response.desc = activityTitle;
                        break;

                    default:

                        break;

                }

                return response;
            }

            public static string GetExtension(string charToUpper, Stream stream)
            {
                switch (charToUpper)
                {
                    case "IVBOR":
                        return ".jpg";

                    case "/9J/2":
                        return ".jpg";

                    case "/9J/4":
                        return ".png";

                    case "JVBER":
                        return ".pdf";
                    case "UESDB":
                        return GetExcelExtension(stream);//== ".xlsx" ? "xlsx" : ".docx";
                    /*if (GetExcelExtension(stream) == ".xlsx")
                    {
                        return ".xlsx";
                    }
                    else
                    {
                        return ".docx";
                    }*/

                    case "0M8R4":
                        return ".doc";

                    case "ZM9YI":
                        return ".txt";

                    case "AAAAI":
                        return ".mp4";

                    case "AAAAG":
                        return ".m4a";

                    case "SUQZA":
                        return ".mp3";

                    case "//VQR":
                        return ".mp3";

                    case "//VQX":
                        return ".wav";

                    case "UMFYI":
                        return ".rar";

                    case "TVQQA":
                        return ".exe";

                    case "ZMLYC":
                        return ".csv";

                    case "R0LGO":
                        return ".gif";

                    case "GKXFO":
                        return ".webm";

                    case "UKLGR":
                        return ".webp";

                    case "//PKZ":
                        return ".mp3";

                    default:
                        return string.Empty;
                }
            }
            public static string GetExcelExtension(Stream stream)
            {
                try
                {
                    using (var package = Package.Open(stream))
                    {
                        // Check if the package contains the required parts for an Excel file
                        if (package.PartExists(new Uri("/xl/workbook.xml", UriKind.Relative)))
                        {
                            return ".xlsx";
                        }
                        else { return ".docx"; }
                    }
                }
                catch (Exception)
                {
                    // Ignore any exceptions and return an empty string
                }
                return string.Empty;
            }

            public static T GetNext<T>(IEnumerable<T> list, T current)
            {
                try
                {
                    return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
                }
                catch
                {
                    return default(T);
                }
            }

            public static T GetPrevious<T>(IEnumerable<T> list, T current)
            {
                try
                {
                    return list.TakeWhile(x => !x.Equals(current)).Last();
                }
                catch
                {
                    return default(T);
                }
            }

            // this code does not work..
            /*private static string GetZipExtension(Stream stream)
            {
                try
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        // If the archive contains at least one .docx file, return the .docx extension
                        if (archive.Entries.Any(e => e.Name.EndsWith(".docx", StringComparison.OrdinalIgnoreCase)))
                        {
                            return ".docx";
                        }
                        // If the archive contains at least one .xlsx file, return the .xlsx extension
                        else if (archive.Entries.Any(e => e.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)))
                        {
                            return ".xlsx";
                        }
                        // If the archive does not contain any .docx or .xlsx files, return the .zip extension
                        else
                        {
                            return ".zip";
                        }
                    }
                }
                catch
                {
                    // If the file is not a valid zip archive, return the .zip extension
                    return ".zip";
                }
            }*/


            /*public static string GetExtension(string charToUpper)
            {
                switch (charToUpper)
                {
                    case "IVBOR":
                        return ".jpg";

                    case "/9J/4":
                        return ".png";

                    case "JVBER":
                        return ".pdf";

                    case "UESDB":
                        return TryOpenSpreadsheet(charToUpper) ? ".xlsx" : ".docx";

                    case "docx":
                        return ".xlxs";

                    case "0M8R4":
                        return ".doc";

                    case "ZM9YI":
                        return ".txt";

                    case "AAAAI":
                        return ".mp4";

                    case "AAAAG":
                        return ".m4a";

                    case "SUQZB":
                        return ".mp3";

                    case "//VQX":
                        return ".wav";
                    default:
                        return string.Empty;
                }
            */


            private static bool TryOpenSpreadsheet(string charToUpper)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(charToUpper);

                // create a MemoryStream from the byte array
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    // try to open the MemoryStream as a SpreadsheetDocument
                    try
                    {
                        using (SpreadsheetDocument document = SpreadsheetDocument.Open(ms, false))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        // if the MemoryStream cannot be opened as a SpreadsheetDocument, return false
                        return false;
                    }
                }

                /*try
                {
                    var bytes = Convert.FromBase64String(base64String);
                    using (var ms = new MemoryStream(bytes))
                    {
                        using (var document = SpreadsheetDocument.Open(ms, false))
                        {
                            // If the file is a valid .xlsx file, return true
                            return true;
                        }
                    }
                }
                catch
                {
                    // If an exception occurs while trying to open the file as a SpreadsheetDocument,
                    // or if the file is not a valid .xlsx file, return false
                    return false;
                }*/
            }
            public static bool IsValidListFile(List<string> file)
            {
                if (file != null && file.Count > 0) return true;

                else return false;
            }

            public static bool IsValidSingleFile(string file)
            {
                if (file != null && file != "" && file != "string") return true;

                else return false;
            }

            public class FileExtensionMap
            {
                public static readonly Dictionary<string, string> ExtensionMap = new Dictionary<string, string>
             {
                { ".jpg",   "Images" },
                { ".jpeg",  "Images" },
                { ".png",   "Images" },
                { ".gif",   "Images" },
                { ".pdf",   "Docs" },
                { ".docx",  "Docs" },
                { ".xlsx",  "Docs" },
                { ".txt",   "Docs" },
                { ".doc",   "Docs" },
                { ".zip",   "Docs" },
                { ".exe",   "Docs" },
                { ".csv",   "Docs" },
                { ".mp4",   "Videos" },
                { ".webm", "MediaTemplate"},
                { ".m4a", "Audios"},
                { ".mp3", "Audios"},
                { ".wav", "Audios"}

            };
                public static string GetCategoryForExtension(string extension)
                {
                    if (ExtensionMap.ContainsKey(extension))
                    {
                        return ExtensionMap[extension];
                    }
                    else
                    {
                        return "Others"; // Categorize as "Other" if extension is not in the map
                    }
                }
            }

            public static string DecryptString(string encrString)
            {
                byte[] b;
                string decrypted;
                try
                {
                    b = Convert.FromBase64String(encrString);
                    decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
                }
                catch (FormatException fe)
                {
                    decrypted = "";
                }
                return decrypted;
            }

            public static string EnryptString(string strEncrypted)
            {
                byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
                string encrypted = Convert.ToBase64String(b);
                return encrypted;
            }

            public static string GenerateTemporaryPassword(int length)
            {
                const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$*!";
                byte[] randomBytes = new byte[length];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }

                StringBuilder password = new StringBuilder(length);

                foreach (byte b in randomBytes)
                {
                    password.Append(validChars[b % validChars.Length]);
                }

                return password.ToString();
            }

            public static string RemoveSpecialCharacters(string str)
            {
                return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            }

            public static string GenerateUniqueToken()
            {
                return new Guid(Guid.NewGuid().ToString()).ToString();
            }

            public static readonly List<string> darkColors = new List<string>
{
    "#6D3F24",
    "#2E2E2E",
    "#3C3F41",
    "#1A1A1A",
    "#292929",
    "#363636",
    // Add more dark colors as needed
};

            // Function to generate a random dark hex color from the predefined list
            public static string GetRandomDarkColor()
            {
                int r = random.Next(0, 128);
                int g = random.Next(0, 128);
                int b = random.Next(0, 128);

                return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
            }
        }

        //public static class EmailUtils
        //{
        //    private readonly IConfiguration _configuration;

        //    public EmailUtils(IConfiguration configuration)
        //    {
        //        _configuration = configuration;
        //    }
        //    public static string GenerateUnsubscribeLink(string token)
        //    {
        //        string unsubscribeUrl = $"{_configuration.GetValue<string>("BaseUrl")}contact/unsubscribe?tkn={token}";
        //        string unsubscribeLink = $@"<p style=""text-align: center;"">
        //                                    <a href=""{unsubscribeUrl}"">Unsubscribe</a>
        //                                </p>";
        //        return unsubscribeLink;
        //    }
        //}

        public static class CreditCardTypeDetector
        {
            public enum CardType
            {
                Unknown,
                AmericanExpress,
                DinersClub,
                Discover,
                EnRoute,
                ECheckNet,
                JCB,
                MasterCard,
                PayPal,
                Visa

            }

            public static CardType GetCardType(string creditCardNumber)
            {
                if (IsAmericanExpress(creditCardNumber)) return CardType.AmericanExpress;
                if (IsDinersClub(creditCardNumber)) return CardType.DinersClub;
                if (IsDiscover(creditCardNumber)) return CardType.Discover;
                if (IsEnRoute(creditCardNumber)) return CardType.EnRoute;
                if (IsECheckNet(creditCardNumber)) return CardType.ECheckNet;
                if (IsJCB(creditCardNumber)) return CardType.JCB;
                if (IsMasterCard(creditCardNumber)) return CardType.MasterCard;
                if (IsPayPal(creditCardNumber)) return CardType.PayPal;
                if (IsVisa(creditCardNumber)) return CardType.Visa;

                return CardType.Unknown;
            }

            private static bool IsAmericanExpress(string creditCardNumber) =>
                (creditCardNumber.StartsWith("34") || creditCardNumber.StartsWith("37")) && creditCardNumber.Length == 15;

            private static bool IsDinersClub(string creditCardNumber) =>
                (creditCardNumber.StartsWith("300") || creditCardNumber.StartsWith("301") ||
                 creditCardNumber.StartsWith("302") || creditCardNumber.StartsWith("303") ||
                 creditCardNumber.StartsWith("304") || creditCardNumber.StartsWith("305") ||
                 creditCardNumber.StartsWith("36") || creditCardNumber.StartsWith("38")) && creditCardNumber.Length == 14;

            private static bool IsDiscover(string creditCardNumber) =>
                creditCardNumber.StartsWith("6011") && creditCardNumber.Length == 16;

            private static bool IsEnRoute(string creditCardNumber) =>
                (creditCardNumber.StartsWith("2014") || creditCardNumber.StartsWith("2149")) && creditCardNumber.Length == 15;

            private static bool IsECheckNet(string creditCardNumber) =>
                creditCardNumber.StartsWith("9") && (creditCardNumber.Length == 16 || creditCardNumber.Length == 17 || creditCardNumber.Length == 18 || creditCardNumber.Length == 19);

            private static bool IsJCB(string creditCardNumber) =>
                (creditCardNumber.StartsWith("3088") || creditCardNumber.StartsWith("3096") ||
                 creditCardNumber.StartsWith("3112") || creditCardNumber.StartsWith("3158") ||
                 creditCardNumber.StartsWith("3337") || creditCardNumber.StartsWith("3528")) && (creditCardNumber.Length == 16 || creditCardNumber.Length == 17 || creditCardNumber.Length == 18 || creditCardNumber.Length == 19);

            private static bool IsMasterCard(string creditCardNumber) =>
                (int.Parse(creditCardNumber.Substring(0, 2)) >= 51 && int.Parse(creditCardNumber.Substring(0, 2)) <= 55) && creditCardNumber.Length == 16;

            private static bool IsPayPal(string creditCardNumber) =>
                creditCardNumber.StartsWith("303") && creditCardNumber.Length == 16;

            private static bool IsVisa(string creditCardNumber) =>
                creditCardNumber.StartsWith("4") && (creditCardNumber.Length == 13 || creditCardNumber.Length == 16);
        }


        public class CsvDataMap : ClassMap<CsvData>
        {
            public CsvDataMap()
            {
                Map(m => m.firstName).Name("firstName", "Name"); // Map "firstName" or "Name" to firstName property
                Map(m => m.lastName).Name("lastName"); // Map "lastName" to lastName property
                Map(m => m.phone).Name("phone", "AgentPhone"); // Map "phone" or "AgentPhone" to phone property
                Map(m => m.mobile).Name("mobile"); // Map "mobile" to mobile property
                Map(m => m.email).Name("email"); // Map "email" to email property
            }
        }
        public class CsvData
        {
            public string firstName
            { get; set; }
            public string lastName
            { get; set; }
            public string phone
            { get; set; }
            public string mobile

            { get; set; }
            public string email
            { get; set; }
        }
    
}
