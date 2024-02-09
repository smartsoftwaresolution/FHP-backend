using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Service
{
    public interface IExceptionHandleService
    {
        Task<IActionResult> HandleException(Exception ex);
    }
}
