# Introduction

Developing a hydrodynamic or water quality model often requires boundary conditions from other sources, such as meteorological, inflow, outflow, and water quality time series.

These time series may be observed data or model simulation results from other models. A typical application, for instance, might be the simulation of a watershed model and the subsequent use of the output of the watershed model as a boundary condition for the downstream EFDC+ model.

Hydrologic Simulation Program-Fortran (HSPF) has been used in the past (e.g., the Tenkiller Lake Model) to develop boundary conditions for the EFDC+ model. Such applications required a multi-step process consisting of running the HSPF model, processing the HSPF output usng an external application, running the EFDC+ model, and then processing the EFDC+ output.

This multi-step process can introduce errors and slow the project down. It is also makes it difficult to hand over the final model to the client, since the modeler would not be providing a single model, but rather a collection of models that could be difficult for the client to use.

As part of DSI's committment to the scientific and modeling community, DSI is now providing a template script that modelers can use to run an HSPF model, process the output, run the EFDC+ model, and plot the output. 

This packaged script is especially useful for individual models that take along time to run; sensitivity analyses; uncertainty analyses; and presenting models to clients.

The template script can be used on the sample model provided along with the script.  Users will need to modify this script for their own applications, however, based on the number and types of output, model simulation period, etc. In some cases, users may need to do some additional data processing, but that can usually be done directly in python quite easily.

Please feel free to ask questions in our forum or raise issues in the Github repository.

Anurag Mishra



