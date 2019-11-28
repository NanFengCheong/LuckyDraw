using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Model
{
    public class Prize
    {
        [Key]
        public int PrizeID { get; set; }

        public string PrizeName { get; set; }
        public int Order { get; set; }
        public string WWID { get; set; }
        public DateTimeOffset? DrawTime { get; set; }
        public DateTimeOffset? CollectTime { get; set; }
        public bool IsCollected { get { return CollectTime != null; } }

        [ForeignKey("WWID")]
        public Employee Employee { get; set; }
    }
}