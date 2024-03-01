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
        public enum JobStatus { Submitted , Draft}
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
