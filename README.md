# Fox Software Deployment and Control

Fox Software Deployment and Control is a progam for Software Deployment & Controling (including computer inventory)
See [here](https://vulpes.lu/#!/sdconprem) for more information about the product

### Prerequisites

* Visual Studio 2017 (I use the Enterprise Edition, other editions may work)
	* .NET 4.5
* [Telerik Reporting Components](https://www.telerik.com/products/reporting.aspx) Version 12.0.18.227
	* Server part can be slightly modified to work without them
* 2 Custom certificates to sign updates & data signature (test certificates included)
* [WiX Toolset v3.11.0.1701](https://wixtoolset.org/releases/)
* [WiX Toolset Visual Studio Extension version 0.9.21.62588](https://marketplace.visualstudio.com/items?itemName=RobMensching.WiXToolset)

### How to compile

* Download the repository
* Install the 2 Certificates (`Vulpes_Licensing.pfx` and `Vulpes_Main.pfx`) into the user-store
* Put the `Telerik.Reporting.dll` into `FoxSDC_Server\DLL`
* Open Command Prompt, and go to the `INSTALL` folder and run the `all.cmd` file

### Note

All these tools are provided as-is from [Vulpes](https://vulpes.lu).
If you've some questions, contact me [here](https://go.vulpes.lu/contact).


