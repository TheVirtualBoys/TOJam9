// Save the current preferences
var startRulerUnits = app.preferences.rulerUnits;
var startTypeUnits = app.preferences.typeUnits;
var startDisplayDialogs = app.displayDialogs;

// Set Photoshop to use pixels and display no dialogs
app.preferences.rulerUnits = Units.PIXELS;
app.preferences.typeUnits = TypeUnits.PIXELS;
app.displayDialogs = DialogModes.NO;

var layerNum = app.activeDocument.layers.length;
var docWidth = app.activeDocument.width;
var docHeight = app.activeDocument.height;
var minWidth;
var minHeight;
var cellWidth;
var cellHeight;
var rows;
var columns;
var i = 0; // Active layer counter


columns = prompt ("Enter the number of columns (1 or more):","1");
rows = prompt ("Enter the number of rows (1 or more):", "1");
cellWidth = prompt ("Enter the width of one cell:", "16");
cellHeight = prompt ("Enter the height of one cell:", "32");

minWidth = cellWidth * columns;
minHeight = cellHeight * rows;

function getLayerSet(name)
{
	var ret = null;
	var doc = app.activeDocument;
	for (var i = 0, len = doc.layerSets.length; i < len; ++i)
	{
		var layerSet = doc.layerSets[i];
		if (layerSet.name == name)
		{
			ret = layerSet;
			break;
		}
	}
	
	return ret;
}

function getList(collection)
{
	var list = new Array();
	for (var i = 0, len = collection.length; i < len; ++i)
	{
		list[i] = collection[i];
	}
	
	return list;
}

function foreach(collection, callback)
{
	var items = getList(collection);
	for (var i = 0, len = items.length; i < len; ++i)
	{
		callback(items[i]);
	}
}

function moveToLayerSet(layerSet)
{
	return function(layer)
	{
		if (!layer.allLocked && !layer.isBackgroundLayer)
		{
			layer.move(layerSet, ElementPlacement.INSIDE);
		}
	}
}

function duplicateIntoLayerSet(layerSet)
{
	return function(layer)
	{
		var newLayer = layer.duplicate();
		newLayer.move(layerSet, ElementPlacement.INSIDE);
	}
}

function translateMergedLayers(mergedLayerSet)
{
	var items = getList(mergedLayerSet.artLayers);
	var i = 0;
	
    for (var rowCount = 1; rowCount<=rows; rowCount++) 
    {
        for (var colCount =1; colCount<=columns; colCount++)
        {
			//for each layer in the merged set move the image around to the proper spot
			i++;
			if (i <= items.length)
			{
                var curLayer = items[items.length-i];
				curLayer.translate(cellWidth * (colCount-1), cellHeight * (rowCount-1));
            };
        };
    };
	
}


var inp = confirm("The sprite frames will be arranged in "+columns+" columns and "+rows+" rows.\rThe sprite sheet will be "+columns*docWidth+" wide and "+rows*docHeight+" tall. \r\rContinue?");
if (inp==true)
{
	var doc = app.activeDocument;
	
	if (docWidth < minWidth || docHeight < minHeight)
	{
		doc.resizeCanvas(Math.max(docWidth, minWidth), Math.max(docHeight, minHeight), AnchorPosition.TOPLEFT);
	}

	var workingSet = getLayerSet("working");
	if (workingSet == null)
	{
		workingSet = doc.layerSets.add();
		workingSet.name = "working";

		//move the current layers into the working set
		foreach(doc.artLayers, moveToLayerSet(workingSet));
	}

	var mergedLayerSet = getLayerSet("merged");
	if (mergedLayerSet != null)
	{
		mergedLayerSet.allLocked = false;
		mergedLayerSet.remove();
	}

	//create the merged layer set and duplicate all layers into there
	mergedLayerSet = doc.layerSets.add();
	mergedLayerSet.name = "merged";
	foreach(workingSet.artLayers, duplicateIntoLayerSet(mergedLayerSet));
	
	translateMergedLayers(mergedLayerSet);
	var mergedLayer = mergedLayerSet.merge();
	mergedLayerSet = doc.layerSets.add();
	mergedLayerSet.name = "merged";
	mergedLayer.move(mergedLayerSet, ElementPlacement.INSIDE);
	
	mergedLayerSet.allLocked = true;
	
    alert("Completed!");
}
else
{
    alert("Cancelled!");
}

// Reset the application preferences
app.preferences.rulerUnits = startRulerUnits;
app.preferences.typeUnits = startTypeUnits;
app.displayDialogs = startDisplayDialogs;