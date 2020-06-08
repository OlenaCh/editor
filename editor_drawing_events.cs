using Gdk;
using Gtk;
using Cairo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphEditor {
    using Graphs;
    using Strings;

    partial class Editor : Gtk.Window {
        bool ShiftPressed = false;
        bool CtrlPressed = false;
        bool Dragging = false;
        bool FileRead = false;

        // Drawing Area events
        void queueDraw(Graph.Events evnt, int id) {
            switch(evnt) {
                case Graph.Events.VERTEX_ADDED:
                    selected = id;
                    pos.Add(id, clicked);
                    break;
                case Graph.Events.VERTEX_DELETED:
                    selected = 0;
                    pos.Remove(id);
                    break;
                case Graph.Events.EDGE_ADDED:
                case Graph.Events.EDGE_DELETED:
                case Graph.Events.WEIGHT_CHANGED:
                case Graph.Events.DISTANCE_CHANGED:
                default:
                    break;
            }

            drawGraph();
        }

        void onKeyPressed(object sender, KeyPressEventArgs args) {
            if (!algorithmRunning) {
                if (args.Event.Key.ToString().Contains("Shift")) {
                    ShiftPressed = true;
                    return;
                }

                if (args.Event.Key.ToString().Contains("Control")) {
                    CtrlPressed = true;
                    return;
                }

                if (settings[Strings.WEIGHTED]
                    && args.Event.Key.ToString().ToUpper() == "Q"
                    && selected > 0) {
                    var isNumeric = int.TryParse(getNewVertexWeight(), out int n);
                    if (isNumeric) graph.setWeight(selected, n);
                    return;
                }

                if (args.Event.Key.ToString() == "Delete" && selected > 0)
                    graph.deleteVertex(selected);
            }
        }

        void onKeyReleased(object sender, KeyReleaseEventArgs args) {
            if (!algorithmRunning) {
                if (args.Event.Key.ToString().Contains("Shift"))
                    ShiftPressed = false;

                if (args.Event.Key.ToString().Contains("Control"))
                    CtrlPressed = false;
            }

            if (settings[Strings.MANUAL]
                && args.Event.Key.ToString() == "slash") {
                if (algorithmRunning) {
                    algorithm.executeSearchStep();
                    drawGraph();
                    algorithmRunning = algorithm.running();
                }
                else {
                    displayResult(
                        algorithm.result(),
                        algorithm.GetType().Name == "PrimsAlgorithm"
                    );
                }
            }
        }

        void onButtonPressed(object sender, ButtonPressEventArgs args) {
            if (!algorithmRunning) {
                int ind = vertexPointExists(args.Event.X, args.Event.Y);
                string edge = "";

                if (settings[Strings.DISTANCES])
                    edge = distancePointExists(args.Event.X, args.Event.Y);

                if (ShiftPressed && ind >= 0) {
                    clicked = new PointD(args.Event.X, args.Event.Y);
                    graph.addVertex();
                    return;
                }

                if (CtrlPressed && selected > 0 && selected != ind && ind > 0) {
                    IEnumerable<int> neighbors = graph.neighbors(selected);
                    if (neighbors.Contains(ind)) graph.disconnect(selected, ind);
                    else graph.connect(selected, ind);
                    return;
                }

                if (settings[Strings.DISTANCES] && edge != "") {
                    string[] vertices = edge.Split();
                    var isNumeric =
                        int.TryParse(getNewEdgeDistance(), out int n);

                    if (isNumeric)
                        graph.setDistance(
                            Int32.Parse(vertices[0]),
                            Int32.Parse(vertices[1]),
                            n
                        );

                    return;
                }

                if (!Dragging && ind > 0) { Dragging = true; }

                selected = ind;
                drawGraph();
            }
        }

        void onButtonReleased(object sender, ButtonReleaseEventArgs args) {
            if (!algorithmRunning) Dragging = false;
        }

        void onPointerMoved(object sender, MotionNotifyEventArgs args) {
            if (Dragging
                && xInEditorRange(args.Event.X)
                && yInEditorRange(args.Event.Y)) {
                pos[selected] = new PointD(args.Event.X, args.Event.Y);
                drawGraph();
            }
        }
    }
}
