using System;

namespace Strings {
    public static class Strings {
        public const string UNWEIGHTED = "Unweighted";
        public const string WEIGHTED = "Weighted";
        public const string UNDIRECTED = "Undirected";
        public const string DIRECTED = "Directed";
        public const string AUTO = "Auto";
        public const string MANUAL = "Manual";
        public const string DISTANCES = "Distances";

        public const string NO_RESPONSE = "Cancel";
        public const string YES_RESPONSE = "Ok";

        public const string BTN_SAVE = "Save";

        public const string INFO = "Information";
        public const string DIST_WARNING = "Distance (integer)";
        public const string WEIGHT_WARNING = "Weight (integer)";

        public const string A_FORM = "Algorithm form";
        public const string S_FORM = "Settings";

        public const string G_DETAILS = "Graph details:";
        public const string A_MODE = "Algorithm mode:";

        public const string DISTANCES_MSG = "Display distances";
        public const string DAGS_ONLY_TIP = "This algorithm can be applied only to weighted DAGs.";
        public const string MANUAL_MODE_TIP = "Algorithm running... Press 'Slash' key.";
        public const string NEW_GRAPH_TIP = "Changes of setting 'Directed/Undirected' creates a new graph. Your current graph may be lost.";
        public const string PRIMS_TIP = "This algorithm can be applied only to undirected graphs with distances.";

        public const string SUCCESS = "Algorithm finished running. See result...";
        public const string FAILURE = "Algorithm finished running. No result found...";
    }
}