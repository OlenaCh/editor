using Gdk;
using Gtk;
using Cairo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphEditor {
    using Algorithms;
    using Graphs;
    using Strings;

    partial class Editor : Gtk.Window {
        void drawGraph() {
            if (FileRead) { buildGraphCoordinates(); FileRead = false; }

            Cairo.Context cr = Gdk.CairoHelper.Create(area.GdkWindow);
            drawBackground(cr);

            cr.SelectFontFace("Purisa", FontSlant.Normal, FontWeight.Bold);
            cr.SetFontSize(16);

            drawEdges(cr);
            drawVertices(cr);

            ((IDisposable)cr.Target).Dispose();
            ((IDisposable)cr).Dispose();
        }

        void drawBackground(Cairo.Context cr) {
            int width = Allocation.Width, height = Allocation.Height;
            cr.Rectangle(0, 0, width, height);
            cr.SetSourceRGB(0.93, 0.93, 0.93);
            cr.Fill();
        }

        void drawEdges(Cairo.Context cr) {
            IEnumerable<int> vertices = graph.vertices();
            IEnumerable<(int, int)> visited = null;
            IEnumerable<(int, int)> frontier = null;

            if (algorithmRunning) {
                visited = algorithm.visitedEdges();
                frontier = algorithm.frontierEdges();
            }

            if (settings[Strings.EDGE_WEIGHTS])
                edgeWeightsPos = new Dictionary<string, PointD>();

            foreach (var v in vertices) {
                IEnumerable<int> neighbors = graph.neighbors(v);
                foreach(var n in neighbors) {
                    var color = edgeColor(v, n, visited, frontier);
                    drawEdge(
                        cr,
                        pos[v],
                        pos[n],
                        color.Red,
                        color.Green,
                        color.Blue
                    );

                    if (settings[Strings.DIRECTED]) {
                        PointD tmp = arrowTip(pos[n], pos[v], 14.0);
                        drawEdge(
                            cr,
                            tmp,
                            arrowCorner(pos[n], pos[v], 14.0, -1.0),
                            color.Red,
                            color.Green,
                            color.Blue
                        );
                        drawEdge(
                            cr,
                            tmp,
                            arrowCorner(pos[n], pos[v], 14.0, 1.0),
                            color.Red,
                            color.Green,
                            color.Blue
                        );
                    }

                    if (settings[Strings.EDGE_WEIGHTS]) {
                        PointD dist = edgeWeightCoords(pos[v], pos[n]);
                        if (updateEdgeWeightPosition(v, n, dist))
                            drawText(
                                cr,
                                dist.X,
                                dist.Y,
                                0.0,
                                graph.getEdgeWeight(v, n).ToString()
                            );
                    }
                }
            }
        }

        void drawEdge(Cairo.Context cr,
                      PointD x,
                      PointD y,
                      double red,
                      double green,
                      double blue) {
            cr.SetSourceRGB(red, green, blue);
            cr.MoveTo(x);
            cr.LineTo(y);
            cr.Stroke();
        }

        (double Red, double Green, double Blue) edgeColor(
                                                int v,
                                                int n,
                                                IEnumerable<(int, int)> visited,
                                                IEnumerable<(int, int)> frontier) {
            if (!algorithmRunning) {
                if (edgeOfFoundResult(v, n))
                    return (Red: 1.0, Green: 0.0, Blue: 0.0);

                return (Red: 0.0, Green: 0.0, Blue: 0.0);
            }

            if (visited != null
                && (visited.Contains((v, n)) || visited.Contains((n, v))))
                return (Red: 0.0, Green: 0.1, Blue: 0.9);
            if (frontier != null
                && (frontier.Contains((v, n)) || frontier.Contains((n, v))))
                return (Red: 0.4, Green: 0.8, Blue: 1.0);

            return (Red: 0.0, Green: 0.0, Blue: 0.0);
        }

        void drawVertices(Cairo.Context cr) {
            int radius = 12;
            IEnumerable<int> visited = null, frontier = null;

            if (algorithmRunning) {
                visited = algorithm.visitedVertices();
                frontier = algorithm.frontierVertices();
            }

            foreach (var vertex in pos) {
                double color = vertexColor(vertex.Key, visited, frontier);

                drawCircle(cr, vertex.Value.X, vertex.Value.Y, 0.0, radius + 2);
                if (color != 0.0)
                    drawCircle(cr, vertex.Value.X, vertex.Value.Y, color, radius);

                color = color < 1.0 ? 1.0 : 0.0;
                drawText(
                    cr,
                    vertex.Value.X - 5.0,
                    vertex.Value.Y + 5.0,
                    color,
                    vertex.Key.ToString()
                );

                if (settings[Strings.WEIGHTED]) {
                    drawText(
                        cr,
                        vertex.Value.X - 5.0,
                        vertex.Value.Y + 30.0,
                        0.0,
                        graph.getWeight(vertex.Key).ToString()
                    );
                }
            }
        }

        void drawCircle(Cairo.Context cr,
                        double x,
                        double y,
                        double color,
                        int radius) {
            cr.SetSourceRGB(color, color, color);
            cr.Arc(x, y, radius, 0, 2 * Math.PI);
            cr.Fill();
        }

        void drawText(Cairo.Context cr,
                            double x,
                            double y,
                            double color,
                            string text) {
            cr.MoveTo(x, y);
            cr.SetSourceRGB(color, color, color);
            cr.ShowText(text);
        }

        double vertexColor(int key,
                           IEnumerable<int> visited,
                           IEnumerable<int> frontier) {
            double color = 1.0;

            if (!algorithmRunning) {
                if (key == selected) color = 0.0;
                return color;
            }

            if (visited != null && visited.Contains(key)) color = 0.0;
            if (frontier != null && frontier.Contains(key)) color = 0.5;

            return color;
        }
    }
}
