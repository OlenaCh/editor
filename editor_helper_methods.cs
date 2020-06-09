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
        GraphWithInterface graph;
        Dictionary<int, PointD> pos = new Dictionary<int, PointD>();
        Dictionary<string, PointD> distancesPos =
            new Dictionary<string, PointD>();

        string filepath = "";

        /*
         * Graph related
         */
        void buildGraphCoordinates() {
            Random rand = new Random();

            foreach (var v in graph.vertices()) {
                int id = v;
                double x, y;
                do {
                    x = Convert.ToDouble(rand.Next(50, 750));
                    y = Convert.ToDouble(rand.Next(50, 750));
                } while (vertexPointExists(x, y) > 0);
                pos.Add(id, new PointD(x, y));
            }
        }

        void newGraph() {
            if (settings[Strings.DIRECTED]) graph = new DirectedGraph();
            else graph = new UndirectedGraph();
            graph.changed += queueDraw;

            pos = new Dictionary<int, PointD>();
            selected = 0;
            algorithm = null;
            algorithmRunning = false;
        }

        /*
         * Algorithm related
         */
        void runAlgorithm(Algorithm algo, bool prims) {
            algorithm = algo;

            if (settings[Strings.AUTO]) {
                algorithm.search();
                displayResult(algorithm.result(), prims);
            }
            else {
                algorithmRunning = algorithm.running();
                algorithmStatus.Text = Strings.MANUAL_MODE_TIP;
            }
        }

        void displayResult(string path, bool prims) {
            if (path == "unreachable") {
                algorithmStatus.Text = Strings.FAILURE;
                return;
            }

            algorithmStatus.Text = Strings.SUCCESS;
            selected = 0;

            if (prims) buildSpanningTree(path);
            else buildFoundPath(path);

            drawGraph();
        }

        void buildSpanningTree(string data) {
            foreach (string pair in data.Trim().Split(';'))
                if (pair != "") spanningTree.Add(pair);
        }

        void buildFoundPath(string data) {
            foreach (string vertex in data.Trim().Split())
                foundPath.Add(Int32.Parse(vertex));
        }

        bool edgeOfFoundPath(int a, int b) {
            if (foundPath.Count <= 1) return false;
            return foundPath.Contains(a) && foundPath.Contains(b);
        }

        bool edgeOfFoundTree(int a, int b) {
            if (spanningTree.Count < 1) return false;
            string strA = a.ToString();
            string strB = b.ToString();
            return spanningTree.Contains(strA + " " + strB)
                   || spanningTree.Contains(strB + " " + strA);
        }

        /*
         * Point related
         */
        int vertexPointExists(double x, double y) {
            foreach(var vertex in pos) {
                if (pointExists(vertex.Value, x, y)) return vertex.Key;
            }

            return 0;
        }

        string distancePointExists(double x, double y) {
            foreach(var edge in distancesPos) {
                if (pointExists(edge.Value, x, y)) return edge.Key;
            }

            return "";
        }

        bool pointExists(PointD p, double x, double y) {
            double px = p.X, py = p.Y;
            return x >= px - 16.0
                   && x <= px + 16.0
                   && y >= py - 16.0
                   && y <= py + 16.0;
        }

        bool xInEditorRange(double val) { return val >= 20 && val <= 880; }
        bool yInEditorRange(double val) { return val >= 20 && val <= 860; }

        PointD arrowCorner(PointD a, PointD b, double diff, double dir) {
            PointD u = unitVector(a, b);
            return perpendicularPoint(a, u, diff, dir);
        }

        PointD arrowTip(PointD a, PointD b, double diff) {
            PointD u = unitVector(a, b);
            return new PointD(a.X + diff * u.X, a.Y + diff * u.Y);
        }

        PointD distanceCoords(PointD a, PointD b) {
            PointD mid = new PointD((a.X + b.X) / 2, (a.Y + b.Y) / 2);
            PointD u = unitVector(new PointD(mid.X * -1.0, mid.Y * -1), a);
            return perpendicularPoint(mid, u, 20.0, 1.0);;
        }

        PointD perpendicularPoint(PointD a, PointD u, double diff, double dir) {
            PointD n = normalVector(u);
            double c = diff * 2, d = diff / 2;
            double x = a.X + c * u.X + dir * d * n.X,
                   y = a.Y + c * u.Y - dir * d * n.Y;

            return new PointD(x, y);
        }

        /*
         * Distance related
         */
        bool updateDistancePosition(int a, int b, PointD dist) {
            string strA = a + " " + b;
            string strB = b + " " + a;

            if (distancesPos.ContainsKey(strA) || distancesPos.ContainsKey(strB))
                return false;

            distancesPos[strA] = dist;
            return true;
        }

        /*
         * Vector related
         */
        PointD unitVector(PointD a, PointD b) {
            PointD v = vector(a, b);
            double s = Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
            return new PointD(v.X / s, v.Y / s);
        }

        PointD normalVector(PointD u) { return new PointD(u.Y, u.X); }

        PointD vector(PointD a, PointD b) {
            return new PointD(b.X - a.X, b.Y - a.Y);
        }

        /*
         * User input related
         */
        void fileAction(string action) {
            FileChooserDialog dialog = new FileChooserDialog(
                action,
                this,
                (action == "Save") ? FileChooserAction.Save : FileChooserAction.Open,
                Strings.NO_RESPONSE, ResponseType.Cancel,
                action, ResponseType.Accept
            );

            if (dialog.Run() == (int)ResponseType.Accept) {
                if (action == "Save") {
                    filepath = dialog.Filename;
                    graph.toFile(filepath);
                }
                else {
                    newGraph();
                    graph.fromFile(dialog.Filename);
                    FileRead = true;
                    drawGraph();
                }
                Title = dialog.Filename;
            }

            dialog.Destroy();
        }

        string getNewEdgeDistance() {
            return userInputDialog(Strings.DIST_WARNING);
        }

        string getNewVertexWeight() {
            return userInputDialog(Strings.WEIGHT_WARNING);
        }

        void showUserInfo(string message) {
            Dialog dialog = new Dialog(
                Strings.INFO,
                this,
                DialogFlags.DestroyWithParent | DialogFlags.Modal,
                Strings.YES_RESPONSE, ResponseType.Yes
            );

            dialog.VBox.Add(new Label(message));

            dialog.ShowAll();
            dialog.Run();
            dialog.Destroy();
        }

        string userInputDialog(string msg) {
            string result = "";
            Dialog dialog = new Dialog(
                msg,
                this,
                DialogFlags.DestroyWithParent | DialogFlags.Modal,
                Strings.BTN_SAVE, ResponseType.Yes,
                Strings.NO_RESPONSE, ResponseType.No
            );

            Entry entry = new Entry();
            dialog.VBox.Add(entry);
            dialog.ShowAll();

            if (dialog.Run() == (int)ResponseType.Yes) result = entry.Text;
            dialog.Destroy();

            return result;
        }
    }
}
