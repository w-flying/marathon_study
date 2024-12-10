using marathon.Models;
using System.ComponentModel.DataAnnotations;

namespace Marathon.Views
{

    public class SummaryData
    {
        public string title { get; set; }
        public string[] categories { get; set; }
        public decimal[] data { get; set; }
		public int [] len { get; set; }
		public List<namevalue> values { get; set; }
        public List<wordtf> wordtfs { get; set; }
        public List<manage> manages { get; set; }


    }
    public class manage
    {
		public manage(string name,int managenum)
		{
            this.name = name;
            this .managenum = managenum;
		}

		[Key]
        public string name { get; set; }
        public int managenum { get; set; }
    }
    public class namevalue
    {
        [Key]
        public string name { get; set; }
        public decimal value { get; set; }
    }
    public class wordtf
    {
        [Key]
        public string name{ get; set; }
        public int value { get; set; }
        public wordtf(string word,int tf)
        {
            this.name = word;
                this.value = tf;
        }
    }
}
