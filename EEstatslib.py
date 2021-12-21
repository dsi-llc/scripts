# -*- coding: utf-8 -*-
"""
Created on Wed Sep  1 11:32:51 2021

This script provides general statistic numbers calculations for model-data comparison
This should be use as a follow up script after using dffromdatfile function in dataFrameFromdatfiles.py
Default input format are pandas dataframe and series

@author: DSI
"""

import numpy as np

# ------------------------------------------------------------------------------
# statistic functions
# ------------------------------------------------------------------------------

def drop_nan(df):
    """
        this function reads in dataframe after using 
        dffromdatfile function in dataFrameFromdatfiles.py
        then returns a dataframe without nan 
        """
    df_dropped = df.dropna()
    return df_dropped

def data_paired(df):
    """
        this function return the number of data paired
        after dropping nan values
        """
    return df.shape[0]

def bias(s, o):
    """
        Bias
        input:
            s: simulated
            o: observed
        output:
            bias
        """
    return np.mean(s-o)

def rbias(s, o):
    """
        Relative Bias
        input:
            s: simulated
            o: observed
        output:
            relative bias
        """
    return 100*(np.sum(s-o))/np.sum(o)
            

def mae(s, o):
    """
        Mean(Average) Absolute Error
        input:
            s: simulated
            o: observed
        output:
            mean absolute error
        """
    return np.mean(np.abs(s-o))

def rmse(s, o):
    """
        Root Mean Squared Error
        input:
            s: simulated
            o: observed
        output:
            root mean squared error
        """
    return np.sqrt(np.mean((s-o)**2))

def rrmse(s, o):
    """
        Relative Root Mean Squared Error
        input:
            s: simulated
            o: observed
        output:
            relative root mean squared error
        """
    return 100*np.sqrt(np.mean((s-o)**2))/(o.max()-o.min())

def correlation(s, o):
    """
        Correlation Coefficient
        input:
            s: simulated
            o: observed
        output:
            correlation coefficient
        """
    return np.corrcoef(o, s)[0, 1]

def r_sqr(s, o):
    """
        R Squared (Square of Correlation Coefficient)
        input:
            s: simulated
            o: observed
        output:
            R Squared
        """    
    return correlation(s, o)**2

def nsi(s, o):
    """
        Nash-Sutcliffe Index of Efficiency
        input:
            s: simulated
            o: observed
        output:
            nash-sutcliffe index of efficiency
        """
    return 1-np.sum((s-o)**2)/np.sum((o-np.mean(o))**2)

def coe(s, o):
    """
        Coefficient of Efficiency
        input:
            s: simulated
            o: observed
        output:
            coefficient of efficiency
        """
    return 1 - np.sum(np.abs(s-o))/np.sum(np.abs(o-np.mean(o)))

def ioa(s, o):
    """
        Index of Agreement
        input:
            s: simulated
            o: observed
        output:
            index of agreement
        """
    return 1 - (np.sum((o-s)**2))/\
               (np.sum((np.abs(s-np.mean(o))+np.abs(o-np.mean(o)))**2))

def kge(s, o):
    """
        Kling-Gupta Efficiency
        input:
            s: simulated
            o: observed
        output:
            kgef: kling-gupta efficiency
            cc: correlation
            alpha: ratio of the standard deviation
            beta: ratio of the mean
        """
    cc = correlation(s, o)
    alpha = np.std(s)/np.std(o)
    beta = np.sum(s)/np.sum(o)
    kgef = 1 - np.sqrt((cc-1)**2 + (alpha-1)**2 + (beta-1)**2)
    return kgef, cc, alpha, beta

def stats_summary(df, sim_column_idx=0, obs_column_idx=1, decimals=3):
    """
        Statistics Summary, output all statistics number in dictionary
        input:
            df: dataframe from EE.dat file 
                (default just two columns, model and data)
            sim_column_idx: column index for simulated values (default 0)
            obs_column_idx: column index for observed values (default 1)
            decimals: round all statistics to the given number of decimals (default 3)
        output:
            statsummary: dictionary with all statistics number
        """
            
            
    df_drop = drop_nan(df)
    
    simulated = df_drop.iloc[:, sim_column_idx]
    observed = df_drop.iloc[:, obs_column_idx]
    statsummary = {'Data Paired': data_paired(df_drop),
                   'Bias': np.round(bias(simulated, observed), decimals),
                   'Percent Bias': np.round(rbias(simulated, observed), decimals),
                   'Mean Absolute Error': np.round(mae(simulated, observed), decimals),
                   'RMSE': np.round(rmse(simulated, observed), decimals),
                   'RRMSE': np.round(rrmse(simulated, observed), decimals),
                   'R': np.round(correlation(simulated, observed), decimals),
                   'R-Sqr': np.round(r_sqr(simulated, observed), decimals),
                   'Nash-Sutcliffe Efficiency': np.round(nsi(simulated, observed), decimals),
                   'Coefficient of Efficiency': np.round(coe(simulated, observed),decimals),
                   'Index of Agreement': np.round(ioa(simulated, observed), decimals),
                   'Kling-Gupta Efficiency': np.round(list(kge(simulated, observed))[0], decimals)}
    return statsummary