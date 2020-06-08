using Gdk;
using Gtk;
using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forms {
    using GraphEditor;
    using Graphs;
    using Strings;

    class AlgorithmForm : Gtk.Window {
        Graph graph;
        string[] vertices;
        List<ComboBox> cbs = new List<ComboBox>();

        public AlgorithmForm(Graph initialGraph) : base(Strings.A_FORM) {
            graph = initialGraph;
            buildArrayOfVerices();
            configure();
        }

        void buildArrayOfVerices() {
            vertices = new string[graph.vertices().Count()];
            int count = 0;

            foreach(var vertex in graph.vertices()) {
                vertices[count] = vertex.ToString();
                count++;
            }
        }

        void configure() {
            SetDefaultSize(200, 170);
            Add(createForm());
            ShowAll();
        }

        VBox createForm() {
            VBox vbox = new VBox();

            foreach(string str in new List<string>() { "Start", "End" })
                addComboBox(vbox, str);

            Button saveBtn = new Button(Strings.BTN_SAVE);
            vbox.PackStart(saveBtn, false, false, 5);
            saveBtn.Clicked += onSaveBtn;

            return vbox;
        }

        void addComboBox(VBox vbox, string label) {
            vbox.PackStart(new Label(label + ":"), false, false, 5);

            ComboBox cb = new ComboBox(vertices);
            vbox.PackStart(cb, false, false, 5);
            cbs.Add(cb);
        }

        void onSaveBtn(object sender, EventArgs args) {
            Editor.setKeyVerticesForShortestPath(getKeyValues());
            Destroy();
        }

        int[] getKeyValues() {
            int[] vals = new int[2];
            int count = 0;

            foreach(var cb in cbs) {
                if (cb.ActiveText != null)
                    vals[count] = Int32.Parse(cb.ActiveText);
                count++;
            }

            return vals;
        }
    }
}