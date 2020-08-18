# SAP1EMU
[![Build Status](https://travis-ci.org/rbaker26/SAP1EMU.svg?branch=master)](https://travis-ci.org/rbaker26/SAP1EMU) 
![.NET Core](https://github.com/rbaker26/SAP1EMU/workflows/.NET%20Core/badge.svg) 
[![codecov](https://codecov.io/gh/rbaker26/SAP1EMU/branch/master/graph/badge.svg)](https://codecov.io/gh/rbaker26/SAP1EMU)
![GitHub All Releases](https://img.shields.io/github/downloads/rbaker26/SAP1EMU/total?color=blue) 
![GitHub language count](https://img.shields.io/github/languages/count/rbaker26/SAP1EMU) 
![GitHub top language](https://img.shields.io/github/languages/top/rbaker26/SAP1EMU)
![GitHub](https://img.shields.io/github/license/rbaker26/SAP1EMU)
![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/rbaker26/SAP1EMU)

An Emulator for the SAP1 Computer, based off of the SAP1 from _Digital Computer Electronics_ by Malvino and Brown.

## About this Project (User Guide Wiki)
This readme.md will address topics regarding the SAP1Emu Library, Engine and API. <br>
For tutorials, file specifications, instruction sets and other educational information, visit the SAP1Emu Project's [Wiki Page](https://github.com/rbaker26/SAP1EMU/wiki).


## About this Project (Technical)
#### This is outdated, I will update after v2.0.0-rc merges to master. Email me if you have questions.
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
 
These three projects hold all of the core logic and class definitions used by the CLI's and the GUI (WebApp). <br>
The other projects "glue" the core projects together in a way useful to the user.


### SAP1EMU.Lib
The SAP1EMU.Lib project contains definitions for all of the SAP1 Components, Registers and some Utily classes.
Most of the registers and components are implamented using the Observer Pattern using .Net Core's IObservable<T> and IObserver<T> interfaces.  This allows most of the registers and components to be unaware of their counterparts.  All they "pay attention to is the Clock and the Control-Word.
 
A few components are dependent on others, like the MAR and the RAM, but efforts will be taken to decouple them further.

The SAP1EMU.Lib project doesn't contain and "runtime logic".  It is a simple library project with no Main().

### SAP1EMU.Engine
The SAP1EMU.Engine is the main interface for the SAP1EMU.Engine-CLI and SAP1EMU.WebApp.  It handles subscribing the registers to the Clock, running the main loop, and perserving the output of the program.

The main loop is only about 14 lines of code. This simplicity is achived because of the Observer Pattern implamented in the SAP1EMU.Lib.  Besides the Instruction register, the Engine is not "aware" of any of the other registers or components, just like the registers or components are not aware of eachother.  

This decoupling has the huge benefit of be able to add new registers or components with little effort of side effects. <br>
To add a new registers or components, all that is needed is that registers or components and adding its control bits to the Sequencer.  Thats it.


### SAP1EMU.Assembler
The Assembler handles the conversion of SAP1Emu Assembly to SAP1Emu Binary and returns an errors via a ParseException object.
The SAP1EMU.Assembler-CLI or SAP1EMU.WebApp can interpret those errors.

### SAP1EMU.Lib.Test
This project contains all the Unit Tests for the project. The CodeCov metrics are based off of these test. <br>
In GitHub, no Pull Request can merge without all the Unit Tests passing.  

### SAP1EMU.WebApp
The SAP1EMU.WebApp is the culmination of all of the core projects.  It presents a cross-platform GUI that provides Assembly and Engine Runtime functionality to the user.  

The GUI is an ASP.NET Core app wrapped in Electron.Net (a .NET wrapper for Electron).  This allows a single .NET Core app to be deployed on Windows, MacOS and Linux platforms.  

I originally wanted to write a single WPF app only support a GUI for Windwows.  I realized that in education, there are a ton of Mac computers, so I would need to write two apps (WPF and GTK#).  That was going  to be too much work, so Electron.Net seemed to be a great solution. I am trying to keep the JavaScript to be only "glue-code" can keep all of the processing in .Net Core by using the REST API as a go-between.  For one-way comunications, AJAX is used for all REST requests to prevent reloading the ASP.NET Views.
For two-way comunication betweeen the processes, the IPC will be used.

## Adding a New Register
To add a new register or compenent to the SAP1EMU.Lib, there are 4 steps:
 1) Create your Class Definition
 2) Implament the IObserver<T> interface
 3) Update the Sequencer's JSON File to include new control bits for your new register (note only one register can push to the bus at a time)
 4) In the SAP1EMU.Engine, Subscribe the new register to the Clock.
 
 Below is an example of adding a Register called CReg:
 
 ```c#
  public class CReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            // Active Hi, Push on Tic
            if (cw[#] == '0/1' & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus
                Wbus.Instance().Value = RegContent;
            }
            
            // Active Low, Pull on Tok
            if (cw[#] == '0/1' && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in A
                RegContent = Wbus.Instance().Value;
            }
        }
 ```
 
 Add the following code to your class:
 
 ```c#
 #region IObserver Region
     private IDisposable unsubscriber;
     public virtual void Subscribe(IObservable<TicTok> clock)
     {
         if (clock != null)
             unsubscriber = clock.Subscribe(this);
     }

     void IObserver<TicTok>.OnCompleted()
     {
         this.Unsubscribe();
     }

     void IObserver<TicTok>.OnError(Exception error)
     {
         throw error;
     }

     void IObserver<TicTok>.OnNext(TicTok value)
     {
         Exec(value);
     }

     public virtual void Unsubscribe()
     {
         unsubscriber.Dispose();
     }
#endregion
```

In SAP1EMU.Engine/EngineProc.cs, Subscribe your new register to the Clock.
```c#
public void Run()
{

    Clock clock = new Clock();
    TicTok tictok = new TicTok();

    tictok.Init(); 

    AReg areg = new AReg();
    BReg breg = new BReg();
    ...            
    // Create your new class instance
    CReg creg = new CReg();  

    ...

    areg.Subscribe(clock);
    breg.Subscribe(clock);
    ...
    // Subscribe your new register to the clock
    creg.Subscribe(clock);

    ...
    
}
```
