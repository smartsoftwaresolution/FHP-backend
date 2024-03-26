using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.UserManagement.FcmToken
{
    public class FcmTokenDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TokenFCM { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
