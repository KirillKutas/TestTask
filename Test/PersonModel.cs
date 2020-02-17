using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class PersonModel
    {
        private int rank;
        private string user;
        private string status;
        private int steps;
        private int? maxSteps;
        private int? minSteps;
        [NonSerialized]
        private int? averageSteps;
        [NonSerialized]
        public List<int> AllSteps = new List<int>();
        public int Rank { get { return rank; } set { rank = value; } }
        public string User { get { return user; } set { user = value; } }
        public string Status { get { return status; } set { status = value; } }
        public int Steps { get { return steps; } set { steps = value; } }
        public int? MaxSteps { get { return maxSteps; } set { maxSteps = value; } }
        public int? MinSteps { get { return minSteps; } set { minSteps = value; } }
        public int? AverageSteps { get { return averageSteps; } set { averageSteps = value; } }

    }
}
