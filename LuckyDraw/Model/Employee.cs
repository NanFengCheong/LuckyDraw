using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Model
{
    public class Employee
    {
        [Key]
        public string WWID { get; set; }
        public string Name { get; set; }
    }
}
