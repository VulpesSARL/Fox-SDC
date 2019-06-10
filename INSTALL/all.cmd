call initvars.cmd

cd ..
call msbuild "Fox Software Deployment and Control.sln" /property:Configuration=Release
timeout 10
call signall.cmd
cd INSTALL
call collect.cmd

