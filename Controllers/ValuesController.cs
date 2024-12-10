using JiebaNet.Segmenter;
using Marathon.Data;
using Marathon.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiebaNet.Analyser;
using System.Linq;

namespace Marathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MarathonContext _context;
        public ValuesController(MarathonContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<SummaryData> Get()
        {
            return GetAgeData(null);
        }

        [HttpGet("{gender}")]
        public ActionResult<SummaryData> Get(int gender)
        {
            return GetAgeData(gender);
        }
        public ActionResult<SummaryData> GetAgeData(int? gender)
        {
            var newData = new SummaryData();
            List<namevalue> vals = new List<namevalue>();
            //词云数据列表
            List<wordtf> wordtfs = new List<wordtf>();
            //管理人员管理数据
            List<manage> manages = new List<manage>();

            newData.title = "赛事报名情况";
            var classes = _context.Marathonentity.ToList();
            var sighup=_context.Check.ToList();
            int group = classes.Count();
            newData.categories = new string[group];
            newData.data = new decimal[group];
			newData.len = new int[group];

            List<string> sentences = new List<string>();
            for (int i = 0; i < sighup.Count(); i++)
            {
                sentences.Add(sighup[i].Experience);
            }
			for (int i = 0; i < group; i++)
            {
                newData.categories[i] = classes[i].MarathonName;
                var average = _context.Check.Include(s=>s.Marathonentity).Include(s => s.User).Where(s => s.Marathonentity.MarathonName == classes[i].MarathonName&& ((gender == null||gender==-1) ? true : (int)s.User.Gender == gender));
                newData.len[i] = average.Count();
				if (average.Count() == 0)
                { newData.data[i] = 0; }
                else
                    newData.data[i] = Math.Round((decimal)average.Average(s => s.User.Age), 2);
                vals.Add(new namevalue { name = newData.categories[i], value = newData.len[i] });
            }
            var dic = AnalyzeWordFrequency(sentences);
            foreach (var kvp in dic)
            {
                wordtfs.Add(new wordtf(kvp.Key, kvp.Value));
            }
            newData.wordtfs = wordtfs.OrderByDescending(n => n.value).Take(150).ToList<wordtf>();  //输出前200项
			newData.values = vals;
            //管理数据
            var a = _context.Manager.ToList();
            foreach (var manager in a)
            {
                var num = _context.Marathonentity.Include(s => s.Manager).Where(s => s.ManagerId == manager.ManagerId);
                manages.Add(new manage(manager.ManagerName, (num == null) ? 0 : num.Count()));
            }
            newData.manages = manages;
            return newData;
        }
        public static Dictionary<string, int> AnalyzeWordFrequency(List<string>sentences)
        {
            var wordFrequency = new Dictionary<string, int>();
            var segmenter = new JiebaSegmenter();
            foreach (var sentence in sentences)
            {
                var words = segmenter.Cut(sentence);
                foreach (var word in words)
                {
                    if (word.Length > 1)
                    {
                        if (wordFrequency.ContainsKey(word))
                        {
                            wordFrequency[word]++;
                        }
                        else
                        {
                            wordFrequency[word] = 1;
                        }
                    }
                    else;

                }
            }

            return wordFrequency;
        }


    }
}

