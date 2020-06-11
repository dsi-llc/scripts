import os
import pandas as pd
import numpy as np

os.chdir(r“Desired working directory”)

matrix = pd.DataFrame(np.random.rand(3651,10000)) #dummy data matrix for demonstration purpose, replace with data desired if needed 

#The following InitialString is optional, because with the asterisk sign in front EFDC+ will  only recognize it as comments
InitialString = """* FILE FORMAT FOR TIME AND SPACE VARYING DATA
*
* Fixed file name convention:  
*   topofld.inp - Topographic data (e.g., dredging/dumping, land erosion/reclaimation)
*   windfld.inp - Wind field (e.g., cyclone)
*   presfld.inp	- Barometric pressure field (e.g., cyclone)
*   rainfld.inp	- Rainfall
*   evapfld.inp	- Evaporation
*   gwspfld.inp - Groundwater fluxes
*   vegefld.inp	- Vegetation field
*   seepfld.inp	- Seepage/groundwater flow
*   sntkfld.inp	- Snow depth
*   snflfld.inp	- Snowfall
*   ictkfld.inp	- Ice thickness
*   shadfld.inp	- Sun shading field
*   ****fld.inp - Other fields
*
*   ITYPE: Input format = 0: ((VALUES(T,N,L,K), K=1,KC), L=2,LA), N=1,NC)
*                       = 1:  L,I,J, (((VALUES(T,N,L,K), K=1,KC), L=1,NL), N=1,NC)
*       NT: NUMBER OF TIME STEPS
*       NC: Number of components (e.g., 1 for pressure, 2 for wind-X, wind-Y)
*       LC: Number of cells (Should = LA = active cell count)
*       NK: Number of layers (Default = 1)
*    ITOPT: Interpolation between time steps (0 = No interp., 1 = Linear interp.)
*  IUPDATE: Flag to direct how to update the current cell value with the reading value
*        0: cell value will be replace by the reading value
*        1: cell value will be added with the reading value
*        2: cell value will be the min. of the current value and the reading value
*        3: cell value will be the max. of the current value and the reading value
*     IFMT: Data operation after interpolation
*        0: Cell value = input value
*        1: Cell value = cell value from FLD file times cell area
*   NODATA: No data. Ignore updating for cells with this value
* YYYY MM DD: Base date
*
*   ITYPE     NT     NC     LC     NK  ITOPT IUPDAT   IFMT NODATA  YYYY MM DD
"""

evapfld = open(‘evapfld.inp’, ‘w’) #create a new file and open it with write mode
evapfld.write(InitialString) # write the above comment section
evapfld.write('        0    3      1   10000      1      1      0      1   -999  2005 09 30\n') # amount of #space between does not need to be exact, as long as its spaced.

# Loop through each day and print out the corresponding data for each day from a matrix

for i in list(range(1,3651)):  # total of 3650 days
	evapfld.write(str(i) + ‘\t10000\n’) # write out the day for the following data block, the 10000 is the amount of cell in the model.
	x = matrix.loc[i] # locate the corresponding data block within a matrix
	x.to_frame().T.to_csv(evapfld, sep=’\t’, header=False, index=False, float_format = ‘%.8f’) # write data block to file

evapfld.close() # close file
