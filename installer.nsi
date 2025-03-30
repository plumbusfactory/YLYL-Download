﻿# Define the installer name and output path
OutFile ".\bin\Installer\YLYL-Downloader-Installer.exe"

# Define the default installation directory
InstallDir "$PROGRAMFILES\YLYL-Downloader"

# Define the name of the application
Name "YLYL-Downloader"

# Include the NSIS Modern User Interface
!include "MUI2.nsh"

# MUI settings
!define MUI_ABORTWARNING
!define MUI_WELCOMEPAGE_TITLE "Welcome to the YLYL-Downloader Setup Wizard"
!define MUI_WELCOMEPAGE_TEXT "This wizard will guide you through the installation of YLYL-Downloader.\n\nClick Next to continue."
!define MUI_FINISHPAGE_TITLE "Setup Completed"
!define MUI_FINISHPAGE_TEXT "YLYL-Downloader Setup Wizard has been successfully installed on your computer.\n\nClick Finish to exit the Setup Wizard."
!define MUI_FINISHPAGE_RUN "$INSTDIR\YLYL-Download.exe"   ; Launch application

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

# Reserve space for uninstaller
!insertmacro MUI_UNPAGE_INSTFILES

# Sections define what the installer does
Section "Install"
  # Create the installation directory
  SetOutPath "$INSTDIR"

  # Copy files from the publish folder
  File /r "C:\Users\Whalerus\source\repos\YLYL-Download\bin\Release\net8.0-windows\*.*"

  # Create uninstaller entry
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  WriteRegStr HKLM "Software\MyApp" "InstallDir" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\YLYL-Downloader" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\YLYL-Downloader" "DisplayName" "GrittyEnergy YLYL Downloader"
SectionEnd

Section "Uninstall"
  # Remove installed files
  Delete "$INSTDIR\*.*"
  RMDir "$INSTDIR"

  # Remove uninstaller entry
  DeleteRegKey HKLM "Software\YLYL-Downloader"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\YLYL-Downloader"
SectionEnd
