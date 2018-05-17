using App_Data;
using App_Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_Client_Desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        QueryModel model;

        TcpClient tcpClientReduce;
        NetworkStream streamReduce;
        BinaryFormatter bfReduce;

        TcpClient tcpClientQuery;
        NetworkStream streamQuery;
        BinaryFormatter bfQuery;

        string pathFile;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                model = new QueryModel();
                LoadGoogleMap();
                //192.168.43.166
                //10.40.198.197

                bfReduce = new BinaryFormatter();
                tcpClientReduce = new TcpClient("10.40.198.197", 5555);
                streamReduce = tcpClientReduce.GetStream();
                Thread listenReduce = new Thread(ListenSocketReduce);
                listenReduce.Start();

                bfQuery = new BinaryFormatter();
                tcpClientQuery = new TcpClient("10.40.198.197", 5556);
                streamQuery = tcpClientQuery.GetStream();
                Thread listenQuery = new Thread(ListenSocketQuery);
                listenQuery.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server bağlantısı kurunuz!");
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.x1 = 0;
            model.x2 = 0;
            model.y1 = 0;
            model.y2 = 0;
            ReloadGoogleMap(0);
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                pathFile = openFileDialog1.FileName;
                Thread readFile = new Thread(ReadFileLocation);
                readFile.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReloadGoogleMap(1);
            ReduceModel modelReduce = new ReduceModel();
            modelReduce.PointRawList = model.PointsRaw;

            bfReduce.Serialize(streamReduce, modelReduce);
            streamReduce.Flush();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(webBrowser1.Document.GetElementById("posStartX").OuterText!=null
               && webBrowser1.Document.GetElementById("posStartY").OuterText != null
               && webBrowser1.Document.GetElementById("posEndX").OuterText != null
               && webBrowser1.Document.GetElementById("posEndY").OuterText != null)
            {
                decimal x1 = decimal.Parse(webBrowser1.Document.GetElementById("posStartX").OuterText);
                decimal y1 = decimal.Parse(webBrowser1.Document.GetElementById("posStartY").OuterText);
                decimal x2 = decimal.Parse(webBrowser1.Document.GetElementById("posEndX").OuterText);
                decimal y2 = decimal.Parse(webBrowser1.Document.GetElementById("posEndY").OuterText);
                model.x1 = x1;
                model.x2 = x2;
                model.y1 = y1;
                model.y2 = y2;
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                label6.Text = "";
                label1.Text = "";
            }
            else
            {
                MessageBox.Show("Bölge seçmelisiniz!");
                return;
            }

            bfQuery.Serialize(streamQuery, model);
            streamQuery.Flush();

        }

        void ListenSocketReduce()
        {
            while (true)
            {
                try
                {
                    ReduceModel modelReduce = (ReduceModel)bfReduce.Deserialize(streamReduce);
                    model.PointsReduce = modelReduce.PointReduceList;
                    LoadGoogleMap();
                    int countReduce = modelReduce.PointReduceList.Count;
                    int countRaw = modelReduce.PointRawList.Count;
                    double ratioReduce = (1 - (Convert.ToDouble(countReduce) / Convert.ToDouble(countRaw))) * 100;
                    this.Invoke((MethodInvoker)(() =>
                        label4.Text = "count : " + listBox1.Items.Count.ToString()
                    ));
                    this.Invoke((MethodInvoker)(() =>
                        label2.Text = "İndirgeme Oranı : %" + ratioReduce.ToString("0.##")
                    ));
                    this.Invoke((MethodInvoker)(() =>
                        label3.Text = "İndirgeme süresi : " + modelReduce.TimeReduce.ToString()
                    ));
                    foreach (var item in model.PointsReduce)
                    {
                        this.Invoke((MethodInvoker)(() =>
                            listBox2.Items.Add(String.Format("{0} - {1}", item.X.ToString(), item.Y.ToString()))
                        ));
                    }
                    this.Invoke((MethodInvoker)(() =>
                        label5.Text = "count : " + listBox2.Items.Count.ToString()
                    ));
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Server bağlantısı koptu! Program kapatılacak...");
                    Environment.Exit(0);
                }
            }
        }

        void ListenSocketQuery()
        {
            while (true)
            {
                try
                {
                    QueryModel modelQuery = (QueryModel)bfQuery.Deserialize(streamQuery);
                    if (modelQuery != null)
                    {
                        model.PointsRemainRaw = modelQuery.PointsRemainRaw;
                        model.PointsRemainReduce = modelQuery.PointsRemainReduce;
                        foreach (var item in model.PointsRemainRaw)
                        {
                            this.Invoke((MethodInvoker)(() =>
                                listBox3.Items.Add(String.Format("{0} - {1}", item.X.ToString(), item.Y.ToString()))
                            ));
                        }
                        foreach (var item in model.PointsRemainReduce)
                        {
                            this.Invoke((MethodInvoker)(() =>
                                listBox4.Items.Add(String.Format("{0} - {1}", item.X.ToString(), item.Y.ToString()))
                            ));
                        }
                        this.Invoke((MethodInvoker)(() =>
                            label6.Text = "Count: " + listBox3.Items.Count
                        ));
                        this.Invoke((MethodInvoker)(() =>
                            label1.Text = "Count: " + listBox4.Items.Count
                        ));
                        LoadGoogleMap();
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        void LoadGoogleMap()
        {
            string html = MapHelper.GetMapHtml(model);
            webBrowser1.DocumentText = html;
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        void ReloadGoogleMap(int index)
        {
            if (index == 0)
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                if (model != null)
                {
                    model.PointsRaw.Clear();
                    model.PointsReduce.Clear();
                    model.PointsRemainRaw.Clear();
                    model.PointsRemainReduce.Clear();
                }
            }
            else if (index == 1)
            {
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                if (model != null)
                {
                    model.PointsReduce.Clear();
                    model.PointsRemainRaw.Clear();
                    model.PointsRemainReduce.Clear();
                }
            }
            else if (index == 2)
            {
                listBox3.Items.Clear();
                listBox4.Items.Clear();
            }
        }

        void ReadFileLocation()
        {
            string content = File.ReadAllText(@"" + pathFile);
            model.PointsRaw = LocationHelper.GetLocationsByPoint(content);
            foreach (var item in model.PointsRaw)
            {
                this.Invoke((MethodInvoker)(() =>
                    listBox1.Items.Add(item.X.ToString() + " , " + item.Y.ToString())
                ));
            }
            this.Invoke((MethodInvoker)(() =>
                label4.Text = "Count: " + listBox1.Items.Count.ToString()
            ));
            LoadGoogleMap();
        }
    }
}
