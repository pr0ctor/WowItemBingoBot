using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    internal class PointGridLocations
    {

        public Point startingPoint { get; set; }
        public int verticalOffset { get; set; }
        public int horizontalOffset { get; set; }

        public PointGridLocations()
        {

        }

        public Point GetHorizontalPoint(int horizontalIndex)
        {
            return new Point( (startingPoint.X + (horizontalIndex * horizontalOffset)) , startingPoint.Y);
        }

        public Point GetVerticalPoint(int verticalIndex)
        {
            return new Point(startingPoint.X, (startingPoint.Y + (verticalIndex * verticalOffset)));
        }

        public Point GetNewPoint(int horizontalIndex, int verticalIndex)
        {
            return new Point((startingPoint.X + (horizontalIndex * horizontalOffset)), (startingPoint.Y + (verticalIndex * verticalOffset)));
        }

        public List<List<Point>> GetSquareOfPoints(int horizontalCount, int verticalCount, bool skipCenter = false)
        {
            List<List<Point>> points = new();

            var centerX = Math.Floor((decimal)horizontalCount / 2);
            var centerY = Math.Floor((decimal)verticalCount / 2);

            for(int h = 0; h < horizontalCount; h++)
            {
                var rowpoints = new List<Point>();

                for (int v = 0; v < verticalCount; v++)
                {
                    if (skipCenter && v == centerY && h == centerX)
                    {
                        rowpoints.Add(new Point(-1, -1));
                        skipCenter = false;
                        continue;
                    }

                    rowpoints.Add(GetNewPoint(h, v));
                }

                points.Add(rowpoints);
            }


            /*Enumerable.Range(0, verticalCount).ToList().ForEach(v =>
            {
                

                foreach (var h in Enumerable.Range(1, horizontalCount))
                {
                    if(skipCenter && v == centerY && h == centerX)
                    {
                        rowpoints.Add(new Point(-1, -1));
                        skipCenter = false;
                        continue;
                    }

                    rowpoints.Add(GetNewPoint(h, v));
                }

                points.Add(rowpoints);
            });*/

            return points;
        } 
    }
}
