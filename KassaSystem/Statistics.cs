using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    public class Statistics
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public Statistics(string name, int count)
        {
            Name = name;
            Count = count;  
        }

    }
}
