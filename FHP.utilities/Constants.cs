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



        public enum JobPosting { Draft, Submitted, Cancel }
        public enum JobOperationStatus { Shortlisting, InProcess, Reviewing,Hired }
        public enum EmployeeAvailability { Pending, Available, NotAvailable }
        public enum JobProcessingStatus { None,ShortListing, InProcess, Reviewing,Hired }
        public enum RecordStatus { Created, Active, Inactive, Deleted }
        
       
        public string SessionInfo_Name { get; private set; }

    }
}
