# -*- coding: utf-8 -*-

import os
import sys
import math
import subprocess
from multiprocessing import current_process, Pool
import time
import psutil
import json

def getDecomp(modelfolder):
    """
    Get information from decomp.jnp and store it as a dictionary
    
    input:
        modelfolder: str, full path of the directory 
                     with subdirectories the models to be run
    
    output:
        dictionary: 'run_type': str, 'MPI' or 'OMP'
                    'subdomains': int, values from 'number_active_subdomains'
                    'model_path': str, full path of the model directory
    """
    fullpath = os.path.join(modelfolder, 'decomp.jnp')
    df = open(fullpath, 'r')
    decomp = json.load(df)
    domainCount = decomp['number_active_subdomains']
    if domainCount > 1:
        run_type = 'MPI'
    else:
        run_type = 'OMP'
    return {'run_type':run_type, 'subdomains':domainCount, 
            'model_path': modelfolder, 'offset':None}
    
def hexpinlist(domainCount, coresPerDomain, offset):
    """
    This function generates a list of hexidecimal numbers to pin
    the exact cores to use on a computer for MPI runs.

    inputs:
        domainCount: int, domain number in the MPI run
        coresPerDomain: int, cores to use in each domain
        offset: int, cores to offset due to existing MPI runs
    
    outputs:
        pinlist: list, list of a string with hexidecimal numbers 
                 to pin each domain to a specific core
    """
    pinlist = []
    # Assign cores usage for each domain
    # fill cores from the front, towards the back
    for i in range(domainCount):
        # cores used are represent by '1'
        # cores offset are represent by '0'
        pin = '1' * coresPerDomain + \
              '0' * (offset + i * coresPerDomain)
        # convert binary to hexidecimal, drop the first 2 char '0x'
        # then append to list
        pinlist.append(hex(int(pin, 2))[2:])
    # formatting, join all str to one 
    pinlist = ', '.join(pinlist)
    
    return pinlist

def vCoreshexpinlist(domainCount, coresPerDomain, offset):
    """
    This function generates a list of hexidecimal numbers to pin
    the exact cores to use on a computer for MPI runs.

    inputs:
        domainCount: int, domain number in the MPI run
        coresPerDomain: int, cores to use in each domain
        offset: int, cores to offset due to existing MPI runs
    
    outputs:
        pinlist: list, list of a string with hexidecimal numbers 
                 to pin each domain to a specific core
    """
    pinlist = []
    # Assign cores usage for each domain
    # fill cores from the front, towards the back
    for i in range(domainCount):
        # cores used are represent by '1'
        # cores offset are represent by '0'
        cores_used = '10' * coresPerDomain
        cores_used = cores_used[:-1]

        empty_cores = '0' * 2 * (offset + i * coresPerDomain)

        pin = cores_used + empty_cores
        # convert binary to hexidecimal, drop the first 2 char '0x'
        # then append to list
        pinlist.append(hex(int(pin, 2))[2:])
    # formatting, join all str to one 
    pinlist = ', '.join(pinlist)
    
    return pinlist

def autoOffset(index, MAXcoresPerRun):
    offset = index * MAXcoresPerRun

    return offset

def checkCustomOffset(dictionary):
    if dictionary['offset'] is None:
        return 0
    else:
        return 1


class RunHandler:
    
    def __init__(self, folderpath, MAXcoresPerRun, efdc, mpiexec, vcoresFlag):
        self.folderpath = folderpath
        self.MAXcoresPerRun = int(MAXcoresPerRun)
        self.efdc = efdc
        self.mpi = mpiexec
        self.vcoresFlag = vcoresFlag
        self.infoDict = self.getModels()
        
        
    def getModels(self):
        """
        Get all model folders under the given directory, 
        retreive info from decomp.jnp and store it as a dictionary
        
        output:
            modelsInfoDict: dict, key: each subfolder name, 
                                  value: dict, decomp.jnp information
        """
        modelsInfoDict = {folder.name : getDecomp(folder.path) 
                         for folder in os.scandir(self.folderpath) 
                         if os.path.isdir(folder.path)}
        
        return modelsInfoDict
    
    def run(self):
        """
        Execute all the runs under the given directory.
        OMP and MPI runs are run separately, 
        OMP runs will go first, then MPI runs
        After the runs finished, it will write out a json log file.
        """
        OMPmodelsList = [k for k, v in self.infoDict.items() if v['run_type'] == 'OMP']
        MPImodelsList = [k for k, v in self.infoDict.items() if v['run_type'] == 'MPI']
        
        if len(OMPmodelsList) > 0:
            self.infoDict = self.runModels(OMPmodelsList)
        if len(MPImodelsList) > 0:
            print(f'Running MPI...')
            self.infoDict = self.runModels(MPImodelsList)
        
        output_file = os.path.join(self.folderpath, 'run_log.json')
        with open(output_file, 'w') as fo:
            json.dump(self.infoDict, fo)
    
    def updateResults(self, results):
        """
        Update the infoDict with run information
        
        input: 
            results: list, list of individual model run's dictionary
        output:
            self.infoDict: updated infoDict
        """
        cleanResults = self.infoDict
        
        for k, v in cleanResults.items():
            for item in results:
                if item['model_path'] == v['model_path']:
                    v.update(item)
        return self.infoDict
    
    def runModels(self, runList):        
        """
        This function computes max available cores and 
        execute all runs in the runlist through parallel processing.

        inputs:
            runList: list, list of configuration dictionaries for each test pairs
        
        outputs:
            self.rebuildResults(results): dict, updated configuration dictionary
        """
        # Get number of total cores on the computer
        availableCores = psutil.cpu_count(logical=False)
        # Minus 2 so that the runs don't occupy every core and bog down the system
        maxRuns = math.floor((availableCores - 2) / self.MAXcoresPerRun) 
        # Assign workers based on max available runs at once to prevent overlapping
        p = Pool(maxRuns)
        # Apply "runModels" function to all item in runList, run all tasks at once
        print(f'Start runModels')
        results = p.map(self.runModel, runList)
        print(f'Finish runModels.')
        
        return self.updateResults(results)
        
        
    def runModel(self, item):
        """
        This function run comparison models by calling "startSubprocess", 
        after run finished (successful or not) record "runtime" and 
        "error" code to test pair dictionary.

        input:
            item: dictionary, individual test scenario dictionary 
        output:
            item: dictionary, updated test dictionary
        """
        # Get current process and the process id to have control to each task
        process = current_process()
        index = process._identity[0] - 1
        print(f'Inside runModel, Running Model {index+1}')
        #result = None
        # Get start/end time to compute time used for each task
        start = time.time()

        print(f'infoDict[item] keys: {self.infoDict[item].keys()}')
        customoffsetflag = checkCustomOffset(self.infoDict[item])
        print(f"Check run_type: {self.infoDict[item]['run_type'] }")
        print(f'Check customoffsetflag (0/1): {customoffsetflag}')

        # run is OMP
        if self.infoDict[item]['run_type'] == 'OMP':
            # auto offset
            if customoffsetflag == 0:
                offset = autoOffset(index, self.MAXcoresPerRun)
                result = self.startSubprocess(self.infoDict[item], offset)
            # custom offset
            elif customoffsetflag == 1:
                result = self.startSubprocess(self.infoDict[item], self.infoDict[item]['offset'])
        # run is MPI
        elif self.infoDict[item]['run_type'] == 'MPI':
            # auto offset
            if customoffsetflag == 0:
                print(f'Inside run_type: MPI, auto offset')
                offset = autoOffset(index, self.MAXcoresPerRun)
                print(f'Index: {index}, maxcoresperrun: {self.MAXcoresPerRun}, Offset: {offset}')
                result = self.startSubprocess(self.infoDict[item], offset)
            # custom offset
            elif customoffsetflag == 1:
                result = self.startSubprocess(self.infoDict[item], self.infoDict[item]['offset'])

        print(f'Result after: {result}')

        end = time.time()
        # if run success (result == 0) record time used to "runtime" key in dictionary
        # else record error code and time used
        if(result == 0):
            self.infoDict[item]["runtime"] = time.strftime("%H:%M:%S", time.gmtime(end - start))
            self.infoDict[item]["status"] = 'Finished'
        else:
            self.infoDict[item]["runtime"] = time.strftime("%H:%M:%S", time.gmtime(end - start))
            self.infoDict[item]["error"] = result
            self.infoDict[item]["status"] = 'Failed'
        
        return self.infoDict[item]
    
    def startSubprocess(self, item, offset, domainCount=1):
        """
        This function write and run batch file for comparison model.
        
        input:
            item: dictionary, comparison test pairs (each scenario)
            offset: int, cores to offset to accomodate OMP runs
        output:
            cp.returncode: 0 is pass, other is fail
        """
        try:
            # if no domain count input (default 1), OMP run, write correspond batch file
            # else, have domain count input, MPI run, write correspond batch file
            domainCount = item['subdomains']
            if domainCount == 1:
                batfile = self.writeBatFile(item, offset)
                print('OMP')
            else:
                batfile = self.writeBatFile(item, offset, domainCount)
                print('MPI')
                #print(domainCount)
            print(batfile, offset)
            print(f'Starting subprocess: {item["model_path"]}')
            # Run batch file for the run, end run after 7200 seconds
            cp = subprocess.run([batfile], capture_output=True, check=True, encoding="utf-8", timeout=7200)
            if (cp.returncode == 0):
                print(f'Subprocess completed successfully: {item}')
            else:
                print(f'Failed to run EFDC. Error code: {cp.returncode} for model {item}')
            return cp.returncode   
        
        # Log specific error to dictionary
        except subprocess.TimeoutExpired as err:
            print(f'{item["model_path"]}:')
            print(err)
        except subprocess.CalledProcessError as err:
            print(f'{item["model_path"]}:')
            print(err)
        
    def writeBatFile(self, item, offset=0, domainCount=1):
        """
        This function writes the batch file for each comparison run

        input:
            config: dictionary, individual test pair
            offset: int, cores to offset, default 0

        output:
            batpath: string, batch file full file path with filename included
        """
        model_name = os.path.basename(item['model_path'])
        batpath = os.path.join(item['model_path'], '0run_efdc.bat')
        efdc =self.efdc
        
        f = open(batpath, "w")
        if(domainCount > 1):
            coresPerDomain = self.MAXcoresPerRun // domainCount
            if self.vcoresFlag == 1:
                corehexList = vCoreshexpinlist(domainCount, int(coresPerDomain), offset)
            else:
                corehexList = hexpinlist(domainCount, int(coresPerDomain), offset)
            try:
                mpi_command = f'"{self.mpi}" -n {domainCount} -genv I_MPI_PIN_DOMAIN="[{corehexList}]" -genv OMP_NUM_THREADS={coresPerDomain}'
                commands = [ f"TITLE={item['model_path']}\n", f"CD /d \"{item['model_path']}\"\n",
                            f'start \"{model_name}\" {mpi_command} \"{efdc}\" -NT{coresPerDomain}\n']
            except Exception as err:
                print(f"Failed to run EFDC. Error code: {err} for model {item['model_path']}")
        else:
            commands = [f"SET KMP_AFFINITY=granularity=fine,compact,1,{offset}\n",f"TITLE={item['model_path']}\n", f"CD /d \"{item['model_path']}\"\n",
                        f"start \"{model_name}\" \"{efdc}\" -NT{self.MAXcoresPerRun}\n"]
        print(commands[-2:])
        f.writelines(commands)
        f.close()
        return batpath
    
    def setoffset(self, offsetlist):
        offsetlist = [int(x) for x in offsetlist.split()]
        for idx, v in enumerate(self.infoDict.values()):
            temp_dict = v
            temp_dict['offset'] = offsetlist[idx]
            v.update(temp_dict)
            

    
if __name__ == '__main__':
    modelFolder = input("Full path of the models folder: \n")
    
    efdc = input("Full path of the EFDC+ executable: \n")

    mpiexec = input("Full path of the mpiexec.exe: \n")

    virtualCoresFlag = input("Computer have virtual cores? (0/1): ")
    
    MAXcoresPerRun = input("Maximum cores for each run: \n")

    runBatch = RunHandler(modelFolder, MAXcoresPerRun, efdc, mpiexec, virtualCoresFlag)
    
    customOffsetFlag = input("Custom offset? (0/1): ")
    if int(customOffsetFlag) == 0:
        print("Auto offset\n")
        time.sleep(5)
        runBatch.run()
    else:
        customOffsetList = input("Custom offset numbers for all runs: \n")
        runBatch.setoffset(customOffsetList)
        #print("Custom offset\n")
        for k, v in runBatch.infoDict.items():
            print(f"{k} offset: {v['offset']}\n")
        time.sleep(5)
        runBatch.run()
        