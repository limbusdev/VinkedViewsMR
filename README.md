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
      <float>0.451337427</float>
      <float>0</float>
    </position>
    <rotation>
      <float>0</float>
      <float>0</float>
      <float>0</float>
      <float>1</float>
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
    <SerializedETV dataSetID="0">
      <position>
        <float>-0.743384</float>
        <float>0.7613374</float>
        <float>0.249999851</float>
      </position>
      <rotation>
        <float>0</float>
        <float>1</float>
        <float>0</float>
        <float>0</float>
      </rotation>
      <visType>SingleAxis3D</visType>
      <variables>
        <string>Property crime rate</string>
      </variables>
    </SerializedETV>
    <SerializedETV dataSetID="0">
      <position>
        <float>-1.22980952</float>
        <float>0.7613374</float>
        <float>0.249999851</float>
      </position>
      <rotation>
        <float>0</float>
        <float>1</float>
        <float>0</float>
        <float>0</float>
      </rotation>
      <visType>SingleAxis3D</visType>
      <variables>
        <string>Rape (rev)</string>
      </variables>
    </SerializedETV>
  </ETVs>
</Setup>

```