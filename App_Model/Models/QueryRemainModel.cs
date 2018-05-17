using App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Model.Models
{
    public class QueryRemainModel
    {
        private void TravelTree(QuadNode node, ref List<PointD> points, decimal x1, decimal y1, decimal x2, decimal y2)
        {
            if (node.ChildGB != null)
            {
                TravelTree(node.ChildGB, ref points, x1, y1, x2, y2);
            }
            if (node.ChildGD != null)
            {
                TravelTree(node.ChildGD, ref points, x1, y1, x2, y2);
            }
            if (node.ChildKB != null)
            {
                TravelTree(node.ChildKB, ref points, x1, y1, x2, y2);
            }
            if (node.ChildKD != null)
            {
                TravelTree(node.ChildKD, ref points, x1, y1, x2, y2);
            }
            if ((Math.Min(x1, x2) <= node.lat && node.lat <= Math.Max(x1, x2)) && (Math.Min(y1, y2) <= node.lng && node.lng <= Math.Max(y1, y2)))
            {
                PointD pointModel = new PointD
                {
                    X = (node.lat),
                    Y = (node.lng)
                };
                points.Add(pointModel);
            }
        }

        public List<PointD> GetRemainPoints(QuadTreeModel qtree, decimal x1, decimal y1, decimal x2, decimal y2)
        {
            List<PointD> points = new List<PointD>();
            TravelTree(qtree.root, ref points, x1, y1, x2, y2);
            return points;
        }

        public QueryModel GetRemainPoints(QueryModel model)
        {
            return model;
        }
    }
}
