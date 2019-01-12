# Vinked Views

Vinks are visually drawn connection lines (from **v**isual l**ink**) between two or more visualizations. They connect graphical primitives. Visualizations can be connected in different ways. *VinkedViews* supports three of them: Brushing & Linking, VisBridges and Meta-Visualizations.

## VisBridges

A *VisBridge* connects graphical primitives of two or more visualizations. The connected primitives have to be in a relation - be it directy (graphical primitives represent the same *Information Object*) or indirectly (the value, the graphical primitive represents, contains the connected information objects value in some way (sum, mean, ...)).

## Meta-Visualizations

Meta-Visualizations connect two or more visualizations with a new visualization. For example, Two orthogonal 1D-Scatterplots (axes) can be connected by a 2D-Scatterplot.

## Visualization Setup

Setups of visualization can be written in code (hard coded), defined in an XML file or put up manually in the App by using the *Visualization Factory*. 


### Setup with XML

The XML file has to be placed in the persistent data path (see Unity documentation) and must be of the following structure:

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

## Providing Data

CSV files must be UTF-8 coded and with Unix (LF) line endings. Otherwise there will be errors. Additionally they must contain the level of measurement in the second row: ratio, ordinal, nominal or interval. If you provide interval, you must provide the unit name as well, like this:

```
Year,Population,Assaults
interval+year,ratio,ratio
2000,15000,21
2001,15320,21
2002,16011,23
```

Ordinal attributes must be numerical coded, to provide their order as well. To get their categorical values, a dictionary must be provided as well.

CSV file DataBase.csv:

```
Date,Time,Address,Crime,Inside/Outside,Weapon
interval+date,interval+minute,nominal,ordinal,nominal,nominal
43288,1433,1600 PENTWOOD RD,6,,FIREARM
43288,1430,ST & DIVISION ST,5,O,OTHER
43288,1398,2500 PERRING MANOR RD,0,I,OTHER
43288,1361,3700 S HANOVER ST,14,,FIREARM
43288,1375,LOMBARD ST & LIGHT ST,5,I,OTHER
43288,1338,1800 W FRANKLIN ST,0,I,OTHER
```

Lets assume one crime is worse than others, and we have defined an order on crimes - from the worst to the least meaningful, they are coded from high to low. The coding is provided in a dictionary file:

Dictionary_Crime.txt:

```
0:Aggressive assault
1:Arson
2:Assault by threat
3:Auto theft
4:Burglary
5:Common assault
6:Homicide
7:Larceny
8:Larceny from auto
9:Rape
10:Robbery - carjackacking
11:Robbery - commercial
12:Robbery - residence
13:Robbery - street
14:Shooting
```

Note: this order is completely random and is only there to give an example.

Provide both files to the DataProvider, and everything is fine.