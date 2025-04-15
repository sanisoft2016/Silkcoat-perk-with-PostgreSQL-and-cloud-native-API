using CustRewardMgtSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.DTOs
{
    public class PinCodeDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string PaintCategoryName { get; set; } = string.Empty;
        public string BatchNo { get; set; } = string.Empty;

        public string Pin { get; set; } = string.Empty;
        public string PinStatus { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; }
    }
}
