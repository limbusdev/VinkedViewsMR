# Vinked Views

Vinks are visually drawn connection lines (from **v**isual l**ink**) between two or more visualizations. They connect graphical primitives. Visualizations can be connected in different ways. *VinkedViews* supports three of them: Brushing & Linking, VisBridges and Meta-Visualizations.

## VisBridges

A *VisBridge* connects graphical primitives of two or more visualizations. The connected primitives have to be in a relation - be it directy (graphical primitives represent the same *Information Object*) or indirectly (the value, the graphical primitive represents, contains the connected information objects value in some way (sum, mean, ...)).

## Meta-Visualizations

Meta-Visualizations connect two or more visualizations with a new visualization. For example, Two orthogonal 1D-Scatterplots (axes) can be connected by a 2D-Scatterplot.

## Visualization Setup

Setups of visualization can be written in code (hard coded) or defined in an XML file. The XML file has to be placed in the persistent data path (see Unity documentation) and must be of the following structure:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Setup xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <!-- Position and Rotation of Visualization Factory -->
  <visFactory>
    <position>
      <float>0</float>
      <float>.5</float>
      <float>0</float>
    </position>
    <rotation>
      <float>0</float>
      <float>0</float>
      <float>0</float>
    </rotation>
  </visFactory>
  
  <!-- Data Set ID, Position, Rotation, Attributes and Visualization Type of ETVs -->
  <!--
	Visualization Types:
		
		* SingleAxis3D
		* ScatterPlot2D
		* ScatterPlot3D
		* PCP2D
		* PCP3D
		* LineChart2D
		* Histogram2D
		* Histogram3D
		* HistogramHeatmap3D
		
	Attributes and Data Set ID:
		
		Look into your Data Set files.
  
  -->
  <!-- Examples -->
  <ETVs>
	<!-- Data Set ID and Visualization Type are defined as XML attributes -->
    <SerializedETV dataSetID="0" visType="SingleAxis3D">
      <position>
        <float>0</float><!-- x component of position vector -->
        <float>2</float><!-- y component of position vector -->
        <float>0</float><!-- z component of position vector -->
      </position>
      <rotation>
        <float>0</float><!-- rotation around x axis -->
        <float>9</float><!-- rotation around y axis -->
        <float>0</float><!-- rotation around z axis -->
      </rotation>
      <variables>
        <string>Property crime rate</string>
      </variables>
    </SerializedETV>
    <SerializedETV dataSetID="0" visType="ScatterPlot2D">
      <position>
        <float>1</float>
        <float>0</float>
        <float>1</float>
      </position>
      <rotation>
        <float>0</float>
        <float>45</float>
        <float>0</float>
      </rotation>
      <variables>
        <string>Year</string>
        <string>Violent crime</string>
      </variables>
    </SerializedETV>
  </ETVs>
</Setup>

```