using Gdk;
using Gtk;
using Cairo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphEditor {
    using Algorithms;
    using Forms;
    using Strings;

    partial class Editor : Gtk.Window {

        // Menu Bar Events
        void onNewClicked(object sender, EventArgs args) { newGraph(); drawGraph(); }
        void onOpenClicked(object sender, EventArgs args) { fileAction("Open"); }
        void onQuitClicked(object sender, EventArgs args) { Application.Quit(); }

        void onSaveClicked(object sender, EventArgs args) {
            if (filepath != "") {
                graph.toFile(filepath);
                return;
            }

            fileAction("Save");
        }

        void onSettignsClicked(object sender, EventArgs args) {
            if (!algorithmRunning) {
                Gtk.Window form = new SettingsForm();
                form.Destroyed += onSettingsFormDestroyed;
            }
            else showUserInfo(Strings.NO_RUNNING_ALGO_TIP);
        }

        void onShortestPathClicked(object sender, EventArgs args) {
            if (settings[Strings.WEIGHTED]) {
                showUserInfo(Strings.SHORTEST_PATH_TIP);
                return;
            }

            Gtk.Window form = new AlgorithmForm(graph);
            form.Destroyed += onAlgorithFormDestroyed;
        }

        void onLongestPathClicked(object sender, EventArgs args) {
            if (settings[Strings.UNWEIGHTED] || settings[Strings.UNDIRECTED]) {
                showUserInfo(Strings.DAGS_ONLY_TIP);
                return;
            }

            runAlgorithm(new LongestPathInDAG(graph));
        }

        void onPrimsAlgoritmClicked(object sender, EventArgs args) {
            if (!settings[Strings.DISTANCES] || settings[Strings.DIRECTED]) {
                showUserInfo(Strings.PRIMS_TIP);
                return;
            }

            runAlgorithm(new PrimsAlgorithm(graph));
        }

        void onStopClicked(object sender, EventArgs args) {
            algorithm = null;
            algorithmRunning = false;
            algorithmStatus.Text = "";
            foundResult = new List<(int, int)>();
            if (settings[Strings.MANUAL]) drawGraph();
        }

        // Other windows events
        void onAlgorithFormDestroyed(object sender, EventArgs args) {
            runAlgorithm(new ShortestPathInGraph(graph, start, end));
        }

        void onSettingsFormDestroyed(object sender, EventArgs args) {
            if (directionsUpdate) {
                newGraph();
                directionsUpdate = false;
            }

            drawGraph();
        }
    }
}
