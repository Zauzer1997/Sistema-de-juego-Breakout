@echo off
title Breakout - Iniciando Proyecto
echo ================================================
echo        INICIANDO BREAKOUT API + JUEGO
echo ================================================

echo.
echo Iniciando la API...

:: Inicia la API en segundo plano (ventana minimizada)
start /min cmd /c "cd /d %~dp0 && dotnet BreakoutAPI.dll"

echo Esperando que la API inicie y cree la base de datos...
echo (Esto puede tardar más la primera vez)
timeout /t 8 /nobreak >nul

echo.
echo Iniciando el juego...
start "" "%~dp0..\Breakout.exe"

echo.
echo ================================================
echo API iniciada en: http://localhost:5000
echo Juego iniciado
echo ================================================
echo.
echo Puedes cerrar esta ventana.
pause