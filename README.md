Jubilee
=======

.NET Sidekick

We all have our workflows, some include building and then running tests. Some might build, compile coffee script and then deploy our website to a local IIS.

Jubilee aims to assist the developer in minimizing the effort to automate their workflows.

Getting Started.
================

Setting up your workflow
Jubilee has a default configuration file "configuration.yaml" which gives you an example of a workflow.

Jubilee assumes that you will be watching the folder it was dropped in if you do not supply a path name. It will then monitor for changes and if will build the visual studio solution it finds.

With Jubilee, you can define dependent plugins. Let's suppose you want to copy files after a successful build. The configuration below shows how to wire up dependent plugins.

Sample Configuration
====================
```
Runner:
  Name: FileSystemWatchingRunner
Plugins:
- Name: MSBuild
  DependentPlugins:
  - Name: FileCopy
    Parameters: C:\SomeFolder->F:\AnotherFolder
Notifications:
- Name: ConsoleNotification
- Name: GrowlNotification
```

ScriptCS Support
====================
Jubilee has the ability to run scripts via ScriptCS. The contextual information, such as which folder is being watched as well as the file that changed will be passed as arguments. (This is assuming the FileSystemWatchingRunner is being used).

Example Jubilee ScriptCS Configuration
```
Runner:
  Name: FileSystemWatchingRunner
Plugins:
- Name: ConsoleWriter.csx
Notifications:
- Name: ConsoleNotification
- Name: GrowlNotification
```

Example ScriptCS script
```
Console.Write(ScriptArgs[0]); //The name of the ScriptCS Script being run
Console.Write(ScriptArgs[1]); //The directory currently being watched by Jubilee
Console.Write(ScriptArgs[2]); //The file that changed the kicked off the workflow

Console.Error.WriteLine("failed"); // will produce an error in Jubilee
```
