using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.UserManagement
{
    public  class AddStateModel
    {
       public int Id { get; set; } 
       public string StateName { get; set; }
       public int CountryId { get; set; } 
    }
}
