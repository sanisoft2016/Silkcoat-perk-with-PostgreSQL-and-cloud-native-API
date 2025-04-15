using CustRewardMgtSys.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Domain.Entities
{
    public class PinCode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public short PaintSubCategoryId { get; set; }
        [ForeignKey("PaintSubCategoryId")]
        public PaintSubCategory? PaintSubCategory { get; set; }

        [MaxLength(10)]
        public string BatchNo { get; set; } = string.Empty;
        [MaxLength(12)]
        public string Pin { get; set; } = string.Empty;
        public PIN_STATUS PinStatus { get; set; }
        public DateTime GeneratedDate { get; set; }

        public DateTime DateDispatched { get; set; }
        [MaxLength(40)]
        public string? DispatchedOperatedUserId { get; set; }
        [ForeignKey("DispatchedOperatedUserId")]
        public ApplicationUser? DispatchedOperatedUser { get; set; }
        public PinConsumption? PinConsumption { get; set; }

    }
}


