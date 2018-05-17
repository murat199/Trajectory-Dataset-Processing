using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Model.Models
{
    public class QuadTreeModel
    {
        public QuadNode root;
        public QuadTreeModel()
        {
            root = null;
        }
        public QuadTreeModel(decimal lat, decimal lng)
        {
            root = new  QuadNode(lat, lng);
        }

        public void Add(decimal lat, decimal lng)
        {
            if (root == null)
            {
                root = new QuadNode(lat, lng);
            }
            else
            {
                AddR(ref root, lat, lng);
            }
        }
        public void AddR(ref QuadNode node, decimal lat, decimal lng)
        {
            if (node == null)
            {
                QuadNode newNode = new QuadNode(lat, lng);
                node = newNode;
                return;
            }
            else
            {
                if (lat <= node.lat && lng <= node.lng)
                {
                    //KUZEY BATI
                    AddR(ref node.ChildKB, lat, lng);
                }
                else if (lat >= node.lat && lng <= node.lng)
                {
                    //GUNEY BATI
                    AddR(ref node.ChildGB, lat, lng);
                }
                else if (lat <= node.lat && lng >= node.lng)
                {
                    //KUZEY DOGU
                    AddR(ref node.ChildKD, lat, lng);
                }
                else if (lat >= node.lat && lng >= node.lng)
                {
                    //GUNEY DOGU
                    AddR(ref node.ChildGD, lat, lng);
                }
            }
        }
    }
}
