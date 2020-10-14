# Contrubuting 
Welcome to the SAP1Emu Project! We are so glad that you want to help contribute to this Open Source Education Tool.

This guide will go over how to contribute, rules and some common setups so you can hit the ground running.
Before continuing, please also read the [Code of Conduct](CODE_OF_CONDUCT.md).

## Good First Issues - A Good Place to Start

The SAP1Emu Project works off of Issue Tickets when deciding what to work on.  All plans are converted into Issue Tickets, of which there are three types:
* Bug Reports
* Feature Requests 
* Project Workflow Items

As these tickets are created or submitted by users, the Dev Team will label and triage the tickets. At any point, feel free to "take ownership" of a ticket by commenting on it or emailing me (rbaker26) regarding your interest in said ticket. Tickets labeled with **Good First Issue** are those which would be easier for a new-comer or are meant to familiarize the user with the code base. 

Don't see your desired issue? [Open](https://github.com/rbaker26/SAP1EMU/issues/new/choose) a Bug Report or Feature Request.


## Aw Fork
To start working on the project, you will need to fork it.  Once forked, create a new branch and give it a descriptive title (i.e. InputBugFix or CSS_Cleanup). After all that is done, start working on the ticket.  Make sure to add testing if required. When you are satisfied with the work done on the ticket, it's time to submit a Pull Request.


## Pull Request Process
All code changes are done via Pull Request.  When you create a pull-request, a template will populate with all of the required information. Please fill it out completely as it helps us know what we are looking at.  If you don't know which branch to target with your pull request, just set it to master. If need be, someone on the Dev Team will update the branch targeting after it is submitted.  

#### Testing & CI/CD
The SAP1Emu Project is nearly full CI/CD and has a lot of different testing methods built in. This includes Unit Tests, Integration Tests, Coverage Metrics, Benchmarking, and Security Scanning. Except for Coverage Metrics, all other tests must pass for a pull request to be considered.  If the tests do not pass, don't worry. We are here to help. 

If a Coverage Metrics fails, we will consider it on a case-by-case basis.  


#### Final Steps 
Once you pull request is approved, we will ask you to add your name to the [wall](https://sap1emu.net/Home/Contributors).
Your name will not appear on GitHub or the wall until the code makes its way to the master branch.  

Once in master, it will take about 5-10 minutes for the Azure App to update and 1-2 minutes for the GitHub Page to update.



## Building and Running the Project
This is a .Net Core 3.1 project, so it will run on Windows, macOS and Linux. For Windows and macOS, using Visual Studio is the easiestway to get started on this project. Simply open the SLN file with from within Visual Studio and it will load the project for you.

For command-line users (Windows, macOS and Linux) run the following commands from the project directory to start the project.

#### Run the GUI Project
```bash
dotnet restore
dotent build
dotnet run --project SAP1EMU.GUI 
```

#### Run the CLI Project
```bash
dotnet restore
dotent build
dotnet run --project SAP1EMU.CLI
```

#### Run Tests
```bash
dotnet restore
dotent build --configuration release
dotnet run --project SAP1EMU.CLI --configuration release
```
