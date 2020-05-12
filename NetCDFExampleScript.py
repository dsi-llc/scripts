# -*- coding: utf-8 -*-
"""
Created on Mon May 11 15:45:23 2020
@author: Anurag Mishra

Test Script to read NetCDF data output by EEMS10.2 and extracting a timeseries of interest.

To download latest version of EEMS, please visit https://www.eemodelingsystem.com/user-center/downloads 
To view latest videos posted on EEMS channel, visit https://www.youtube.com/channel/UCAbxUm-Dm5BAVG3qxtnmlDA 
EEMS User guide is available at https://eemodelingsystem.atlassian.net/wiki/spaces/EK/overview

"""


import pandas as pd
import xarray as xr
#importing essential libraries
#You will need to install NetCDF4, xarray and pandas libraries.

filepath='test.nc'
#location of NetCDF file with the output data.
#In some cases, you may have multiple files where each file is for an 
#individual day. You may have to read all those files through a loop.
#In the EEMS interface, click on Tools--> Export to NetCDF files and 
#select proper option to export the data. 

ds = xr.open_dataset(filepath)
#reading the data in a multi-dimensionaldata.


rowNumber=70
columnNumber=30
#row and column numbers refer to i and j mapping of your project.

newdf=pd.Series(data=ds.WSEL[:,rowNumber,columnNumber],index=ds['time'].values)
#extracting the WSEL data at specifc row and column for all the timesteps,
#extracting the time stamps and making a pandas dataSeries.
#You may have other constituents that you may want to extract. 


newdf.plot()
#plotting the dataSeries 

#
