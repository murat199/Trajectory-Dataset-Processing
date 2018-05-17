using App_Data;
using App_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App_Server
{
    class Program
    {
        static Socket socketReduce;
        static NetworkStream streamReduce;
        static BinaryFormatter bfReduce = new BinaryFormatter();

        static Socket socketQuery;
        static NetworkStream streamQuery;
        static BinaryFormatter bfQuery = new BinaryFormatter();

        static void Main(string[] args)
        {
            Console.WriteLine("Server başlatıldı...");

            TcpListener listenerReduce = new TcpListener(new System.Net.IPEndPoint(IPAddress.Parse("10.40.198.197"),5555));
            //TcpListener listenerReduce = new TcpListener(5555);
            listenerReduce.Start();
            socketReduce = listenerReduce.AcceptSocket();
            streamReduce = new NetworkStream(socketReduce);
            Thread listenReduce = new Thread(ListenSocketReduce);
            listenReduce.Start();

            TcpListener listenerQuery = new TcpListener(new System.Net.IPEndPoint(IPAddress.Parse("10.40.198.197"), 5556));
            //TcpListener listenerQuery = new TcpListener(5556);
            listenerQuery.Start();
            socketQuery = listenerQuery.AcceptSocket();
            streamQuery = new NetworkStream(socketQuery);
            Thread listenQuery = new Thread(ListenSocketQuery);
            listenQuery.Start();
        }

        static void ListenSocketReduce()
        {
            while (socketReduce.Connected)
            {
                try
                {
                    ReduceModel modelClient = (ReduceModel)bfReduce.Deserialize(streamReduce);
                    if (modelClient != null)
                    {
                        ReduceModel modelServer = new ReduceModel();
                        modelServer.PointRawList = modelClient.PointRawList;
                        modelServer.PointReduceList = modelClient.PointRawList;
                        DateTime timeStartReduce = DateTime.Now;
                        modelServer.PointReduceList = HelperDouglasPeucker.DouglasPeuckerReduction(modelClient.PointRawList, 0.00001);
                        DateTime timeEndReduce = DateTime.Now;
                        modelServer.TimeReduce = timeEndReduce.Subtract(timeStartReduce);
                        bfReduce.Serialize(streamReduce, modelServer);
                        streamReduce.Flush();
                    }
                    //socketReduce.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("İstemci bağlantısı kesildi!");
                    break;
                }
            }
        }

        static void ListenSocketQuery()
        {
            while (socketQuery.Connected)
            {
                try
                {
                    QueryModel modelClient = (QueryModel)bfQuery.Deserialize(streamQuery);
                    if (modelClient != null)
                    {
                        QueryModel modelServer = new QueryModel();
                        modelServer = modelClient;
                        QuadTreeModel qtreeRaw = new QuadTreeModel();
                        QuadTreeModel qtreeReduce = new QuadTreeModel();
                        foreach (var item in modelServer.PointsRaw)
                        {
                            qtreeRaw.Add(item.X, item.Y);
                        }
                        foreach (var item in modelServer.PointsReduce)
                        {
                            qtreeReduce.Add(item.X, item.Y);
                        }
                        QueryRemainModel modelRemainRaw, modelRemainReduce;
                        if (qtreeRaw.root != null)
                        {
                            modelRemainRaw = new QueryRemainModel();
                            modelServer.PointsRemainRaw = modelRemainRaw.GetRemainPoints(qtreeRaw, modelServer.x1, modelServer.y1, modelServer.x2, modelServer.y2);
                        }
                        if (qtreeReduce.root != null)
                        {
                            modelRemainReduce = new QueryRemainModel();
                            modelServer.PointsRemainReduce = modelRemainReduce.GetRemainPoints(qtreeReduce, modelServer.x1, modelServer.y1, modelServer.x2, modelServer.y2);
                        }
                        bfQuery.Serialize(streamQuery, modelServer);
                        streamQuery.Flush();
                    }
                    //socketQuery.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("İstemci bağlantısı kesildi!");
                    break;
                }
            }
        }
    }
}
