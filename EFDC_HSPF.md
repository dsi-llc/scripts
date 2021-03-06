# Introduction

Developing a hydrodynamic or a water quality model often requires boundary conditions from other sources, such as meteorological, inflow, outflow, and water quality time series.

These time series may be observed data or model simulation results from other models. A typical application, for instance, might be the simulation of a watershed model and then using the output timeseries from the watershed model as a boundary condition for the downstream EFDC+ model.

Hydrologic Simulation Program-Fortran (HSPF) has been used in the past (e.g., the Tenkiller Lake Model) to develop boundary conditions for the EFDC+ model. Such applications required a multi-step process consisting of running the HSPF model, processing the HSPF output using an external application, running the EFDC+ model, and then processing the EFDC+ output.

This multi-step process can introduce errors and slow the project down. It also makes it difficult to hand over the final model to the client, since the modeler would not be providing a single model, but rather a collection of models that could be difficult for the client to use.

As part of DSI's commitment to the scientific and modeling community, DSI is now providing a template script that modelers can use to run an HSPF model, process the output, run the EFDC+ model, and plot the output. 

This packaged script is especially useful for individual models that take along time to run; sensitivity analyses; uncertainty analyses; and presenting models to clients.

To use this script, user should install the needed software and download the demo models. The links to all the programs and files are provided below. Once installed, users may need to modify the hardcoded locations of executables in the script. 

The relevant links are:

1. [EFDC_HSPF.py](https://github.com/dsi-llc/scripts/blob/master/EFDC_HSPF.py)
2. [Model Files](https://github.com/dsi-llc/scripts/tree/master/EFDC_HSPF_Files)
3. [BASINS](https://www.epa.gov/sites/production/files/2020-04/basins4.5.2020.03.31.exe)
4. [HSPF](https://www.epa.gov/sites/production/files/2020-06/hspf12.5plugin.2020.06.exe)
5. [EEMS](https://www.eemodelingsystem.com/buy/demo-version)
6. [Anaconda Python](https://docs.anaconda.com/anaconda/install/)
7. [WDMToolbox](https://pypi.org/project/wdmtoolbox/)


Users will also need to modify this script for their own applications, based on the number and types of output, model simulation period etc. In some cases, users may need to do some additional data processing. Any additional data processing can be completed directly in python.

Please feel free to ask questions in [our forum](https://www.eemodelingsystem.com/user-center/forum) or raise issues in the [GitHub repository](https://github.com/dsi-llc/scripts/issues).

Anurag Mishra



