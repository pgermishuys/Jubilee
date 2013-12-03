Jubilee
=======

.NET Sidekick

We all have our workflows, some include building and then running tests, some might build, compile coffee script and then deploy our website to a local IIS.

Jubilee aims to assist the developer in minimizing the effort to automate their workflows.

Getting Started.
================

Setting up your workflow
Jubilee has a default configuration file "configuration.yaml" which defines a mini-development workflow.

Jubilee assumes that you will be watching the folder it was dropped in if you do not supply a path name to the console runner. It will then monitor for changes and if will build the visual studio solution it finds.

It then has the option of running dependency plugins such as copying files.


Sample Configuration
====================
```
RunnerConfiguration:
  Name: FileSystemWatchingRunner
PluginsConfiguration:
- Name: MSBuild
  DependentPlugins:
  - Name: FileCopy
    Parameters: C:\SomeFolder->F:\AnotherFolder
NotificationsConfiguration:
- Name: ConsoleNotification
- Name: GrowlNotification```
