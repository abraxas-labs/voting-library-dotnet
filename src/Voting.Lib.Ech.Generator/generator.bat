@echo off
SETLOCAL enabledelayedexpansion

REM to make sure xscgen is available
dotnet tool restore

REM generate all the classes
echo Generating ABX-Voting-1-0
dotnet xscgen ./Schemas/ABX-Voting-1-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/5=Ech0007_5_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/5=Ech0010_5_1 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0010/7=Ech0010_7_0 -n http://www.ech.ch/xmlns/eCH-0011/8=Ech0011_8_1 -n http://www.ech.ch/xmlns/eCH-0021/7=Ech0021_7_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0046/4=Ech0046_4_0 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0097/4=Ech0097_4_0 -n http://www.ech.ch/xmlns/eCH-0098/4=Ech0098_4_0 -n http://www.ech.ch/xmlns/eCH-0135/1=Ech0135_1_0 -n ABX_Voting_1_0

REM exit if the command failed
if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0157-4-0
dotnet xscgen ./Schemas/eCH-0157-4-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0155/4=Ech0155_4_0 -n Ech0157_4_0

if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0159-4-0
dotnet xscgen ./Schemas/eCH-0159-4-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0155/4=Ech0155_4_0 -n Ech0159_4_0

if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0110-4-0
dotnet xscgen ./Schemas/eCH-0110-4-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0155/4=Ech0155_4_0 -n http://www.ech.ch/xmlns/eCH-0222/1=Ech0222_1_0 -n Ech0110_4_0

if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0228-1-0
dotnet xscgen ./Schemas/eCH-0228-1-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/5=Ech0007_5_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/5=Ech0010_5_1 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0011/8=Ech0011_8_1 -n http://www.ech.ch/xmlns/eCH-0021/7=Ech0021_7_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0045/4=Ech0045_4_0 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0155/4=Ech0155_4_0  -n Ech0228_1_0

if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0045-4-0
dotnet xscgen ./Schemas/eCH-0045-4-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/5=Ech0007_5_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/5=Ech0010_5_1 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0011/8=Ech0011_8_1 -n http://www.ech.ch/xmlns/eCH-0021/7=Ech0021_7_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0135/1=Ech0135_1_0 -n http://www.ech.ch/xmlns/eCH-0155/4=Ech0155_4_0 -n Ech0045_4_0

if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0252-1-0
dotnet xscgen ./Schemas/eCH-0252-1-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_0 -n Ech0252_1_0

if %errorlevel% neq 0 exit /b %errorlevel%

echo Generating eCH-0252-2-0
dotnet xscgen ./Schemas/eCH-0252-2-0.xsd --netCore --separateFiles --output=./Models --nullable --order --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0 -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1 -n Ech0252_2_0

if %errorlevel% neq 0 exit /b %errorlevel%

REM eCH-0252-2-0 references eCH-0155-5-1, but because CandidateType of eCH-0252-2-0 extends eCH-0155-5-1, we need to generate it seperately to remove automatically generated code which leads to a circular reference
echo Generating eCH-0155-5-1
dotnet xscgen ./Schemas/eCH-0155-5-1.xsd --netCore --separateFiles --output=./Models --nullable --order  --serializeEmptyCollections --collectionSettersMode=Public --generatedCodeAttribute- --collectionType="System.Collections.Generic.List`1" --commandArgs- -n http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0 -n http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0 -n http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0 -n http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0 -n http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1 -n Ech0155_5_1

if %errorlevel% neq 0 exit /b %errorlevel%

echo Copying files into C# projects

SET script_dir=%~dp0
SET models_dir=%script_dir%Models
SET project_dir_prefix=%script_dir%..\Voting.Lib.Ech.

REM Enumerate directories in /Models
for /D %%d in ("%models_dir%\*") do (
  REM extract the directory name
  For %%A in ("%%d%") do (
    SET directory_name=%%~nxA
  )
  
  REM copy the files to the correct eCH project. It must already exist
  xcopy /s /y /q "%models_dir%\!directory_name!\*" "%project_dir_prefix%!directory_name!\!directory_name!\"
  
  if %errorlevel% neq 0 exit /b %errorlevel%
)

echo Done