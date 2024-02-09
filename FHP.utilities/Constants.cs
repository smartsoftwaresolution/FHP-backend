using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.utilities
{
    public class Constants
    {
        public const string DateFormat = "MM/dd/yyyy";

        public const int DefaultPageSize = 10;

        //public const string baseUrl = "https://localhost:5001/attachments/";
        //public const string baseSiteUrl = "https://localhost:5001/api/";




        // development server 5000
        //public const string baseUrl = "https://www.contactaholic.com:5000/attachments/";
        //public const string baseSiteUrl = "https://www.contactaholic.com:5000/api/";
        //public static string siteUrl = "https://www.contactaholic.com:5000/";
        //public static string pixelTrackingUrl = "https://www.contactaholic.com:5000/pixel.gif";


        //public const string baseUrl = "https://contactaholic.com:5000/attachments/";
        //public const string baseSiteUrl = "https://contactaholic.com:5000/api/";
        //public static string siteUrl = "https://contactaholic.com:5000/";
        //public static string pixelTrackingUrl = "https://contactaholic.com:5000/pixel.gif";


        //for contactaholic(client) 4000
        //public const string baseUrl = "https://www.contactaholic.com:4000/attachments/";
        //public const string baseSiteUrl = "https://www.contactaholic.com:4000/api/";
        //public static string siteUrl = "https://www.contactaholic.com:4000/";
        //public static string pixelTrackingUrl = "https://www.contactaholic.com:4000/pixel.gif";

        //public const string baseUrl = "https://contactaholic.com:4000/attachments/";
        //public const string baseSiteUrl = "https://contactaholic.com:4000/api/";
        //public static string siteUrl = "https://contactaholic.com:4000/";
        //public static string pixelTrackingUrl = "https://contactaholic.com:4000/pixel.gif";



        public const string deleted = "record(s) deleted successfully!!";
        public const string added = "record added successfully!!";
        public const string paymentAdded = "payment done successfully!!";
        public const string updated = "record updated successfully!!";
        public const string moved = "record(s) moved successfully!!";
        public const string emailSent = "email sent successfully!!";
        public const string error = "error occurred";
        public const string status = "status changed";
        public const string fetch = "record(s) fetch successfully!!";
        public const string provideValues = "please provide proper values";
        public const string requestSendSuccess = "Request sent successfully!";


        public enum RecordStatus { Created, Active, Inactive, Deleted }
        public enum CallCampaignType { Sale, Dialable, Droppped }
        public enum DealStatus { Active, Won, Lost }
        public enum Label { Nothing, Sales, AS, All, Admin }
        public enum TransactionStatus { Success, Failed }
        public enum CustomerStatus { Nothing, New, Done, Lose, Approve, Reject }
        public enum PaymentType { Service, Bonus }
        public enum CustomFieldType { Others, Checkbox, File }
        public enum EmailInvitationStatus { Sent, Accepted, Rejected }
        public enum TemplateType { Voice, Video, SMS, Email, Agreement }
        public enum RecordingTranscriptionRequestStatus { None, Requested, Approved }
        public enum CallStatus
        {
            IN_CALL,
            CLOSER,
            WRAPUP,
            PAUSED,
            OFFLINE
        }
        public enum DistributionType { None, RoundRobin, FirstToClaim }
        public enum CallCampaignCallingStatus { Nothing, Created, CallDone }
        public enum PostType { }
        public enum UserOrClientTypePermission { None, User, Client }
        public enum DaysType { None, Today, ThisWeek, ThisMonth, LastMonth, LastThreeMonths }
        public enum CallCampaignCustomerFilter { All, Approved, Rejected }

        public enum DuplicateCriteria { none, OverWrite, Skip }
        public enum DuplicateCheck { none, Mobile, Email }
        public enum DuplicateRecord { none, Skipped, Overwrite }
        public enum IsImported { Pending, Processing, Processed }
        public enum EmailOpenType { None, Never, Once, Mulitple, All }
        public enum CallsMadeType { None, Never, Once, Mulitple }
        public enum ClickedLinkType { None, Never, Once, Mulitple } // email attachment
        public enum PaymentPaidStatus { Due, Paid, Partially }
        public enum PostCardTemplateType { PostCard, Letter }
        public struct UserType
        {
            public const string Admin = "Administrator";
            public const string Employee = "Employee";
        }

        public struct EmailTemplateType
        {
            public const string ToCustomerOnRegistration = "to_customer_on_registration.html";
            public const string ToAdminOnCustomerRegistration = "to_admin_on_customer_registration.html";
        }
        public const string SessionKeyName = "CompanyTenantId";
        public string SessionInfo_Name { get; private set; }

        public class TwilioCallStatus
        {
            public const string Initiated = "initiated";
            public const string Ringing = "ringing";
            public const string InProgress = "in-progress";
            public const string Completed = "completed";
        }





        /*public static class Global
        {
            /// <summary>
            /// Global variable storing important stuff.
            /// </summary>
            static string _HeaderData;

            /// <summary>
            /// Get or set the static important data.
            /// </summary>
            public static string HeaderData
            {
                get
                {
                    return _HeaderData;
                }
                set
                {
                    _HeaderData = value;
                }
            }
        }*/
    }
}
