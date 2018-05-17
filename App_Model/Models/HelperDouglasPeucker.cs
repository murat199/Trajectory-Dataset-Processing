using App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Model.Models
{
    public static class HelperDouglasPeucker
    {
        public static List<PointD> DouglasPeuckerReduction(List<PointD> Points, Double Tolerance)
        {
            if (Points == null || Points.Count < 3)
                return Points;

            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            //The first and the last point cannot be the same
            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(Points, firstPoint, lastPoint,
            Tolerance, ref pointIndexsToKeep);

            List<PointD> returnPoints = new List<PointD>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }

            return returnPoints;
        }

        private static void DouglasPeuckerReduction(List<PointD> points, Int32 firstPoint, Int32 lastPoint, Double tolerance, ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = TripleDistance(points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        public static Double TripleDistance(PointD Point1, PointD Point2, PointD Point3)
        {
            Double area = Math.Abs(0.5 *
                                    (double.Parse(Point1.X.ToString()) * double.Parse(Point2.Y.ToString())
                                    + double.Parse(Point2.X.ToString()) * double.Parse(Point3.Y.ToString())
                                    + double.Parse(Point3.X.ToString()) * double.Parse(Point1.Y.ToString())
                                    - double.Parse(Point2.X.ToString()) * double.Parse(Point1.Y.ToString())
                                    - double.Parse(Point3.X.ToString()) * double.Parse(Point2.Y.ToString())
                                    - double.Parse(Point1.X.ToString()) * double.Parse(Point3.Y.ToString())));

            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            //Double area = 1;
            //Double area = Math.Abs(decimal.Parse("0.5") * decimal.Parse((Point1.X * Point2.Y) + (Point2.X * Point3.Y) + (Point3.X * Point1.Y) - (Point2.X * Point1.Y) - (Point3.X * Point2.Y) - (Point1.X * Point3.Y)));

            Double bottom = Math.Sqrt(Math.Pow(double.Parse(Point1.X.ToString()) - double.Parse(Point2.X.ToString()), 2) + Math.Pow(double.Parse(Point1.Y.ToString()) - double.Parse(Point2.Y.ToString()), 2));
            Double height = area / bottom * 2;

            return height;
        }
    }
}
