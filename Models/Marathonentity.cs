using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marathon.Models
{
    public class Marathonentity
    {
        [Key]
        [Display(Name = "赛事编号")]
        public int MarathonId { get; set; }
        [Required]
        [Display(Name = "名称")]
        public string MarathonName { get; set; }
        [Required]
        [Display(Name = "日期")]
        public DateTime MarathonData { get; set; }
        [Required]
        [Display(Name = "地址")]
        public string MarathonPlace { get; set; }
        [Display(Name = "详情")]
        public string MarathonDetails { get; set; }
        [Display(Name = "赞助商")]
        public string MarathonSponsor { get; set; }
        [Column(TypeName = "varbinary(max)")]
        [Display(Name = "图片")]
        public byte[]? Image { get; set; }
        [Display(Name = "管理者")]
        [ForeignKey("Manager")]
        public String ManagerId { get; set; }
        public virtual Manager? Manager { get; set; }//导航属性


    }
}
