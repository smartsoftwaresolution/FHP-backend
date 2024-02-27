using FHP.entity.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FHP.factories.FHP
{
    public class EmployerDetailFactory
    {
        public static EmployerDetail Create(AddEmployerDetailModel model)
        {
            var data = new EmployerDetail
            {
                UserId = model.UserId,
                CompanyName=model.CompanyName,
                CompanyEmail=model.CompanyEmail,
                NationalAddress=model.NationalAddress,  
                CertificateRegistrationURL=model.CertificateRegistrationURL,
                VATCertificateURL=model.VATCertificateURL,
                ContactId=model.ContactId,
                CityId=model.CityId,
                CountryId=model.CountryId,
                StateId=model.StateId,
                CompanyLogoURL=model.CompanyLogoURL,
                Telephone=model.Telephone,
                Fax=model.Fax,      
                TypeOfBusiness=model.TypeOfBusiness,
                PrincipalBusinessActivity=model.PrincipalBusinessActivity,
                PersonToContact=model.PersonToContact,
                WebAddress=model.WebAddress,
                CreatedOn=Utility.GetDateTime(),
                Status=Constants.RecordStatus.Active,
            };

            return data;
        }

        public static void Update(EmployerDetail entity,AddEmployerDetailModel model)
        {
            entity.UserId= model.UserId;    
            entity.CompanyName= model.CompanyName;
            entity.CompanyEmail= model.CompanyEmail;
            entity.NationalAddress= model.NationalAddress;
            entity.CertificateRegistrationURL= model.CertificateRegistrationURL;
            entity.VATCertificateURL= model.VATCertificateURL;
            entity.ContactId= model.ContactId;
            entity.CityId= model.CityId;
            entity.CountryId= model.CountryId;
            entity.StateId= model.StateId;
            entity.CompanyLogoURL= model.CompanyLogoURL;
            entity.Telephone= model.Telephone;  
            entity.Fax= model.Fax;
            entity.TypeOfBusiness= model.TypeOfBusiness;
            entity.PrincipalBusinessActivity= model.PrincipalBusinessActivity;
            entity.PersonToContact= model.PersonToContact;
            entity.WebAddress= model.WebAddress;
            entity.UpdatedOn=Utility.GetDateTime();
        }
    }
}
