﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.EmployerDetail
{
    public class AddEmployerDetailModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NationalAddress { get; set; }
        public IFormFile? CertificateRegistrationURL { get; set; }
        public IFormFile? VATCertificateURL { get; set; }
        public string ContactId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string CompanyLogoURL { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string TypeOfBusiness { get; set; }
        public string PrincipalBusinessActivity { get; set; }
        public string PersonToContact { get; set; }
        public string WebAddress { get; set; }
    }
}
