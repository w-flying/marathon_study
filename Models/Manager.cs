using System.ComponentModel.DataAnnotations;

namespace marathon.Models
{
    public class Manager
    {
        [Key]
        [Display(Name = "管理账号")]
        public string ManagerId { get; set; }
        [Required]
        [Display(Name = "密码")]
        public string PassWord { get; set; }
        [Required]
        [Display(Name = "姓名")]
        public string ManagerName { get; set; }

    }
}
