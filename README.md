The File menu contains these menu items:

New - Delete all vertices and edges, i.e. start over with a new graph.
Open... - Prompt the user for a filename, then read the graph from that file.
Save - Save the graph to a file. If the graph was previously read from a file, it is saved back to that same file.
Quit - Exit the application.

The Algorithms menu contains these menu items:
Shortest path - Run shortest path in graph algorithm and display result. For any type of graph.
Longest path (DAGs only) - Run longest path in graph algorithm and display result. Only for DAGs.
Prim's algorithm - Run Prim's algorithm and display result. Only for undirected graphs with set distances.
Stop/Clear status - Stop algorithm if it is running, clear algorithm status.

The Settings item allows to change graph settings. Graph settings are not stored/read to/from file, therefore,
before opening file with graph details it is necessary to make sure that graph settings are set correctly. 
Most settings can be change without deleting currect graph, however, the change of directed/undirected type of graph
will result in creating a new one.

Use cases:

1) Adding new vertex. The user can create a new vertex by holding down Shift and clicking in the window.
2) Selecting vertex. The user can click any vertex to select it. Newly added vertex is auto selected. 
If the user clicks in the window outside any vertices, then the selection is cleared.
3) Adding/deleting new edge. The user can hold down Control and click any vertex V to create an edge between the selected vertex and V.
Or, if there is already an edge between the selected vertex and V, then this action removes that edge.
4) Dragging vertex. The user can click any vertex and drag it with the mouse to change its position. 
5) Deleting vertex. The user can press the Delete key to delete selected vertex with all edges that are attached to it.
6) Setting vertex weight. If graph is weighted then user can set vertex weight by selecting a vertex and pressing q/Q key.
7) Setting edge distance. If distances are displayed then user can set edge distance by clicking on edge distance number.
This data is NOT stored/read from file.
8) Running algorithms manually. To proceed through algorithm step by step user can set algorithm mode to Manual and 
then press Slash key.
