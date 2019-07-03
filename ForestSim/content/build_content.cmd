cd %~dp0
if "%1"=="" (
    echo "ERROR: Missing target directory argument."
    goto END
)
"C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\MGCB.exe" /@:".\Content.mgcb" /platform:Windows /outputDir:"..\%1" /intermediateDir:"..\obj\Windows" /quiet
:END