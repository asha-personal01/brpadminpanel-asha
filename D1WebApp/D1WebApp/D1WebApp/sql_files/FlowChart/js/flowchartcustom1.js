function init() {    
    if (window.goSamples) goSamples();  // init for these samples -- you don't need to call this
    var $ = go.GraphObject.make;  // for conciseness in defining templates
    myDiagram =
      $(go.Diagram, "myDiagramDiv",  // must name or refer to the DIV HTML element
        {
          initialContentAlignment: go.Spot.Center,
		  initialViewportSpot: go.Spot.TopCenter,		  
          allowDrop: true,  // must be true to accept drops from the Palette
          "LinkDrawn": showLinkLabel,  // this DiagramEvent listener is defined below
          "LinkRelinked": showLinkLabel,
          "animationManager.duration": 800, // slightly longer than default (600ms) animation
		    "undoManager.isEnabled": true,  // enable undo & redo
			 "ModelChanged": function(e) {
            if (e.isTransactionFinished) {
              document.getElementById("mySavedModel").textContent = myDiagram.model.toJson();
            }
			}
        });
		myDiagram.addDiagramListener("ObjectDoubleClicked",
      function(e) {
        var part = e.subject.part;
		 alert("Clicked on " + part.data.Comments);
      });
    // when the document is modified, add a "*" to the title and enable the "Save" button
    myDiagram.addDiagramListener("Modified", function(e) {
      var button = document.getElementById("SaveButton");
      if (button) button.disabled = !myDiagram.isModified;
       var idx = document.title.indexOf("*"); 
      if (myDiagram.isModified) {
        if (idx < 0) document.title += "*";
      } else {
        if (idx >= 0) document.title = document.title.substr(0, idx); 
      }
    });
	myDiagram.toolManager.mouseMoveTools.insertAt(2, new DragZoomingTool());
    // helper definitions for node templates
    function nodeStyle() {
      return [
        // The Node.location comes from the "loc" property of the node data,
        // converted by the Point.parse static method.
        // If the Node.location is changed, it updates the "loc" property of the node data,
        // converting back using the Point.stringify static method.
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        {
          // the Node.location is at the center of each node
          locationSpot: go.Spot.Center,
          //isShadowed: true,
          //shadowColor: "#888",
          // handle mouse enter/leave events to show/hide the ports
          mouseEnter: function (e, obj) { showPorts(obj.part, true); },
          mouseLeave: function (e, obj) { showPorts(obj.part, false); }
        }
      ];
    }
	  
    function makePort(name, spot, output, input) {
      // the port is basically just a small circle that has a white stroke when it is made visible
      return $(go.Shape, "Circle",
               {
                  fill: "transparent",
                  stroke: null,  // this is changed to "white" in the showPorts function
                  desiredSize: new go.Size(8, 8),
                  alignment: spot, alignmentFocus: spot,  // align the port on the main Shape
                  portId: name,  // declare this object to be a "port"
                  fromSpot: spot, toSpot: spot,  // declare where links may connect at this port
                  fromLinkable: output, toLinkable: input,  // declare whether the user may draw links to/from here
                  cursor: "pointer"  // show a different cursor to indicate potential link point
               });
    }
    // define the Node templates for regular nodes
    var lightText = 'whitesmoke';
    myDiagram.nodeTemplateMap.add("",  // the default category
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center},nodeStyle(),
        // the main object is a Panel that surrounds a TextBlock with a rectangular Shape
        $(go.Panel, "Auto",
          $(go.Shape, "Rectangle",
            { fill: "#00A9C9", stroke: null },
            new go.Binding("figure", "figure")),
          $(go.TextBlock,
            {
              font: "bold 11pt Helvetica, Arial, sans-serif",
              stroke: lightText,
              margin: 8,
              maxSize: new go.Size(NaN, NaN),
              wrap: go.TextBlock.WrapFit
              
            },
            new go.Binding("text")),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" }),
            $(go.TextBlock, { margin: 4},
              new go.Binding("text"))
          )  // end of Adornment
      }
        )
     ));
     
	 
	  
	  myDiagram.nodeTemplateMap.add("AP",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-AP.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "AP" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  
	   myDiagram.nodeTemplateMap.add("AR",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-AR.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "AR" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	   myDiagram.nodeTemplateMap.add("audit",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-audit.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "audit" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  
	  myDiagram.nodeTemplateMap.add("billing",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-billing.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "billing" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  
	   myDiagram.nodeTemplateMap.add("cashdisp",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-cashdisp.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "cashdisp" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  
	   myDiagram.nodeTemplateMap.add("cashreceipt",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-cashreceipt.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "cashreceipt" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  
	   myDiagram.nodeTemplateMap.add("checkprint",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-checkprint.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "checkprint" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	   myDiagram.nodeTemplateMap.add("commission",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-commission.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "commission" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("costing",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-costing.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "costing" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("credit",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-credit.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "credit" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("creditentry",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-creditentry.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "creditentry" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("creditinquiry",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-creditinquiry.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "creditinquiry" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("customers",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-customers.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "customers" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("dbquery",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-dbquery.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "dbquery" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("ecomwebsite",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-ecomwebsite.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "ecomwebsite" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("edi",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-edi.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "edi" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("export",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-export.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "export" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("formprint",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-formprint.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "formprint" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("GL",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-GL.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "GL" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("help",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-help.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "help" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("hidden",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-hidden.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "hidden" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("holdrelease",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-holdrelease.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "holdrelease" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("import",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-import.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "import" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("inquiry",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-inquiry.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "inquiry" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("invinquiry",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-invinquiry.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "invinquiry" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("invM",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-invM.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "invM" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("kitting",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-kitting.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "kitting" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("labels",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-labels.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "labels" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("Minfo",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-Minfo.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "Minfo" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("modify",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-modify.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "modify" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("notes",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-notes.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "notes" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("oe",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-oe.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "oe" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("poe",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-poe.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "poe" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("POentry",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-POentry.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "POentry" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("poinq",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-poinq.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "poinq" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("POS",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-POS.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "POS" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("post",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-post.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "post" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("pricing",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-pricing.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "pricing" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("pricingavail",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-pricingavail.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "pricingavail" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("quote",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-quote.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "quote" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("receiving",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-receiving.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "receiving" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("rentalentry",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-rentalentry.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "rentalentry" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("rentalinq",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-rentalinq.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "rentalinq" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("reporting",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-reporting.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "reporting" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("setup",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-setup.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "setup" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("shipping",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-shipping.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "shipping" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("systemsetup",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-systemsetup.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "systemsetup" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("utility",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-utility.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "utility" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	  myDiagram.nodeTemplateMap.add("vendors",
      $(go.Node, "Spot",{ locationSpot: go.Spot.Center}, nodeStyle(),
          $(go.Picture, { source: "./images/Flowchart-vendors.bmp" ,imageStretch: go.GraphObject.Fill,width: 128, height: 128  }),
      $(go.TextBlock,  "vendors" ,{ alignment: go.Spot.Top, font: "bold 11pt Helvetica, Arial, sans-serif", stroke: lightText,editable: false,wrap: go.TextBlock.WrapFit,margin: 10 }),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" } ),
            $(go.TextBlock, { margin: 2})
          )  // end of Adornment
      }));
	
	  
	  
	  myDiagram.toolManager.mouseDownTools.insertAt(3, new GeometryReshapingTool());
	  myDiagram.nodeTemplateMap.add("FreehandDrawing",
      $(go.Part,
        { locationSpot: go.Spot.Center, isLayoutPositioned: false },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        {
          selectionAdorned: true, selectionObjectName: "SHAPE",
          selectionAdornmentTemplate:  // custom selection adornment: a blue rectangle
            $(go.Adornment, "Auto",
              $(go.Shape, { stroke: "dodgerblue", fill: null }),
              $(go.Placeholder, { margin: -1 }))
        },
        { resizable: false, resizeObjectName: "SHAPE" },
        { rotatable: false, rotateObjectName: "SHAPE" },
        { reshapable: false },  // GeometryReshapingTool assumes nonexistent Part.reshapeObjectName would be "SHAPE"
        $(go.Shape,
          { name: "SHAPE", fill: null, strokeWidth: 1.5 },
          new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
          new go.Binding("angle"),
          new go.Binding("geometryString", "geo"),
          new go.Binding("fill"),
          new go.Binding("stroke"),
          new go.Binding("strokeWidth"))
      ));
  var tool = new FreehandDrawingTool();
    // provide the default JavaScript object for a new polygon in the model
    tool.archetypePartData =
      { stroke: "green", strokeWidth: 3, category: "FreehandDrawing" };
    // allow the tool to start on top of an existing Part
    tool.isBackgroundOnly = false;
    // install as first mouse-move-tool
    myDiagram.toolManager.mouseMoveTools.insertAt(0, tool);
	  
    myDiagram.nodeTemplateMap.add("Comment",
      $(go.Node, "Auto",{ locationSpot: go.Spot.Center },{ resizable: true }, nodeStyle(),
        $(go.Shape, "File",
          { fill: "#EFFAB4", stroke: null }),
        $(go.TextBlock,
          {
            margin: 5,
            maxSize: new go.Size(NaN, NaN),
            wrap: go.TextBlock.WrapFit,
            textAlign: "center",
            
            font: "bold 12pt Helvetica, Arial, sans-serif",
            stroke: '#454545'
          },
          new go.Binding("text")),{
        toolTip:  // define a tooltip for each node that displays the color as text
          $(go.Adornment, "Auto",
            $(go.Shape, { fill: "#84ACB3" }),
            $(go.TextBlock, { margin: 4},
              new go.Binding("text"))
          )  // end of Adornment
      }
        // no ports, because no links are allowed to connect with a comment
      ));
	
	

	
    // replace the default Link template in the linkTemplateMap
    myDiagram.linkTemplate =
      $(go.Link, // the whole link panel
        {
          routing: go.Link.AvoidsNodes,
          curve: go.Link.JumpOver,
          corner: 15, toShortLength: 4,
          relinkableFrom: true,
          relinkableTo: false,
          reshapable: false,
          resegmentable: true, 
		  
          // mouse-overs subtly highlight links:
          mouseEnter: function(e, link) { link.findObject("HIGHLIGHT").stroke = "rgba(30,144,255,0.2)"; },
          mouseLeave: function(e, link) { link.findObject("HIGHLIGHT").stroke = "transparent"; }
        },
        new go.Binding("text"),
        $(go.Shape,  // the highlight shape, normally transparent
          { isPanelMain: true, strokeWidth: 8, stroke: "transparent", name: "HIGHLIGHT" }),
        $(go.Shape,  // the link path shape
          { isPanelMain: true, stroke: "white", strokeWidth: 5 }),
        $(go.Shape,  // the arrowhead
          { toArrow: "standard", stroke: null, fill: "white"}),
        $(go.Panel, "Auto",  // the link label, normally not visible
          { visible: false, name: "LABEL", segmentIndex: 2, segmentFraction: 0.5},
          new go.Binding("text"),
          $(go.Shape, "RoundedRectangle",  // the label shape
            { fill: "#F8F8F8", stroke: null })         
        )
      );
    // Make link labels visible if coming out of a "conditional" node.
    // This listener is called by the "LinkDrawn" and "LinkRelinked" DiagramEvents.
    function showLinkLabel(e) {
      <!-- var label = e.subject.findObject("LABEL"); -->
      <!-- if (label !== null) label.visible = (e.subject.fromNode.data.figure === "Diamond"); -->
    }
	
    // temporary links used by LinkingTool and RelinkingTool are also orthogonal:
    myDiagram.toolManager.linkingTool.temporaryLink.routing = go.Link.Orthogonal;
    myDiagram.toolManager.relinkingTool.temporaryLink.routing = go.Link.Orthogonal;
   // load an initial diagram from some JSON text
    // initialize the Palette that is on the left side of the page
    myPalette =
      $(go.Palette, "myPaletteDiv",  // must name or refer to the DIV HTML element
        {
          "animationManager.duration": 800, // slightly longer than default (600ms) animation
          nodeTemplateMap: myDiagram.nodeTemplateMap,  // share the templates used by myDiagram
          model: new go.GraphLinksModel([  // specify the contents of the Palette
           { category: "cashreceipt", text: "cashreceipt" },
			{ category: "credit", text: "credit" },
			{ category: "customers", text: "customers" },
			{ category: "oe", text: "oe" },
			{ category: "poe", text: "poe" },
			{ category: "poinq", text: "poinq" },
			{ category: "post", text: "post" },
			{ category: "shipping", text: "shipping" },
			{ category: "AP", text: "AP" },
			{ category: "AR", text: "AR" },
			{ category: "audit", text: "audit" },
			{ category: "billing", text: "billing" },
			{ category: "cashdisp", text: "cashdisp" },
			{ category: "cashreceipt", text: "cashreceipt" },
			{ category: "checkprint", text: "checkprint" },
			{ category: "commission", text: "commission" },
			{ category: "costing", text: "costing" },
			{ category: "credit", text: "credit" },
			{ category: "creditentry", text: "creditentry" },
			{ category: "creditinquiry", text: "creditinquiry" },
			{ category: "customers", text: "customers" },
			{ category: "dbquery", text: "dbquery" },
			{ category: "ecomwebsite", text: "ecomwebsite" },
			{ category: "edi", text: "edi" },
			{ category: "export", text: "export" },
			{ category: "formprint", text: "formprint" },
			{ category: "GL", text: "GL" },
			{ category: "help", text: "help" },
			{ category: "hidden", text: "hidden" },
			{ category: "holdrelease", text: "holdrelease" },
			{ category: "import", text: "import" },
			{ category: "inquiry", text: "inquiry" },
			{ category: "invinquiry", text: "invinquiry" },
			{ category: "invM", text: "invM" },
			{ category: "kitting", text: "kitting" },
			{ category: "labels", text: "labels" },
			{ category: "Minfo", text: "Minfo" },
			{ category: "modify", text: "modify" },
			{ category: "notes", text: "notes" },
			{ category: "oe", text: "oe" },
			{ category: "poe", text: "poe" },
			{ category: "POentry", text: "POentry" },
			{ category: "poinq", text: "poinq" },
			{ category: "POS", text: "POS" },
			{ category: "post", text: "post" },
			{ category: "pricing", text: "pricing" },
			{ category: "pricingavail", text: "pricingavail" },
			{ category: "quote", text: "quote" },
			{ category: "receiving", text: "receiving" },
			{ category: "rentalentry", text: "rentalentry" },
			{ category: "rentalinq", text: "rentalinq" },
			{ category: "reporting", text: "reporting" },
			{ category: "setup", text: "setup" },
			{ category: "shipping", text: "shipping" },
			{ category: "systemsetup", text: "systemsetup" },
			{ category: "utility", text: "utility" },
			{ category: "vendors", text: "vendors" },		
            { category: "Comment", text: "Comment" }
          ])
        });
		load();
		 myOverview =
      $(go.Overview, "myOverviewDiv",  // the HTML DIV element for the Overview
        { observed: myDiagram, contentAlignment: go.Spot.Center });   // tell it which Diagram to show and pan
		myDiagram.groupTemplate =
      $(go.Group, "Vertical",
        { selectionObjectName: "PANEL",  // selection handle goes around shape, not label
          ungroupable: true },  // enable Ctrl-Shift-G to ungroup a selected Group
        $(go.TextBlock,
          {
            font: "bold 19px sans-serif",
            isMultiline: true,  // don't allow newlines in text
            
          },
          new go.Binding("text", "text")),
        $(go.Panel, "Auto",
          { name: "PANEL" },
          $(go.Shape, "Rectangle",  // the rectangular shape around the members
            { fill: "rgba(128,128,128,0.2)", stroke: "gray", strokeWidth: 3 }),
          $(go.Placeholder, { padding: 10 })  // represents where the members are
		  
        )
		
      );
	  
 	

  }
  
  // Make all ports on a node visible when the mouse is over the node
  function showPorts(node, show) {
	  
    var diagram = node.diagram;
    if (!diagram || diagram.isReadOnly || !diagram.allowLink) return;
    node.ports.each(function(port) {
        port.stroke = (show ? "white" : null);
      });
  }
  // Show the diagram's model in JSON format that the user may edit
  function save() {
    document.getElementById("mySavedModel").value = myDiagram.model.toJson();
    myDiagram.isModified = false;
  }
  	  function modeChange() {	  
    var tool = myDiagram.toolManager.findTool("FreehandDrawing");
    tool.isEnabled = false;
  }
  function load() {	  	  
  
  modeChange();
  $("#myPaletteDiv").css("visibility","hidden");
    myDiagram.model = go.Model.fromJson(document.getElementById("mySavedModel").value);	
    //myDiagram.maybeUpdate(); // force measure before selecting object for inspector
    myDiagram.select(myDiagram.nodes.first()) 
  }
  // add an SVG rendering of the diagram at the end of this page
    // Call it with some default values
  function UserAction() {
    var xhttp = new XMLHttpRequest();
	var datas =document.getElementById("mySavedModel").value;
    xhttp.open("Get", "http://localhost:38051/DistOneAPi/SetDiagram?DiagramData="+datas, true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();
    var response = xhttp.responseText;
	alert(xhttp.responseText);
}
 function toolEnabled() {
      var enable = document.getElementById("ToolEnabled").checked;
      var tool = myDiagram.toolManager.findTool("DragCreating");
      if (tool !== null) tool.isEnabled = false;
    }
  
$(function(){
	
	$('#sample').trigger('onload');	
	// document.getElementById("DSave").addEventListener("click", UserAction);
});

function showhideprop(){
	alert("helllo");
// $("#Inspec").toggle();
}

$(document).ready(function(){
     var x=$(window).width();
    var y=$(window).height();
     var a=x/2;
     var b=y/2;

    $("#main1").css('width', x);
     $("#main1").css('height', y);
});

 // $(document).bind("contextmenu",function(e){
  // e.preventDefault();
  // console.log(e.pageX + "," + e.pageY);
  // $("#myPaletteDiv").css("left",e.pageX);
  // $("#myPaletteDiv").css("top",e.pageY);    
  // $("#myPaletteDiv").css("position","absolute");    
  // $("#myPaletteDiv").css("visibility","visible");
  // $("#myPaletteDiv").fadeIn(200,startFocusOut());      
// });

function startFocusOut(){
  $(document).on("click",function(){
  $("#myPaletteDiv").hide();        
  $(document).off("click");
  });
}