@echo off
powershell -NoProfile -ExecutionPolicy unrestricted -Command "& .\build\build.ps1 %*"