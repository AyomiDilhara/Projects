using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentDashboard.Models
{
    public class ProcessedResult
    {
        public int operatinalCount { get; set; }
        public int nonOperatinalCount { get; set; }
        public Dictionary<string, int> keyValues {get; set;}
        public List<string> keys { get; set; }
    }
}
