using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CustRewardMgtSys.Application.DTOs
{
    public class ProductUploadDto
    {
        public IFormFile ProductsFileToUpload { get; set; }
    }
}
