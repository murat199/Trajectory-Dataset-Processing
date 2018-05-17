using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data
{
    [Serializable]
    public class ReduceModel
    {
        public ReduceModel()
        {
            PointRawList = new List<PointD>();
            PointReduceList = new List<PointD>();
        }
        public List<PointD> PointRawList { get; set; }
        public List<PointD> PointReduceList { get; set; }
        public TimeSpan TimeReduce { get; set; }
    }
}
