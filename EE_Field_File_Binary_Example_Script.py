import os
import pandas as pd
import numpy as np

os.chdir(r“Desired working directory”)

full_matrix = pd.DataFrame(np.random.rand(3651,10000)) #dummy data matrix for demonstration purpose, replace with data desired if needed 

ITYPE = 0		# Input format (0 = all cells)
NT=3650		# NUMBER OF TIME STEPS
NC=1		# Number of components (e.g., 1 for pressure, 2 for wind-X, wind-Y)
NL=10000		# Number of cells (active cell count = LA - 1 = LC - 2)
NK=1		# Number of layers
ITOPT=1		# Interpolation between time steps (0 = No interp., 1 = Linear interp.)
IUPDATE=0	# Cell values will be replace by the reading values
IDIST=0		# Cell value from FLD file times cell area
NODATA=-999	# No data. Ignore updating for cells with this value
TSCALE=86400	# Time scale, amount of second when timestamp increase by 1
TSHIFT=0
VSCALE=1
VSHIFT=0
YYYY=2005	#
MM=9		#
DD=30		#
L2=0

fo = open(fileName, 'wb')  #create file and open with write binary mode

signature = bytearray([70, 76, 68, 49]) # signature must be exact
fo.write(signature)
fo.write(np.uint32(ITYPE))  #data type for each header input must be exact
fo.write(np.uint32(NT))
fo.write(np.uint32(NC))
fo.write(np.uint32(NL))
fo.write(np.uint32(NK))
fo.write(np.uint32(ITOPT))
fo.write(np.uint32(IUPDATE))
fo.write(np.uint32(IDIST))
fo.write(np.float32(NODATA))
fo.write(np.float32(TSCALE))
fo.write(np.float32(TSHIFT))
fo.write(np.float32(VSCALE))
fo.write(np.float32(VSHIFT))
fo.write(np.uint32(YYYY))
fo.write(np.uint32(MM))
fo.write(np.uint32(DD))
fo.write(np.uint32(L2))
fo.write(np.uint32(L2))
fo.write(np.uint32(L2))

for t in range(0,NT):
    fo.write(np.float64(t+1))
    fo.write(np.uint32(NL))
fo.write(np.float32(full_matrix.iloc[t,:].values))

fo.close()
