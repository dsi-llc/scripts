# -*- coding: utf-8 -*-

"""
Created on Thu July 09 11:54:56 2020

@author: Anurag
This program runs a UCI file, extracts the relevant data, prepares an input 
file for the EFDC+ model, and runs the EFDC+ model.
"""
import subprocess
import os
from wdmtoolbox import wdmtoolbox as wdm
from datetime import datetime as dt
import csv
import numpy as np
import pandas as pd
import shutil
import xarray as xr
#importing essential libraries

pHSPFModelPath=r'C:\BASINS45\modelout\EFDC_HSPF'
UCIFile='Demo.uci'
pWDMFile='Demo.wdm'
pEchoFile='Demo.ech'
pHSPFexe=r'C:\BASINS45\models\HSPF\bin\WinHspfLt.exe'
pEFDCModelPath=r'C:\BASINS45\modelout\EFDC_HSPF\DM-14_Lake_T_HYD-WQ_Model\Models\Hyd'

zeroDay=dt.strptime('2012-1-1','%Y-%m-%d') 
#This is the base day for the EFDC model run. The timeseries produced by HSPF
#will be adjusted assuming this to be the zero day

pUCI=os.path.join(pHSPFModelPath, UCIFile) #Location of UCI file name

process = subprocess.Popen([pHSPFexe,'-1', '-1', pUCI],
                           stdout=subprocess.PIPE,
                           universal_newlines=True)
#Run the HSPF model of the Locust Fork for real time data

print('Running HSPF model. Please wait...')
process.wait()
#Wait for the model run to complete

try:
#Read echo file to check the model run
    echoFileContent=open(os.path.join(pHSPFModelPath,pEchoFile),'r')
    echoFileLines=echoFileContent.readlines()
    echoFileContent.close()
    if 'End of Job' in echoFileLines[len(echoFileLines)-1]:
        print('Real Time HSPF model ran successfully!')
    else:
        print('Real Time HSPF model did not run successfully')    
except:
    print('Could not open echo file. Check if any of the files are locked.')



DSNList=[100]
#A list of DSN with the data that is needed for the EFDC+ model
OverAllDF=[]

for DSN in DSNList:
#Read the WDM data and save in a dataframe
    df=wdm.extract(os.path.join(pHSPFModelPath,pWDMFile),DSN)
    df=df['2012-1-1':]
    df=df.resample('D').mean()
    #Clip the data
    df.columns=['HSPF_'+ str(DSN)]
    #Rename the column name
    diff=df.index-zeroDay
    df.index=diff/pd.to_timedelta(1,unit='D')
    #Change the index so that it matches the numerical indexing for EFDC+
    OverAllDF.append(df)

OverAllDF=pd.concat(OverAllDF, axis=1)
#The OverAllDF has all the real time model output and it can be processed to 
#add in the EFDC+ model.

'''Now, write the qser.inp file so that EFDC+ can read it'''
print('Now creating qser.inp file for the EFDC+ model')
pOrigFlowInpFile=os.path.join(pEFDCModelPath,'qser.inp')
shutil.copy2(pOrigFlowInpFile,os.path.join(pEFDCModelPath,'qser_orig.inp'))
pOrigFlowInpFile=os.path.join(pEFDCModelPath,'qser_orig.inp')
pNewFlowInpFile=os.path.join(pEFDCModelPath,'qser.inp')

with open(pOrigFlowInpFile,'r') as origflowfile:
    with open(pNewFlowInpFile,'w') as newflowfile:
        for line in origflowfile:
            if 'Inflow' in line:
                newflowfile.writelines('   1       {} 86400.000         0         1         0         0 ! Inflow\n'.format(len(OverAllDF)))
                newflowfile.writelines('               1.0000000\n')
                break
            newflowfile.writelines(line)

    
OverAllDF.to_csv(pNewFlowInpFile, mode='a', header=False, sep='\t',
                float_format='%10.4f',quoting=csv.QUOTE_NONE)
print('EFDC+ model files created.')
process = subprocess.Popen(os.path.join(pEFDCModelPath,'0run_efdc.bat'),
                        stdout=subprocess.PIPE,shell=True,bufsize=1,
                           universal_newlines=True)
print('Running EFDC+ model. Please wait...')

stdout, stderr = process.communicate()
print(stdout)
process.wait()
#Wait for the model run to complete
print('EFDC+ model run complete')

OutputNCfile=os.path.join(pEFDCModelPath,"#Output",'DSI_EFDC+.nc')
ds = xr.open_dataset(OutputNCfile)

rowNumber=70
columnNumber=30

rowNumber=10
columnNumber=18
#row and column numbers refer to i and j mapping of your project.

newdf=pd.Series(data=ds.WSEL[:,rowNumber,columnNumber],index=ds['time'].values)
#extracting the WSEL data at specifc row and column for all the timesteps,
#extracting the time stamps and making a pandas dataSeries.
#You may have other constituents that you may want to extract. 


newdf.plot()
#plotting the dataSeries 


