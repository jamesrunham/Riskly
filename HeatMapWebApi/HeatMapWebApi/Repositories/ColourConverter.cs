using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using HeatMapWebApi.Models;

namespace HeatMapWebApi.Repositories
{
    public class ColourConverter
    {
        public double Percentage { get; private set; }
        //public int Height { get; private set; }

        public string GetColour(int totalCheckins, int nodeCheckins)
        {
            if (nodeCheckins > totalCheckins)
                throw new ArgumentOutOfRangeException("nodeCheckins cannot be higher than totalCheckins");

            //formula to create parabolic gradient, which distributes the colour
            Percentage = ParaCalculator(totalCheckins,nodeCheckins, 4) * 100;

            var colourDifference = byte.MaxValue * (Percentage / 100);
            double red;
            double green;

            if (Percentage < 50)
            {
                green = 255;
                red = colourDifference * 2;
            }
            else
            {
                Percentage -= 50;
                red = 255;
                green = (byte.MaxValue - colourDifference) * 2;
            }

            var colour = Color.FromArgb((int)red, (int)green, 0);

            return colour.Name;
        }

        public void ChildColours(NodeDto nodes, int checkins)
        {
            //var rootCheckins = checkins;
            if (nodes.children == null)
                return;

            foreach (var child in nodes.children)
            {
                var colours = new ColourConverter();
                child.data.colourHex = colours.GetColour(checkins, child.data.checkins).Substring(2, 6);
                child.data.percentage = ParaCalculator(checkins, child.data.checkins, 5.5)*150; //Math.Pow(((child.data.checkins / (double)checkins)), 1.0 / 5.5)*150;
                ChildColours(child, checkins);
            }
        }

        private static double ParaCalculator(int totalCheckins, int childCheckins, double root)
        {
            return Math.Pow(((childCheckins / (double)totalCheckins)), 1.0 / root);
        }
    }
}