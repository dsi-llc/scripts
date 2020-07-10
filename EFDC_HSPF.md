# Introduction

Developing a hydrodynamic and water quality model often requires boundary conditions from other sources. These sources may include meteorological, inflow, outflow, and water quality timeseries.

These timeseries may be observed data or model simulation results from other models. A typical application would be the simulation of a watershed model and using the output of the watershed model as a boundary condition for the downstream EFDC+ model.

Hydrologic Simulation Program-Fortran (HSPF) has been previously used to develop boundary condition for EFDC+ model (e.g. Tenkiller Lake Model). This and similar applications required running the HSPF model, processing the output usng an external application, running the EFDC+ model, and then processing the output.

Multiple steps in this process can introduce errors and also delay the project. It is also difficult to hand over the final model to the client, if it needs multiple steps to complete.

As part of DSI committment to the scientific and modeling community, DSI is providing a template script that can be used by modelers as an example to run a HSPF model, process the output, run the EFDC+ model, and plot an output. The template script can be used on the example model provided along with the script.

This packaging is useful in following cases;

1. Individual models take long time to run
2. Sensitivity analysis
3. Uncertainty analysis
4. Handing over models to clients


Users will need to modify this script for their applications based on the number and types of output, model simulation period etc. In some cases, users may need to do some additional data processing, but that can be easily done directly in python.

Please feel free to ask questions in our forum or raise issue in Github repository.

Anurag Mishra



