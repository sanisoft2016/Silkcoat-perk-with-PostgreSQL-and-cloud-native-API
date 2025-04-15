using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustRewardMgtSys.Domain.Entities
{
    public class PaintMainCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [MaxLength(50)]
        public string CatName { get; set; } = string.Empty;
        //[MaxLength(200)]
        //public string Description { get; set; } = string.Empty;
        public int NoOfCharacters { get; set; }
        public virtual List<PaintSubCategory>? PaintSubCategories { get; set; }

    }
}
