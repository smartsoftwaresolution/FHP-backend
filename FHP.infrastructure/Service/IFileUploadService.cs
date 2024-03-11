﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Service
{
    public interface IFileUploadService
    {
        Task<string> UploadIFormFileAsync(IFormFile file);
    }
}
