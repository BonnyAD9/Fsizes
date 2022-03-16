# Fsizes
This app helps managing space on disk by displaying sizes of folders

## Why not use normal file explorer
File explorers usualy don't display sizes of folders because they wold have to visit each file in that folder and subfolders and subsubfolders and subsubsub...

That can be very slow for folders with large amount of files.

This app can display sizes of folders and files in given directory for all folders at once and you can quicky navigate between folders. This can help you find why is your disk so full because you can easily spot large folders and files.

## How it works
This app still has to visit every file. You will have to wait until it visits all the files in the given directory and loads all necesary data. Than you can easily navigate the
files without waiting because all the data is already loded.

## Usage
Pass directory you want to scan as the first argument. Otherwise the working directory will be chosen.

To navigate in the file system type name of the folder you want to open from the shown list. (folders have `|-` before their size, files have only `|`)

To go back type `..` (there is currently bug where this sometimes doesn't work).

To close the app type `:exit`.

## Example output
```
C:\
 |- 0              $WinREAgent
 |- 0              AvidDownloads
 |- 0              Config.Msi
 |- 0              Documents and Settings
 |- 0              ESD
 |- 0              MSOCache
 |- 0              OneDriveTemp
 |- 0              PerfLogs
 |- 0              Recovery
 |- 0              System Volume Information
 |- 0              temp
 |- 444     B      MSI
 |- 15.39   KiB    Microsoft
 |- 126.64  KiB    $GetCurrent
 |- 359.78  KiB    $Windows.~WS
 |- 26.99   MiB    $Recycle.Bin
 |- 38.20   MiB    $AV_AVG
 |- 66.12   MiB    tmp
 |- 280.59  MiB    Riot Games
 |- 844.65  MiB    xampp
 |- 3.09    GiB    Tools
 |- 7.06    GiB    texlive
 |- 20.28   GiB    ProgramData
 |- 43.49   GiB    Windows
 |- 73.63   GiB    Program Files (x86)
 |- 94.00   GiB    Program Files
 |- 95.34   GiB    Users
 |  0              desktop.ini
 |  8.00    KiB    DumpStack.log
 |  12.00   KiB    DumpStack.log.tmp
 |  16.00   MiB    swapfile.sys
 |  6.38    GiB    hiberfil.sys
 |  10.00   GiB    pagefile.sys
Total files: 1137500
Total size:  354.52  GiB
 > you can type here :)
```
This was loading about 30 s.
