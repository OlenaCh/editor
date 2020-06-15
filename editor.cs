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
    using Graphs;
    using Strings;

    partial class Editor : Gtk.Window {
        public static Dictionary<string, bool> settings =
            new Dictionary<string, bool>() {
                { Strings.UNWEIGHTED, true },
                { Strings.WEIGHTED, false },
                { Strings.UNDIRECTED, true },
                { Strings.DIRECTED, false },
                { Strings.MANUAL, false },
                { Strings.AUTO, true },
                { Strings.EDGE_WEIGHTS, false }
            };
        public static int start, end;
        public static bool directionsUpdate = false;

        DrawingArea area;
        Label algorithmStatus;

        int selected;
        PointD clicked;

        Algorithm algorithm;
        bool algorithmRunning = false;
        List<(int, int)> foundResult;

        public Editor(string name) : base(name) { configure(); }

        public static Dictionary<string, bool> getSettings() { return settings; }
        public static void setSettings(Dictionary<string, bool> newSettings) {
            if (settings[Strings.UNDIRECTED] != newSettings[Strings.UNDIRECTED])
                directionsUpdate = true;
            settings = newSettings;
        }

        public static void setKeyVerticesForShortestPath(int[] vals) {
            start = vals[0];
            end = vals[1];
        }

        void configure() {
            SetDefaultSize(900, 900);

            DeleteEvent += delegate { Application.Quit(); };
            KeyPressEvent += onKeyPressed;
            KeyReleaseEvent += onKeyReleased;
            ExposeEvent += onExpose;

            MenuBar mb = createMenuBar();
            algorithmStatus = new Label();
            createDrawingArea();

            VBox vbox = new VBox(false, 0);
            vbox.PackStart(mb, false, false, 0);
            vbox.PackStart(algorithmStatus, false, false, 0);
            vbox.PackStart(area, true, true, 0);
            Add(vbox);

            ShowAll();

            newGraph();
        }

        void createDrawingArea() {
            area = new DrawingArea();
            area.AddEvents((int) (EventMask.ButtonPressMask
                                  |EventMask.ButtonReleaseMask
                                  |EventMask.PointerMotionMask));

            area.ButtonPressEvent += onButtonPressed;
            area.ButtonReleaseEvent += onButtonReleased;
            area.MotionNotifyEvent += onPointerMoved;
        }

        MenuBar createMenuBar() {
            MenuBar mb = new MenuBar();
            string[] menuitems = { "File", "Algorithms", "Settings" };
            Dictionary<string, EventHandler>[] items = {
                new Dictionary<string, EventHandler>() {
                    { "New", onNewClicked },
                    { "Open", onOpenClicked },
                    { "Save", onSaveClicked },
                    { "Quit", onQuitClicked }
                },
                new Dictionary<string, EventHandler>() {
                    { "Shortest path", onShortestPathClicked },
                    { "Longest path (DAGs only)", onLongestPathClicked },
                    { "Prim's algorithm", onPrimsAlgoritmClicked },
                    { "Stop/Clear status", onStopClicked }
                }
            };

            for (int i = 0; i < 3; i++) {
                Menu menu = new Menu();
                MenuItem mi = new MenuItem(menuitems[i]);
                mi.Submenu = menu;

                if (i < 2) {
                    foreach (var item in items[i]) {
                        MenuItem tmp = new MenuItem(item.Key);
                        tmp.Activated += item.Value;
                        menu.Append(tmp);
                    }
                }
                else mi.Activated += onSettignsClicked;

                mb.Append(mi);
            }

            return mb;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            new Editor("(untitled)");
            Application.Run();
        }
    }
}
