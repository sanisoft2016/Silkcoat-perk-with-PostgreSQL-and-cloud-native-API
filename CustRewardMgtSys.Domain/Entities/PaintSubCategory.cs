using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Domain.Entities
{
    public class PaintSubCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        public byte PaintMainCategoryId { get; set; }
        [ForeignKey("PaintMainCategoryId")]
        public PaintMainCategory? PaintMainCategory { get; set; }

        [MaxLength(50)]
        public string SubCatName { get; set; } = string.Empty;
        public float CoinValue { get; set; }
    }
}
