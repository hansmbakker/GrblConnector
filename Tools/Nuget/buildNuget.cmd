@echo off

@echo *******************************************
@echo * COPYING BINARIES FOR NUGET              *
@echo *******************************************
REM msbuild ..\..\src\GrblConnector.sln /t:Rebuild /p:Configuration=Release
msbuild ..\..\src\GrblConnector.WinDesktop\GrblConnector.WinDesktop.csproj /t:Rebuild /p:Configuration=Release
msbuild ..\..\src\GrblConnector.WinStore\GrblConnector.WinStore.csproj /t:Rebuild /p:Configuration=Release
msbuild ..\..\src\GrblConnector.WinPhone\GrblConnector.WinPhone.csproj /t:Rebuild /p:Configuration=Release
msbuild ..\..\src\GrblConnector.UWP\GrblConnector.UWP.csproj /t:Rebuild /p:Configuration=Release
msbuild ..\..\src\GrblConnector.Android\GrblConnector.Android.csproj /t:Rebuild /p:Configuration=Release
msbuild ..\..\src\GrblConnector.iOS\GrblConnector.iOS.csproj /t:Rebuild /p:Configuration=Release
xcopy ..\..\src\bin\Release\GrblConnector.WinStore.dll .\GrblConnector\lib\netcore45\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.WinStore.xml .\GrblConnector\lib\netcore45\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.WinPhone.dll .\GrblConnector\lib\wpa\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.WinPhone.xml .\GrblConnector\lib\wpa\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.WinDesktop.dll .\GrblConnector\lib\net40-client\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.WinDesktop.xml .\GrblConnector\lib\net40-client\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.UWP.dll .\GrblConnector\lib\uap10.0\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.UWP.xml .\GrblConnector\lib\uap10.0\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.Android.dll .\GrblConnector\lib\MonoAndroid10\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.Android.xml .\GrblConnector\lib\MonoAndroid10\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.iOS.dll .\GrblConnector\lib\Xamarin.iOS10\ /Y
xcopy ..\..\src\bin\Release\GrblConnector.iOS.xml .\GrblConnector\lib\Xamarin.iOS10\ /Y


@echo *******************************************
@echo * BUILDING NUGET PAKCAGE					*
@echo *******************************************
nuget pack GrblConnector\GrblConnector.nuspec -o .\
