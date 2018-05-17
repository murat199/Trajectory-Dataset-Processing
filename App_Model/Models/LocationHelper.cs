using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data
{
    public static class LocationHelper
    {
        public static List<PointD> GetLocationsByPoint(string content)
        {
            var contentRows = content.Split('\n');
            int count = 0;
            List<PointD> pointList = new List<PointD>();
            foreach (var item in contentRows)
            {
                count++;
                if (count > 6 && count != contentRows.Length)
                {
                    PointD pointd = GetPointFByText(item);
                    pointList.Add(pointd);
                }
            }
            return pointList;
        }

        private static PointD GetPointFByText(string content)
        {
            PointD pf = new PointD();
            int start = content.IndexOf(",");
            int end = content.Length;
            pf.X = decimal.Parse(content.Substring(0, start));
            pf.Y = decimal.Parse(content.Substring(start + 1, content.Substring(start+1).IndexOf(',')));
            return pf;
        }
    }
}
