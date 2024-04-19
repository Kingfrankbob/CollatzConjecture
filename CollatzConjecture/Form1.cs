using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CollatzConjecture
{
    public partial class Form1 : Form
    {
        private const int ITERATIONS = 2500;
        private const int ANGLE = 8;
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

        private void collatzBox_Paint(object sender, PaintEventArgs e)
        {
            foreach (var pointsList in pointsDict)
            {
                if (pointsList.Count == 0)
                    continue;

                double prevX = 000;
                double prevY = 000;

                double rotationAngle = 0;

                for (int j = 0; j < Math.Min(step, pointsList.Count - 1); j++)
                {
                    double x2 = prevX;
                    double y2 = prevY;

                    rotationAngle = (pointsList[j] % 2 == 0) ? rotationAngle += ANGLE : rotationAngle -= ANGLE;

                    RotateAndMoveForward(ref x2, ref y2, rotationAngle, 200);

                    //Console.WriteLine($"Prev X: {prevX} Prev Y: {prevY} X2: {x2} Y2: {y2} step: {step} point: {pointsList[j]}");

                    float prevXMapped = Map((float)prevX, -5000, 5000, 0, collatzBox.Width);
                    float prevYMapped = Map((float)prevY, -5000, 5000, 0, collatzBox.Height);
                    float x2Mapped = Map((float)x2, -5000, 5000, 0, collatzBox.Width);
                    float y2Mapped = Map((float)y2, -5000, 5000, 0, collatzBox.Height);

                    e.Graphics.DrawLine(Pens.Black, x2Mapped, y2Mapped, prevXMapped, prevYMapped);
                    //e.Graphics.DrawEllipse(Pens.Red, prevXMapped - 1, prevYMapped - 1, 2, 2);

                    prevX = x2;
                    prevY = y2;
                }
            }
        }

        private static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        private static void RotateAndMoveForward(ref double x, ref double y, double angle, int distance)
        {
            double angleRadians = angle * Math.PI / 180; // Convert degrees to radians

            double deltaX = distance * Math.Cos(angleRadians);
            double deltaY = distance * Math.Sin(angleRadians);

            double newX = x + deltaX;
            double newY = y + deltaY;

            x = newX;
            y = newY;
        }

        private static List<int> Collatz(int n)
        {
            var pointsList = new List<int>();
            while (n != 1)
            {
                pointsList.Add(n);
                if (n % 2 == 0)
                {
                    n /= 2;
                }
                else
                {
                    n = 3 * n + 1;
                }
            }
            pointsList.Add(n);
            pointsList.Reverse();
            return pointsList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 5; i <= ITERATIONS; i++)
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
