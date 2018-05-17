using App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Model.Models
{
    public static class MapHelper
    {
        public static String GetMapHtml(QueryModel model)
        {
            string result = "";
            string rawCoordinates = "";
            string reduceCoordinates = "";
            string queryRawCoordinates = "";
            string queryReduceCoordinates = "";
            string polyRawLineCoordinates = "var polyRawCoordinates=[";
            string polyReduceCoordinates = "var polyReduceCoordinates=[";
            string centerMap = "{lat: 41.879, lng: -87.624}";
            string bounds = "var bounds={";
            string rectangle = "";
            decimal x1 = 0, x2 = 0, y1 = 0, y2 = 0;

            foreach (var item in model.PointsRaw)
            {
                if (HasItemInList(model.PointsRemainRaw, item))
                {
                    PointMapModel mapPoint = new PointMapModel();
                    mapPoint.PointMap = item;
                    mapPoint.FillColor = "green";

                    rawCoordinates += "{center:{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                    rawCoordinates += "color:'green'},";

                    model.PointsAll.Add(mapPoint);
                }
                else
                {
                    if(HasItemInList(model.PointsReduce, item))
                    {
                        PointMapModel mapPoint = new PointMapModel();
                        mapPoint.PointMap = item;
                        mapPoint.FillColor = "red";

                        rawCoordinates += "{center:{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                        rawCoordinates += "color:'red'},";

                        model.PointsAll.Add(mapPoint);
                    }
                    else
                    {
                        PointMapModel mapPoint = new PointMapModel();
                        mapPoint.PointMap = item;
                        mapPoint.FillColor = "green";

                        rawCoordinates += "{center:{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                        rawCoordinates += "color:'#4285f4'},";

                        model.PointsAll.Add(mapPoint);
                    }
                }
                x1 = item.X;
                y1 = item.X;
                x2 = item.Y;
                y2 = item.Y;
                centerMap = "{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                polyRawLineCoordinates += "{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
            }
            foreach (var item in model.PointsReduce)
            {
                if (HasItemInList(model.PointsRemainReduce, item))
                {
                    PointMapModel mapPoint = new PointMapModel();
                    mapPoint.PointMap = item;
                    mapPoint.FillColor = "green";

                    rawCoordinates += "{center:{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                    rawCoordinates += "color:'green'},";

                    model.PointsAll.Add(mapPoint);
                }
                else
                {
                    PointMapModel mapPoint = new PointMapModel();
                    mapPoint.PointMap = item;
                    mapPoint.FillColor = "red";

                    rawCoordinates += "{center:{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                    rawCoordinates += "color:'red'},";

                    model.PointsAll.Add(mapPoint);
                }

                x1 = item.X;
                y1 = item.X;
                x2 = item.Y;
                y2 = item.Y;
                centerMap = "{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
                polyReduceCoordinates += "{" + String.Format("lat: {0}, lng: {1}", item.X.ToString(), item.Y.ToString()) + "},";
            }
            if (model.x1 != 0 && model.x2 != 0 && model.y1!=0 && model.y2 != 0)
            {
                rectangle += "var rectangle = new google.maps.Rectangle({strokeColor: '#FF0000',strokeOpacity: 0.8,strokeWeight: 2,fillColor: '#FF0000',fillOpacity: 0.35,map: map,bounds: {north:" + model.x1.ToString() + ",south:" + model.x2.ToString() + ",east:" + model.y2.ToString() + ",west:" + model.y1.ToString() + "}}); ";
            }
            polyReduceCoordinates += "];";
            polyRawLineCoordinates += "];";
            bounds += String.Format("north:{0},south:{1},east:{2},west:{3}", x1.ToString("0.######"), y1.ToString("0.######"), x2.ToString("0.######"), y2.ToString("0.######")) + "};";
            result += "<!DOCTYPE html><html><head><meta name=\"viewport\" content=\"initial-scale=1.0,user-scalable=no\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta charset=\"utf-8\"><title>Complex Polylines</title><style>#map {height: 100%;}html, body {height: 100%;margin: 0;padding: 0;}</style></head><body><input type=\"hidden\" id=\"posStartX\" value=\"\"/><input type=\"hidden\" id=\"posStartY\" value=\"\"/><input type=\"hidden\" id=\"posEndX\" value=\"\" /><input type=\"hidden\" id=\"posEndY\" value=\"\"/><button id=\"btnDrawRectangle\" onclick=\"DrawRectangle()\"> KARE ÇİZ</Button><div id=\"map\"></div><script>var poly;var map;var lineSymbol = { path: 'M 0,-1 0,1', strokeOpacity: 1, scale: 4};var flightPlanCoordinates = [";
            result += rawCoordinates;
            result += "];";
            result += polyReduceCoordinates + polyRawLineCoordinates;
            result += "var reducePlanCoordinates=[";
            result += reduceCoordinates;
            result += "];var queryReduceCoordinates=[";
            result += queryReduceCoordinates + "];var queryRawCoordinates=[";
            result += queryRawCoordinates + "];";
            result += bounds + "function initMap() {map = new google.maps.Map(document.getElementById('map'), {zoom: 12,scrollwheel: true,center: ";
            result += centerMap;
            result += "});"+rectangle+"var flightPath1=new google.maps.Polyline({path: polyRawCoordinates, geodesic: true, strokeColor: '#4285f4', strokeOpacity: 1.0, strokeWeight: 2}); flightPath1.setMap(map); var flightPath2=new google.maps.Polyline({path: polyReduceCoordinates,icons: [{icon: lineSymbol, offset: '0', repeat: '20px'}], geodesic: true, strokeColor: 'red', strokeOpacity: 1.0, strokeWeight: 2}); flightPath2.setMap(map); for (var city in flightPlanCoordinates) {var cityCircle = new google.maps.Circle({strokeColor: flightPlanCoordinates[city].color,strokeOpacity: 0.8,strokeWeight: 2,fillColor: flightPlanCoordinates[city].color,fillOpacity: 1,map: map,center: flightPlanCoordinates[city].center,radius: 1});}}function DrawRectangle(){rectangle = new google.maps.Rectangle({ bounds: bounds, editable: true, draggable: true }); rectangle.setMap(map);rectangle.addListener('bounds_changed', showNewRect);infoWindow = new google.maps.InfoWindow();}function showNewRect(event) {var ne = rectangle.getBounds().getNorthEast(); var sw = rectangle.getBounds().getSouthWest();document.getElementById(\"posStartX\").innerHTML=ne.lat();document.getElementById(\"posStartY\").innerHTML=sw.lng();document.getElementById(\"posEndX\").innerHTML =sw.lat();document.getElementById(\"posEndY\").innerHTML=ne.lng(); infoWindow.open(map); }</script><script src=\"https://maps.googleapis.com/maps/api/js?key=AIzaSyAQs92sutIepGn4IfuCfJf61WaZ3Va5piQ&callback=initMap\"></script></body></html>";
            return result;
        }

        private static Boolean HasItemInList(List<PointD> points, PointD search)
        {
            if (points != null)
            {
                foreach (var item in points)
                {
                    if (item.X == search.X && item.Y == search.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
