using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.EmployerDetail
{
    public class EmployerDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NationalAddress { get; set; }
        public string CertificateRegistrationURL { get; set; }
        public string VATCertificateURL { get; set; }
        public string ContactId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string CompanyLogoURL { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string TypeOfBusiness { get; set; }
        public string PrincipalBusinessActivity { get; set; }
        public string PersonToContact { get; set; }
        public string WebAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
