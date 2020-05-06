call initvars.cmd

cd ..

del "INSTALL\FoxSDC_Agent_Setup32\bin\Release\FoxSDC_Agent_Setup.msi"
del "INSTALL\FoxSDC_Agent_Setup64\bin\x64\Release\FoxSDC_Agent_Setup.msi"

call msbuild "Fox Software Deployment and Control.sln" /property:Configuration=Release
timeout 10
call signall.cmd
cd INSTALL
call collect.cmd

