using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace CollatzConjecture
{
    public partial class Form1 : Form
    {
        private const int ITERATIONS = 50;
        private List<List<int>> pointsDict = new List<List<int>>();
        private int step = 0;
        public Form1()
        {
            InitializeComponent();
            ExtraInit();
        }

        private void ExtraInit()
        {
            KeyPress += MainForm_KeyPress;
            Load += Form1_Load;
            collatzBox.Paint += collatzBox_Paint;
        }

        private List<int> Collatz(int n)
        {
            var pointsList = new List<int>();
            while (n != 1)
            {
                if (n % 2 == 0)
                {
                    n /= 2;
                    pointsList.Add(n);
                }
                else
                {
                    n = 3 * n + 1;
                    pointsList.Add(n);
                }
            }
            pointsList.Reverse();
            return pointsList;
        }

        private void collatzBox_Paint(object sender, PaintEventArgs e)
        {
            foreach (var pointsList in pointsDict)
            {
                if (pointsList.Count == 0)
                    continue;
                var prevX = collatzBox.Width / 2;
                var prevY = collatzBox.Height / 2;


                double rotationAngle = (3*Math.PI)/2 ;

                for (int j = 0; j < Math.Min(step, pointsList.Count - 1); j++)
                {
                    int x2 = prevX;
                    int y2 = prevY;

                    if (pointsList[j] % 2 == 0)
                    {
                        rotationAngle += (Math.PI / 24); // 8 degrees in radians
                    }
                    else
                    {
                        rotationAngle -= (Math.PI / 24); // 8 degrees in radians
                    }

                    RotateAndMoveForward(ref x2, ref y2, rotationAngle, 5); // Adjust the distance as needed

                    var rand = new Random();
                    Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));

                    e.Graphics.DrawLine(new Pen(Color.Black), prevX * 2, prevY * 2, x2 * 2, y2 * 2);
                    e.Graphics.DrawEllipse(new Pen(randomColor), x2 * 2, y2 * 2, 2, 2);

                    prevX = x2;
                    prevY = y2;
                }
            }
        }



        private static void Rotate(ref int x, ref int y, int xCenter, int yCenter, double angle)
        {
            double cosTheta = Math.Cos(angle);
            double sinTheta = Math.Sin(angle);

            // Translate the point so that the center becomes the origin
            double translatedX = x - xCenter;
            double translatedY = y - yCenter;

            // Perform the rotation
            double newX = translatedX * cosTheta - translatedY * sinTheta + xCenter;
            double newY = translatedX * sinTheta + translatedY * cosTheta + yCenter;

            // Update the original coordinates
            x = (int)newX;
            y = (int)newY;
        }

        private static void RotateAndMoveForward(ref int x, ref int y, double angle, int distance)
        {
            // Rotate the point around the origin
            double newX = x * Math.Cos(angle) - y * Math.Sin(angle);
            double newY = x * Math.Sin(angle) + y * Math.Cos(angle);

            // Move the rotated point forward
            x = (int)(newX + distance * Math.Cos(angle));
            y = (int)(newY + distance * Math.Sin(angle));
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= ITERATIONS; i++)
            {
                var pointsList = Collatz(i);
                pointsDict.Add(pointsList);
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                step++;
                collatzBox.Invalidate();
            }
        }
    }
}
