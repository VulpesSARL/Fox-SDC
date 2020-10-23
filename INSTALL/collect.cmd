rd /s /q CD
md CD
md "CD\ADMX"
md "CD\SDC Agent"
md "CD\SDC Tools"
md "CD\SDC Server"

rd /s /q PDB
md PDB
copy "..\Release\*.PDB" PDB

copy "..\Release\MyUCID.exe" "CD\MyUCID.exe"
copy "..\Release\FoxSDC_Agent.exe" "CD\SDC Agent"
copy "..\Release\FoxSDC_Agent_UI.exe" "CD\SDC Agent"
copy "..\Release\FoxSDC_AgentDLL32.dll" "CD\SDC Agent"
copy "..\Release\FoxSDC_AgentDLL64.dll" "CD\SDC Agent"
copy "..\Release\FoxSDC_ApplyUserSettings.exe" "CD\SDC Agent"
copy "..\Release\FoxSDC_Common.dll" "CD\SDC Agent"
copy "..\Release\FoxSDC_Selfupdate.exe" "CD\SDC Agent"
copy "..\Release\FoxSDC_UninstallData.exe" "CD\SDC Agent"
copy "..\Release\FoxSDC_Configure.exe" "CD\SDC Agent"
copy "..\Release\FoxSDC_SAS32.dll" "CD\SDC Agent"
copy "..\Release\FoxSDC_SAS64.dll" "CD\SDC Agent"

copy "..\Release\FoxSDC_Common.dll" "CD\SDC Tools"
copy "..\Release\FoxSDC_MGMT.exe" "CD\SDC Tools"
copy "..\Release\FoxSDC_PackageCreator.exe" "CD\SDC Tools"
copy "..\Release\FoxSDC_PackageCreatorC.exe" "CD\SDC Tools"
copy "..\Release\FoxSDC_RedirConsole.exe" "CD\SDC Tools"
copy "..\Registry Tricks.txt" "CD\SDC Tools"
copy "..\Usefull EventLog Entries.txt" "CD\SDC Tools"
copy "..\DNS Configuration.txt" "CD\SDC Tools"
copy "..\Release\FoxSDC_RemoteConnect.exe" "CD\SDC Tools"
copy "..\Release\FoxSDC_ManageScreen.exe" "CD\SDC Tools"

copy "..\Release\FoxSDC_Common.dll" "CD\SDC Server"
copy "..\Release\FoxSDC_Server.exe" "CD\SDC Server"
copy "..\Release\FoxSDC_Server.exe.config" "CD\SDC Server"
copy "..\FoxSDC_Server\Blank DB.sql" "CD\SDC Server"
copy "..\FoxSDC_Server\Telerik.ReportDesigner.exe" "CD\SDC Server"
copy "..\FoxSDC_Server\Telerik.ReportDesigner.exe.config" "CD\SDC Server"
copy "..\FoxSDC_Server\DLL\Telerik.Reporting.dll" "CD\SDC Server"

xcopy /e "..\ADMX\*.*" "CD\ADMX\" /y

call msbuild "FoxSDC_Agent_Setup32\FoxSDC_Agent_Setup32.wixproj" /property:Configuration=Release /property:Platform="x86"
call msbuild "FoxSDC_Agent_Setup64\FoxSDC_Agent_Setup64.wixproj" /property:Configuration=Release /property:Platform="x64"

copy "FoxSDC_Agent_Setup64\bin\x64\Release\FoxSDC_Agent_Setup.msi" "CD\FoxSDC_Agent_Setup64.msi"
copy "FoxSDC_Agent_Setup32\bin\Release\FoxSDC_Agent_Setup.msi" "CD\FoxSDC_Agent_Setup32.msi"

call signtool sign /sha1 %SHACERT% "CD\FoxSDC_Agent_Setup32.msi" "CD\FoxSDC_Agent_Setup64.msi"
call "CD\SDC Tools\FoxSDC_PackageCreatorC.exe" compile SDCA.foxps %PKG%

copy SDCA.foxpkg "CD"

del "CD\Fox SDC.iso" > NUL
oscdimg -lFOXSDC -u2 CD "Fox SDC.iso"
move "Fox SDC.iso" CD
