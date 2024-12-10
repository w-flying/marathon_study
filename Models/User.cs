using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marathon.Models
{
    public class User
    {
        [Key]
        [Display(Name = "账号")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "密码")]
        public string PassWord { get; set; }
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "性别")]
        public Gender Gender { get; set; }
        [Display(Name = "年龄")]
        public int Age { get; set; }

        [Display(Name = "手机号")]
        [StringLength(15, ErrorMessage = "长度不符,必须为{0}之{2}之间！", MinimumLength = 11)]
        public string PhoneNumber { get; set; }
		[Column(TypeName = "varbinary(max)")]
		[Display(Name = "头像")]
		public byte[]? Image { get; set; }

	}
}
