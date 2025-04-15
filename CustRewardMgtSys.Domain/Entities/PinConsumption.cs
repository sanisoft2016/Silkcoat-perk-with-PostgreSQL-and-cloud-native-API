using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Domain.Entities
{
    public class PinConsumption
    {
        [Key]
        public int PinId { get; set; }
        public PinCode? Pin { get; set; }
        [MaxLength(40)]
        public string PainterUserId { get; set; } = string.Empty;
        [ForeignKey("PainterUserId")]
        public ApplicationUser? PainterUser { get; set; }

        public DateTime ConsumptionDate { get; set; }
        [MaxLength(40)]
        public string? ConsumptionOperatedUserId { get; set; }
        [ForeignKey("ConsumptionOperatedUserId")]
        public ApplicationUser? ConsumptionOperatedUser { get; set; }
        public DateTime DateRewarded { get; set; }

    }
}
