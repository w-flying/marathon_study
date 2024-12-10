using marathon.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marathon.Models
{
	public class Post
	{

		[Key]
		public int Post_Id { get; set; }
		[ForeignKey("User")]
		[Display(Name = "用户Id")]
		public string User_id { get; set; }
		[Required]
		[Display(Name = "内容")]
		public string Content { get; set; }
		[Required]
		[Display(Name = "创建时间")]
		public DateTime Create_time { get; set; }
		public virtual User? User { get; set; }
	}
}
