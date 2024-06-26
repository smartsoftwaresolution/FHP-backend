﻿using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.utilities
{
   
        public class BaseResponse<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public string Message { get; set; }
        }

        public class BaseResponseGet<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public int TotalCount { get; set; }
        }

        public class BaseResponsePagination<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public int TotalCount { get; set; }
            public string Message { get; set; }
        }

        public class BaseResponsePaginationMyList<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public int TotalCount { get; set; }
            public int DisconnectedTotalCount { get; set; }
            public T Settings { get; set; }
            public string Message { get; set; }
        }

        public class BaseResponseScheduledCalls<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public int TotalCount { get; set; }
            public int OverdueCount { get; set; }
            public int TodayCount { get; set; }
            public int CompletedCount { get; set; }
            public int UpcomingCount { get; set; }
            public string Message { get; set; }
        }


         public class BaseResponseAdd
         {
            public int StatusCode { get; set; }
           
            public string Message { get; set; }
         }
        public class BaseResponseAddResponse<T>
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }


        public class BaseResponseCount
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public int TotalCount { get; set; }
        }

        public class ResponseDashboardCounts<T>
        {

            public int StatusCode { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
          /*  public int TotalEmployee { get; set; }
            public int TotalEmployer { get; set; }
            public int TotalUser { get; set; }
            public int TotalJobPost { get; set; }
            public int TotalJobPostById { get; set; }
            public int TotalContract { get; set; }*/
          
        }

       
        public class BaseResponseDealCount
        {
            public int DealCount { get; set; }
            public decimal DealValue { get; set; }
            public decimal DealConversionRate { get; set; }

            public int ActiveDealCount { get; set; }
            public int WonDealCount { get; set; }
            public int LostDealCount { get; set; }
            public int StatusCode { get; set; }

            public string Message { get; set; }

            public decimal ActiveDealValue { get; set; }
            public decimal WonDealValue { get; set; }
            public decimal LostDealValue { get; set; }
        }

        public class BaseResponseCampaignPagination<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public int TotalCount { get; set; }
            public string Message { get; set; }
        }

        public class BaseResponseCampaignWithTimeSlot<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public int TotalCount { get; set; }
            public string Message { get; set; }
            public T TimeSlotDetails { get; set; }
        }

        public class BaseResponseCard<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public List<T> Cards { get; set; }
            public int TotalCount { get; set; }
            public string Message { get; set; }
        }

        public class TxnResponse
        {
            public decimal Amount { get; set; }
            public string TransactionId { get; set; }
            public string ContactName { get; set; }
            public string Description { get; set; }
            public string InvoiceNumber { get; set; }
            public int InvoiceId { get; set; }
            public string Token { get; set; }
            public string CustProfId { get; set; }
        }

        public class BaseResponseVerification
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string VerificationCode { get; set; }
        }

    
}
