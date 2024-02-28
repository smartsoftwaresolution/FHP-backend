using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.UserManagement
{
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public Constants.RecordStatus Status {  get; set; } 
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
    }
}
