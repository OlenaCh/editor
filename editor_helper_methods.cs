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
        Dictionary<string, PointD> edgeWeightsPos =
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
            clearAlgorithmRelatedData();
        }

        /*
         * Algorithm related
         */
        void runAlgorithm(Algorithm algorithm) {
            this.algorithm = algorithm;

            if (settings[Strings.AUTO]) {
                algorithm.search();
                displayResult(algorithm.result());
            }
            else {
                algorithmRunning = algorithm.running();
                algorithmStatus.Text = Strings.MANUAL_MODE_TIP;
            }
        }

        void displayResult(Result result) {
            if (!result.buildResult()) {
                algorithmStatus.Text = Strings.FAILURE;
                return;
            }

            algorithmStatus.Text = Strings.SUCCESS;
            selected = 0;
            foundResult = result.result();

            drawGraph();
        }

        bool edgeOfFoundResult(int a, int b) {
            if (foundResult.Count < 1) return false;
            return foundResult.Contains((a, b)) || foundResult.Contains((b, a));
        }

        void clearAlgorithmRelatedData() {
            algorithm = null;
            algorithmRunning = false;
            algorithmStatus.Text = "";
            foundResult = new List<(int, int)>();
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

        string edgeWeightPointExists(double x, double y) {
            foreach(var edge in edgeWeightsPos) {
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
            return (new Vector(a, b, diff, dir)).arrowCornerPoint();
        }

        PointD arrowTip(PointD a, PointD b, double diff) {
            return (new Vector(a, b, diff)).arrowTipPoint();
        }

        PointD edgeWeightCoords(PointD a, PointD b) {
            return (new Vector(a, b, 15.0)).edgeWeightPoint();
        }

        /*
         * Edge weight related
         */
        bool updateEdgeWeightPosition(int a, int b, PointD dist) {
            string strA = a + " " + b;
            string strB = b + " " + a;

            if (edgeWeightsPos.ContainsKey(strA)
                || edgeWeightsPos.ContainsKey(strB))
                return false;

            edgeWeightsPos[strA] = dist;
            return true;
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
                    filepath = dialog.Filename;
                    graph.fromFile(dialog.Filename);
                    FileRead = true;
                    drawGraph();
                }
                Title = dialog.Filename;
            }

            dialog.Destroy();
        }

        string getNewEdgeWeight() {
            return userInputDialog(Strings.EDGE_WARNING);
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
