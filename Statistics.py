# -*- coding: utf-8 -*-
"""
Created on Fri Jul 23 15:53:06 2021

@author: P H DUY
"""
import pandas as pd
import math


#import dataset
df = pd.read_csv("Field Facility_Model_Data_2015.csv", sep=",")

x = df["Measured"]
y = df["Predicted"]

err = y - x
N = len(x) # number of data paired
sumE = sum(err)
sumA = sum(abs(err))
sumE2 = sum(err**2)
sumX = sum(x)
sumY = sum(y)
sumX2 = sum(x**2)
sumY2 = sum(y**2)
sumXY = sum(x*y)

minX = min(x)
maxX = max(x)
minY = min(y)
maxY = max(y)

Sxx = N*sumX2 - sumX**2
Syy = N*sumY2 - sumY**2
Sxy = N*sumXY - sumX*sumY

R2 = (Sxy * Sxy) / (Sxx * Syy) # Coefficient of Determination
R = math.sqrt(R2)

meanE = sumE/N # ME: Mean Error
meanA = sumA/N # MEA: Average Absolute Error, Mean Error Absolute
RMSE = math.sqrt(sumE2/N) # Root Mean Square Error
RRMSE = 100 * RMSE / (maxX - minX) # % Relative Root Mean Square Error
# initialise data of lists
table = {'# Data Paired':[N],'ME':[meanE], 'MEA':[meanA], 'RMSE':[RMSE], 
         'RRMSE':[RRMSE], 'R':[R], 'R2':[R2]}

# Create DataFrame
df1 = pd.DataFrame(table)
df1.ME = df1.ME.round(3)
df1.MEA = df1.MEA.round(3)
df1.RMSE = df1.RMSE.round(3)
df1.RRMSE = df1.RRMSE.round(3)
df1.R = df1.R.round(3)
df1.R2 = df1.R2.round(3)

#print(df1)
print(df1.to_string(index=False))

