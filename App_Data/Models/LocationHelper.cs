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
            string cleanContent = "";
            content = content.Replace(Environment.NewLine, "bitti");
            cleanContent = content.Substring(content.IndexOf("My Track,0,0,2,8421376bitti0bitti") + ("My Track,0,0,2,8421376bitti0bitti").Length);
            int start = cleanContent.IndexOf(",0,");
            List<PointD> pointList = new List<PointD>();
            while (start > 0)
            {
                pointList.Add(GetPointFByText(cleanContent.Substring(0, start)));
                int end = cleanContent.IndexOf("bitti");
                if (end > 0)
                {
                    end += 5;
                    cleanContent = cleanContent.Remove(0, end);
                    start = cleanContent.IndexOf(",0,");
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
            pf.Y = decimal.Parse(content.Substring(start + 1, end - start - 1));
            return pf;
        }
    }
}
