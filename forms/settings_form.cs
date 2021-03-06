using Gdk;
using Gtk;
using Cairo;
using System;
using System.Collections.Generic;

namespace Forms {
    using GraphEditor;
    using Strings;

    class SettingsForm : Gtk.Window {
        Dictionary<string, bool> oldSettings, newSettings;
        List<RadioButton> radioBtns = new List<RadioButton>();
        CheckButton checkBtn;
        int index = -1;

        public SettingsForm() : base(Strings.S_FORM) {
            oldSettings = Editor.getSettings();
            newSettings = new Dictionary<string, bool>();
            configure();
        }

        void configure() {
            SetDefaultSize(300, 200);

            Add(createForm());
            ShowAll();
        }

        VBox createForm() {
            VBox vbox = new VBox();

            vbox.PackStart(new Label(Strings.G_DETAILS), false, true, 5);

            checkBtn = new CheckButton(Strings.EDGE_WEIGHT_MSG);
            vbox.PackStart(checkBtn, false, true, 5);
            checkBtn.Active = oldSettings[Strings.EDGE_WEIGHTS];

            vbox.PackStart(
                createRadioGroup(new List<string>() {
                    Strings.UNWEIGHTED,
                    Strings.WEIGHTED
                }),
                false,
                true,
                5
            );
            vbox.PackStart(
                createRadioGroup(new List<string>() {
                    Strings.UNDIRECTED,
                    Strings.DIRECTED
                }),
                false,
                true,
                5
            );

            vbox.PackStart(new Label(Strings.A_MODE), false, true, 5);
            vbox.PackStart(
                createRadioGroup(new List<string>() {
                    Strings.AUTO,
                    Strings.MANUAL
                }),
                false,
                true,
                5
            );

            vbox.PackStart(createControlButtons(), false, false, 5);

            return vbox;
        }

        HBox createRadioGroup(List<string> items) {
            HBox hbox = new HBox();
            RadioButton btn = null;

            foreach (var item in items) {
                btn = new RadioButton(btn, item);
                hbox.PackStart(btn, false, true, 5);
                btn.Active = oldSettings[item];
                radioBtns.Add(btn);

                if (Strings.UNDIRECTED == item) index = radioBtns.Count - 1;
            }

            return hbox;
        }

        HBox createControlButtons() {
            HBox hbox = new HBox();

            Button saveBtn = new Button(Strings.BTN_SAVE);
            hbox.PackStart(saveBtn, true, true, 5);
            saveBtn.Clicked += onSaveBtn;

            Button cancelBtn = new Button(Strings.NO_RESPONSE);
            hbox.PackStart(cancelBtn, true, true, 5);
            cancelBtn.Clicked += delegate { Destroy(); };

            return hbox;
        }

        void onSaveBtn(object sender, EventArgs args) {
            if (index >= 0
                && radioBtns[index].Active != oldSettings[Strings.UNDIRECTED])
                displayUserWarning();
            else updateSettings();
        }

        void displayUserWarning() {
            Dialog dialog = new Dialog(
                Strings.INFO,
                this,
                DialogFlags.DestroyWithParent | DialogFlags.Modal,
                Strings.YES_RESPONSE, ResponseType.Yes,
                Strings.NO_RESPONSE, ResponseType.No
            );

            dialog.VBox.Add(new Label(Strings.NEW_GRAPH_TIP));
            dialog.ShowAll();

            if (dialog.Run() == (int)ResponseType.Yes) updateSettings();

            dialog.Destroy();
        }

        void updateSettings() {
            foreach (var btn in radioBtns)
                newSettings[btn.Label.ToString()] = btn.Active;
            newSettings[Strings.EDGE_WEIGHTS] = checkBtn.Active;
            Editor.setSettings(newSettings);
            Destroy();
        }
    }
}