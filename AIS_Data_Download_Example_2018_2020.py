# -*- coding: utf-8 -*-
"""
Created on Sun May 23 09:25:51 2021

@author: P H Duy
"""

import urllib.request

for year in range(2018, 2021):
    for month in {1, 3, 5, 7, 8, 10, 12}:
        for i in range(1, 32):
            url = 'https://coast.noaa.gov/htdata/CMSP/AISDataHandler/' + str(year) + "/AIS_" + str(year) + "_" + "{:02}".format(month) + "_" + "{:02}".format(i) + '.zip'
            urllib.request.urlretrieve(url, url.split('/')[-1])

    for month in {4, 6, 9, 11}:
        for i in range(1, 31):
            url = 'https://coast.noaa.gov/htdata/CMSP/AISDataHandler/' + str(year) + "/AIS_" + str(year) + "_" + "{:02}".format(month) + "_" + "{:02}".format(i) + '.zip'
            urllib.request.urlretrieve(url, url.split('/')[-1])

    for month in {2}:
        for i in range (1,29):
            url = 'https://coast.noaa.gov/htdata/CMSP/AISDataHandler/' + str(year) + "/AIS_" + str(year) + "_" + "{:02}".format(month) + "_" + "{:02}".format(i) + '.zip'
            urllib.request.urlretrieve(url, url.split('/')[-1])

# Example for file name format
# https://coast.noaa.gov/htdata/CMSP/AISDataHandler/2019/AIS_2019_01_10.zip
