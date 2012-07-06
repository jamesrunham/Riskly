var Log = {
    elem: false,
    write: function (text) {
        if (!this.elem)
            this.elem = document.getElementById('log');
        this.elem.innerHTML = text;
        this.elem.style.left = (500 - this.elem.offsetWidth / 2) + 'px';
    }
};

function initHT(container, dates, iterationType, path, depth) {
    $.getJSON('/heatmap', { pathContains: path, iteration: iterationType, depth: depth }, function (response) {
        var json = response;
        console.log(response);
        //init Spacetree
        //Create a new ST instance
        var st = new $jit.ST({
            //id of viz container element
            injectInto: container,
            //set duration for the animation
            duration: 800,
            //set animation transition type
            transition: $jit.Trans.Back.easeInOut,
            //set distance between node and its children
            levelDistance: 50,
            constrained: false,
            levelsToShow: 20,
            offsetX: 1101,
            offsetY: -600,

            //set node and edge styles
            //set overridable=true for styling individual
            //nodes or edges
            Node: {
                width: 200,
                height: 50,
                //dim: 500,
                type: 'circle',
                color: 'blue',
                overridable: true,
                border: 'black',
                lineWidth: 10,
                align: 'left'
            },

            Edge: {
                type: 'bezier',
                overridable: true,
                lineWidth: 3,
                epsilon: 10
            },

           
            Tree : {
                orientation: "right",
                subtreeOffset: 100,
                siblingOffset: 200,
                indent: 10,
                multitree: false,
                align: "center"
            },

            //This method is called on DOM label creation.
            //Use this method to add event handlers and styles to
            //your node.
            onCreateLabel: function (label, node) {
                label.id = node.id;
                label.innerHTML = '<span class="nodeName">' + node.name + "<br>" + (node.data.checkins > 0 ? node.data.checkins : "") + '</span>';

                var style = label.style;
                style.width = 60 + 'px';
                style.height = 17 + 'px';
                style.cursor = 'pointer';
                style.color = '#333';
                style.fontSize = '0.8em';
                style.textAlign = 'center';
                style.padding = '3px';
            },

            //This method is called right before plotting
            //a node. It's useful for changing an individual node
            //style properties before plotting it.
            //The data properties prefixed with a dollar
            //sign will override the global node style properties.
            onBeforePlotNode: function (node) {

                if (node.data.checkins == "") {
                    node.data.$colour = "rgba(0,0,255,0.3)";
                }

                node.data.$color = "#" + node.data.colourHex;
                node.data.$dim = node.data.percentage;
                console.log(node.data.percentage);

                if (node.data.isFolder = true) {
                    node.data.$border = 200;
                }
            },

            //This method is called right before plotting
            //an edge. It's useful for changing an individual edge
            //style properties before plotting it.
            //Edge data proprties prefixed with a dollar sign will
            //override the Edge global style properties.
            onBeforePlotLine: function (adj) {
                if (adj.nodeFrom.selected && adj.nodeTo.selected) {
                    adj.data.$color = "#eed";
                    adj.data.$lineWidth = 3;
                }
            },

            switchPosition: {
                pos: "top"
            },

            //enable panning
            Navigation: {
                enable: true,
                panning: true
            },
            
            onAfterCompute :  function (){
                Log.write("Code changes between " + dates.dateFrom + ' - '  + dates.dateTo);  
            }

        });
        //load json data   
        st.loadJSON(json);
        //compute node positions and layout
        st.compute();
        //optional: make a translation of the tree
        st.geom.translate(new $jit.Complex(-200, 0), "current");
        //emulate a click on the root node.
        st.onClick(st.root);
        //end
    });
}

function Clear(domElements) {
    var thisObj = this;
    thisObj.domElements = domElements;
    for (var i = 0; i < thisObj.domElements.length; i++) {
        this.elem = document.getElementById(thisObj.domElements[i]);
        this.elem.innerHTML = "";
    }
}


var initTree = function (container, dates, iterationType, path, depth) {
    $.getJSON('/heatmap', { pathContains: path, iteration: iterationType, depth: depth }, function(response) {
        var json = response;

        var ht = new $jit.Hypertree({
        //id of the visualization container  
            injectInto: 'infovis',
            //canvas width and height  
            width: 3000,
            height: 4000,
            //Change node and edge styles such as  
            //color, width and dimensions.  
            Node: {
                dim: 9,
                color: "#f00"
            },
            Edge: {
                lineWidth: 2,
                color: "#088"
            },
            onBeforeCompute: function(node) {
                Log.write("centering");
            },
            //Attach event handlers and add text to the  
            //labels. This method is only triggered on label  
            //creation  
            onCreateLabel: function(domElement, node) {
                domElement.innerHTML = node.name;
                $jit.util.addEvent(domElement, 'click', function() {
                    ht.onClick(node.id, {
                        onComplete: function() {
                            ht.controller.onComplete();
                        }
                    });
                });
            },
            //Change node styles when labels are placed  
            //or moved.  
            onPlaceLabel: function(domElement, node) {
                var style = domElement.style;
                style.display = '';
                style.cursor = 'pointer';
                if (node._depth <= 1) {
                    style.fontSize = "0.8em";
                    style.color = "#ddd";

                } else if (node._depth == 2) {
                    style.fontSize = "0.7em";
                    style.color = "#555";

                } else {
                    style.display = 'none';
                }

                var left = parseInt(style.left);
                var w = domElement.offsetWidth;
                style.left = (left - w / 2) + 'px';
            },

            onComplete: function() {
                Log.write("done");

                //Build the right column relations list.  
                //This is done by collecting the information (stored in the data property)   
                //for all the nodes adjacent to the centered node.  
                var node = ht.graph.getClosestNodeToOrigin("current");
                var html = "<h4>" + node.name + "</h4><b>Connections:</b>";
                html += "<ul>";
                node.eachAdjacency(function(adj) {
                    var child = adj.nodeTo;
                    if (child.data) {
                        var rel = (child.data.band == node.name) ? child.data.relation : node.data.relation;
                        html += "<li>" + child.name + " " + "<div class=\"relation\">(relation: " + rel + ")</div></li>";
                    }
                });
                html += "</ul>";
                $jit.id('inner-details').innerHTML = html;
            }
        });
        //load JSON data.  
        ht.loadJSON(json);
        //compute positions and plot.  
        ht.refresh();
    });
};
