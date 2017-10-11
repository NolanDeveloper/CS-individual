using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DelaunayTriangulation
{
    class TriangluationPlotter : UserControl
    {
        private class Vector
        {
            private float x;
            private float y;

            public float X { get { return x; } set { x = value; } }
            public float Y { get { return y; } set { y = value; } }
            public float Len { get { return (float)Math.Sqrt(x * x + y * y); } }

            public Vector(float x, float y) { X = x; Y = y; }

            public static Vector Between(PointF a, PointF b) { return new Vector(b.X - a.X, b.Y - a.Y); }
            public static Vector operator +(Vector a, Vector b) { return new Vector(a.X + b.X, a.Y + b.Y); }
            public static Vector operator -(Vector a, Vector b) { return a + (-1) * b; }
            public static Vector operator *(Vector a, float b) { return new Vector(b * a.X, b * a.Y); }
            public static Vector operator *(float a, Vector b) { return b * a; }
            public static Vector operator /(Vector a, float b) { return a * (1 / b); }
            public static float DotProduct(Vector a, Vector b) { return a.X * b.X + a.Y * b.Y; }
            public static float CosBetween(Vector a, Vector b) { return DotProduct(a, b) / (a.Len * b.Len); }
        }

        private class PointComparer : IComparer<PointF>
        {
            public static PointComparer INSTANCE = new PointComparer();

            private PointComparer() { }

            public int Compare(PointF a, PointF b)
            {
                var t = (int)(1000 * (a.X - b.X));
                if (0 != t) return t;
                return (int) (1000 * (b.Y - a.Y));
            }
        }

        private SortedSet<PointF> points = new SortedSet<PointF>(PointComparer.INSTANCE);
        private SortedSet<PointF> selectedPoints = new SortedSet<PointF>(PointComparer.INSTANCE);

        public TriangluationPlotter()
        {
            var flags = ControlStyles.AllPaintingInWmPaint
                      | ControlStyles.DoubleBuffer
                      | ControlStyles.UserPaint;
            SetStyle(flags, true);
            Invalidate();
        }

        private void RemoveAt(PointF c)
        {
            var nearest = points
                .OrderBy(p => Vector.Between(c, p).Len)
                .First();
            var distance = Vector.Between(c, nearest).Len;
            if (20 < distance) return;
            points.Remove(nearest);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ModifierKeys.HasFlag(Keys.Shift))
                RemoveAt(e.Location);
            else
                points.Add(e.Location);
            Invalidate();
        }

        private void DrawDiamond(Graphics g, PointF p, float s)
        {
            PointF[] diamond = new PointF[4];
            diamond[0] = new PointF(p.X, p.Y - s);
            diamond[1] = new PointF(p.X + s, p.Y);
            diamond[2] = new PointF(p.X, p.Y + s);
            diamond[3] = new PointF(p.X - s, p.Y);
            g.FillPolygon(Brushes.White, diamond);
            g.DrawPolygon(Pens.Black, diamond);
        }

        private static HashSet<Tuple<PointF, PointF>> Triangulate(SortedSet<PointF> points)
        {
            var triangulation = new HashSet<Tuple<PointF, PointF>>();
            var activeEdges = new HashSet<Tuple<PointF, PointF>>();
            {
                var a = points.Min;
                var startVector = new Vector(0, -1);
                var b = points
                    .Where(x => x != a)
                    .OrderByDescending(x =>
                        Vector.CosBetween(
                            startVector,
                            Vector.Between(a, x)))
                    .First();
                activeEdges.Add(new Tuple<PointF, PointF>(a, b));
            }
            while (0 != activeEdges.Count())
            {
                var minCos = 1.0f;
                Tuple<PointF, PointF> minEdge = null;
                PointF minPoint = new PointF(0, 0);
                List<Tuple<PointF, PointF>> fixedEdges = new List<Tuple<PointF, PointF>>();
                foreach (var edge in activeEdges)
                {
                    var a = edge.Item1;
                    var b = edge.Item2;
                    var dx = b.X - a.X;
                    var dy = b.Y - a.Y;
                    var pointsByAngle = points
                        .Where(x =>
                            x != a && x != b &&
                            0 < -dy * x.X + dx * x.Y - dx * a.Y + dy * a.X)
                        .OrderBy(x =>
                            Vector.CosBetween(
                                Vector.Between(x, a),
                                Vector.Between(x, b)));
                    if (0 == pointsByAngle.Count())
                    {
                        fixedEdges.Add(edge);
                        continue;
                    }
                    var c = pointsByAngle.First();
                    var cos = Vector.CosBetween(
                        Vector.Between(c, a),
                        Vector.Between(c, b));
                    if (minCos < cos) continue;
                    minCos = cos;
                    minEdge = edge;
                    minPoint = c;
                }
                foreach (var edge in fixedEdges)
                {
                    triangulation.Add(edge);
                    activeEdges.Remove(edge);
                }
                if (null == minEdge) break;
                activeEdges.Remove(minEdge);
                triangulation.Add(minEdge);
                var e1 = new Tuple<PointF, PointF>(minEdge.Item1, minPoint);
                if (!triangulation.Contains(e1)) activeEdges.Add(e1);
                var e2 = new Tuple<PointF, PointF>(minPoint, minEdge.Item2);
                if (!triangulation.Contains(e2)) activeEdges.Add(e2);
            }
            return triangulation;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(SystemColors.Control);
            if (2 <= points.Count)
                foreach (var edge in Triangulate(points))
                    g.DrawLine(Pens.Black, edge.Item1, edge.Item2);
            foreach (var p in points)
                DrawDiamond(g, p, 4);
        }
    }
}
