# -*- coding: utf-8 -*-
"""
Created on Mon May 11 15:45:23 2020

@author: Anurag

Test Script to read NetCDF data and extracting a timeseries of interest.

You will need to install NetCDF4, xarray and pandas libraries.
"""


import pandas as pd
import xarray as xr
#importing essential libraries

filepath='test.nc'
#location of NetCDF file with the output data.
#In some cases, you may have multiple files where each file is for an 
#individual days.
#In the EEMS interface, click on Tools--> Export to NetCDF files and 
#select proper option to export the data. 

ds = xr.open_dataset(filepath)
#reading the data in a multi-dimensionaldata.


rowNumber=70
columnNumber=30
#row and column numbers refer to i and j mapping of your project.

newdf=pd.Series(data=ds.WSEL[:,rowNumber,columnNumber],index=ds['time'].values)
#extracting the data at specifc row and column, extracting the time stamps
#and making a pandas dataSeries.

newdf.plot()
#plotting the dataSeries 