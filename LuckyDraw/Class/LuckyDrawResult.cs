using LuckyDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Class
{
    public class LuckyDrawResult
    {
        public Prize Prize { get; set; }
        public List<Prize> Prizes { get; set; }
        public Employee Winner { get; set; }
        public List<Employee> Winners { get; set; }
    }
}