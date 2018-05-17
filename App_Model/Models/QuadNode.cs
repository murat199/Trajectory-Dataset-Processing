using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Model.Models
{
    public class QuadNode
    {
        public decimal lat;
        public decimal lng;

        //public double x1;
        //public double x2;
        //public double y1;
        //public double y2;

        public QuadNode ChildKD;
        public QuadNode ChildKB;
        public QuadNode ChildGD;
        public QuadNode ChildGB;

        public QuadNode(decimal lat, decimal lng)
        {
            this.lat = lat;
            this.lng = lng;

            this.ChildGB = null;
            this.ChildGD = null;
            this.ChildKB = null;
            this.ChildKD = null;
        }
    }
}
