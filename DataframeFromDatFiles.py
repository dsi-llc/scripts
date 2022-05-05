'''
@author: Anurag Mishra
This module has one function that reads the typical *.dat files exported from EEMS or read in EEMS and returns a dataframe. If the input *.dat file has more than one series, then the dataframe will have more than one series.

The function makes following assumptions
1. Values are tab-separated
2. Typical EEMS output *.dat files have correct dates


'''
import pandas as pd
import os
import re

def dffromdatfile(filename):
    "This function reads the typical *.dat files exported from EEMS or read in EEMS and returns a dataframe."
    with open(filename,'r') as fobj:
        firstline=fobj.readline().rstrip()
        #Reading the first line of the data file.
        if '$' in firstline or '*' in firstline:
            #This is to figure out the type of file.
            frames=[]
            df=pd.DataFrame()
            TotalNumberOfSeries=0
            for line in fobj:
                #Going through lines to figure out number of dataseries.
                if 'NumberOfSeries' in line:
                    TotalNumberOfSeries=int(line.split('=')[1])
                    #Now that we know the number of dataseries in this file, we will exit from this loop.
                    break
            fobj.seek(0,0)
            nlines=0
            SeriesDictionary={}
            linenumber=0
            for seriesnumber in range(1,TotalNumberOfSeries+1):
                
                for line in fobj:
                    linenumber+=1
                    if linenumber<=nlines:continue
                
                    nlines+=1
                    
                    if 'BaseDate' in line:
                        BaseDate=pd.to_datetime(line.split('=')[1])
                        #print(str(BaseDate))
                    if 'Column2' in line:
                        ColumName=line.split('=')[1].rstrip()
                        
                    
                    try:
                        if '$' in line or '*' in line:continue
                        numberofdatapoints=int(re.search('\d+',line.split('\t')[0])[0]) 
                       
                        
                        SeriesDictionary[seriesnumber]=[numberofdatapoints,ColumName, nlines]
                        nlines+=numberofdatapoints
                       
                        break
                        
                        
                    except:
                        '''Nothing Special'''
            for serieskey in SeriesDictionary.keys():
                fobj.seek(0,0)
                df=pd.read_csv(fobj,sep='\t', header=None, names=['DateTime',SeriesDictionary[serieskey][1]], nrows=SeriesDictionary[serieskey][0],parse_dates=True,index_col=[0],skiprows=SeriesDictionary[serieskey][2],na_values=-999)

                frames.append(df)
                

            df=pd.concat(frames,axis=1)
        
        elif len(firstline.split('\t'))==2:
            '''This might have only dataset'''
            df=pd.read_csv(fobj,sep='\t',skiprows=0,header=None, names=['DateTime',os.path.basename(filename)],index_col=0,na_values=-999)
        else:
            '''This might not be timeseries'''
            df=pd.DataFrame()
        
    

    return(df)
