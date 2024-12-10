using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marathon.Models
{
    public class Check
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "报名人")]
        [ForeignKey("User")]
        public string UserName { get; set; }
        [Display(Name = "赛事编号")]
        [ForeignKey("Marathonentity")]
        public int MarathonId { get; set; }

        [Display(Name = "身体健康程度")]
        public Health Health { get; set; }
        [Display(Name = "报名类型")]
        public MarathonClass MarathonClass { get; set; }
        [Display(Name = "经历")]
        public string? Experience { get; set; }
        [Display(Name = "审核状态")]
        public string? CheckState { get; set; }
        public virtual User? User { get; set; }//导航属性
        public virtual Marathonentity? Marathonentity { get; set; }//导航属性
    }
}
