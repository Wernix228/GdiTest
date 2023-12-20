using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GdiTest
{
    internal class Drawer
    {
        private Image image;
        private Color32 color;
        public Drawer(Image image, Color32 color)
        {
            this.image = image;
            this.color = color;
        }
        public void drawLine(int x0, int y0, int x1, int y1)
        {
            int deltaX = x1 - x0;
            int deltaY = y1 - y0;
            int absDeltaX = Math.Abs(deltaX);
            int absDeltaY = Math.Abs(deltaY);

            float accretion = 0;

            if(absDeltaX >= absDeltaY)
            {
                int y = y0;
                int direction = deltaY != 0 ? (deltaY > 0 ? 1 : -1) : 0;
                 
                    for(int x = x0; deltaX > 0 ? x <= x1 : x >= x1;)
                    {
                        image.SetPixel(x, y, color);
                        accretion += (float)absDeltaY / absDeltaX;

                        if(accretion >= 1.0f)
                        {
                            accretion -= 1.0f;
                            y += direction;
                        }
                        if (deltaX > 0) x++; else x--;
                    }
            }
            else
            {
                int x = x0;
                int direction = deltaX != 0 ? (deltaX > 0 ? 1 : -1) : 0;

                    for (int y = y0; deltaY > 0 ? y <= y1 : y >= y1;)
                    {
                        image.SetPixel(x, y, color);
                        accretion += (float)absDeltaX / absDeltaY;

                        if (accretion >= 1.0f)
                        {
                            accretion -= 1.0f;
                            x += direction;
                        }
                        if (deltaY > 0) x++; else x--;
                    }
            }
        }
        public void fillTriangle(int x0, int y0, int x1, int y1, int x2, int y2)
        {
            Vector2[] box = findTriangleBoundingBox2D(new Vector2(x0,y0), new Vector2(x1, y1), new Vector2(x2, y2));

            for(int y = Convert.ToInt32(box[0].Y); y <= box[1].Y; y++)
            {
                for(int x = Convert.ToInt32(box[0].X); x <= box[1].X; x++)
                {
                    if (IsInTriangle(new Vector2(x, y), new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2)))
                    {
                        image.SetPixel(x,y,color);
                    }
                }
            }
        }
        private Vector2[] findTriangleBoundingBox2D(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Vector2[] result = new Vector2[2];

            float minX = Math.Min(Math.Min(p0.X, p1.X), p2.X);
            float minY = Math.Min(Math.Min(p0.Y, p1.Y), p2.Y);

            float maxX = Math.Max(Math.Max(p0.X, p1.X), p2.X);
            float maxY = Math.Max(Math.Max(p0.Y, p1.Y), p2.Y);

            result[0].X = minX; result[0].Y = minY;
            result[1].X = maxX; result[1].Y = maxY;

            return result;
        }
        private bool IsInTriangle(Vector2 p,Vector2 a, Vector2 b, Vector2 c)
        {
            int aSide = Convert.ToInt32((a.Y - b.Y)*p.X + (b.X - a.X)*p.Y + (a.X * b.Y - b.X * a.Y));
            int bSide = Convert.ToInt32((b.Y - c.Y) * p.X + (c.X - b.X) * p.Y + (b.X * c.Y - c.X * b.Y));
            int cSide = Convert.ToInt32((c.Y - a.Y) * p.X + (a.X - c.X) * p.Y + (c.X * a.Y - a.X * c.Y));

            return (aSide >= 0 && bSide >= 0 && cSide >= 0) || (aSide < 0 && bSide < 0 && cSide < 0);
        }
    }
}
