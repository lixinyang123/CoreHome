using System.Collections.Generic;

namespace CoreHome.Data.Model
{
    public class Year
    {
        public int Value { get; set; }

        public List<Month> Months { get; set; }
    }
}
