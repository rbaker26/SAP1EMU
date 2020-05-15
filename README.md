# SAP1EMU
[![Build Status](https://travis-ci.org/rbaker26/SAP1EMU.svg?branch=master)](https://travis-ci.org/rbaker26/SAP1EMU) 
[![codecov](https://codecov.io/gh/rbaker26/SAP1EMU/branch/master/graph/badge.svg)](https://codecov.io/gh/rbaker26/SAP1EMU)
![GitHub All Releases](https://img.shields.io/github/downloads/rbaker26/SAP1EMU/total?color=blue) 
![GitHub language count](https://img.shields.io/github/languages/count/rbaker26/SAP1EMU) 
![GitHub top language](https://img.shields.io/github/languages/top/rbaker26/SAP1EMU)
![GitHub](https://img.shields.io/github/license/rbaker26/SAP1EMU)
![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/rbaker26/SAP1EMU)

An Emulator for the SAP1 Computer, based off of the SAP1 from _Digital Computer Electronics_ by Malvino and Brown.

## About this Project (User Guide Wiki)
This readme.md will address topics regarding the SAP1Emu Library, Engine and API. <br>
For tutorials, file specifications, instruction sets and other educational information, vist the SAP1Emu Project's [Wiki Page](https://github.com/rbaker26/SAP1EMU/wiki).


## About this Project (Technical)
I decided to break this project up into a bunch of different reusable conponents knowing that once the CLI was complete, I wanted to reuse as much code as posible for the GUI.  To do this, I broke the project up into eight district parts. <br>
Below is a diagram of how this project (solution) is set up with its corresponding .csproj files.
```
SAP1EMU.sln/
├── SAP1EMU.Lib/
├── SAP1EMU.Lib.Test/
├── SAP1EMU.Assembler/
├── SAP1EMU.Assembler-CLI/
├── SAP1EMU.Engine/
├── SAP1EMU.Engine-CLI/
└── SAP1EMU.WebApp/
```

Each project file (.csproj) contains only one discrete prortion of the project to allow reusability and structure. The three major building blocks of this project are the following:
 * SAP1EMU.Lib
 * SAP1EMU.Assembler
 * SAP1EMU.Engine
These three projects hold all of the core logic and class definitions used by the CLI's and the GUI (WebApp).
