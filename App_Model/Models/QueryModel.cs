using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data
{
    [Serializable]
    public class QueryModel
    {
        public QueryModel()
        {
            PointsRaw = new List<PointD>();
            PointsReduce = new List<PointD>();
            PointsRemainRaw = new List<PointD>();
            PointsRemainReduce = new List<PointD>();
            PointsAll = new List<PointMapModel>();
        }
        public decimal x1 { get; set; }
        public decimal x2 { get; set; }
        public decimal y1 { get; set; }
        public decimal y2 { get; set; }
        public List<PointD> PointsRaw { get; set; }
        public List<PointD> PointsReduce { get; set; }
        public List<PointD> PointsRemainRaw { get; set; }
        public List<PointD> PointsRemainReduce { get; set; }
        public List<PointMapModel> PointsAll { get; set; }
    }

    [Serializable]
    public class PointMapModel
    {
        public PointMapModel()
        {
            PointMap = new PointD();
        }
        public PointD PointMap { get; set; }
        public string FillColor { get; set; }
    }
}
