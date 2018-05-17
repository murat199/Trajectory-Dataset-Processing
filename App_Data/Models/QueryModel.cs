using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data
{
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
        public float x1 { get; set; }
        public float x2 { get; set; }
        public float y1 { get; set; }
        public float y2 { get; set; }
        public List<PointD> PointsRaw { get; set; }
        public List<PointD> PointsReduce { get; set; }
        public List<PointD> PointsRemainRaw { get; set; }
        public List<PointD> PointsRemainReduce { get; set; }
        public List<PointMapModel> PointsAll { get; set; }
    }
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
