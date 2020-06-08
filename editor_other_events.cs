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
        void onSaveClicked(object sender, EventArgs args) { fileAction("Save"); }
        void onQuitClicked(object sender, EventArgs args) { Application.Quit(); }

        void onSettignsClicked(object sender, EventArgs args) {
            Gtk.Window form = new SettingsForm();
            form.Destroyed += onSettingsFormDestroyed;
        }

        void onShortestPathClicked(object sender, EventArgs args) {
            Gtk.Window form = new AlgorithmForm(graph);
            form.Destroyed += onAlgorithFormDestroyed;
        }

        void onLongestPathClicked(object sender, EventArgs args) {
            if (settings[Strings.UNWEIGHTED] || settings[Strings.UNDIRECTED]) {
                showUserInfo(Strings.DAGS_ONLY_TIP);
                return;
            }

            runAlgorithm(new LongestPathInDAG(graph), false);
        }

        void onPrimsAlgoritmClicked(object sender, EventArgs args) {
            if (!settings[Strings.DISTANCES] || settings[Strings.DIRECTED]) {
                showUserInfo(Strings.PRIMS_TIP);
                return;
            }

            runAlgorithm(new PrimsAlgorithm(graph), true);
        }

        void onStopClicked(object sender, EventArgs args) {
            algorithm = null;
            algorithmRunning = false;
            algorithmStatus.Text = "";
            if (settings[Strings.MANUAL]) drawGraph();
        }

        // Other windows events
        void onAlgorithFormDestroyed(object sender, EventArgs args) {
            runAlgorithm(new ShortestPathInGraph(graph, start, end), false);
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
