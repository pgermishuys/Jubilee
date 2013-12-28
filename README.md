Jubilee
=======

.NET Sidekick

We all have our workflows, some include building and then running tests. Some might build, compile coffee script and then deploy our website to a local IIS.

Jubilee aims to assist the developer in minimizing the effort to automate their workflows.

Getting Started.
================

Ask Jubilee for some help

```
Jubilee.exe -?
```

Scaffold a new ScriptCS Plugin

```
Jubilee.exe -s YourPluginName
```

With Jubilee, you can define dependent plugins. Let's suppose you want to copy files after a successful build. The configuration below shows how to wire up dependent plugins.

Sample Configuration
====================
```
Runner:
  Name: FileSystemWatchingRunner
  Parameters:
    FolderToWatch: f:\SomeFolderToMonitorForChanges
Plugins:
- Name: MSBuild
  DependentPlugins:
  - Name: FileCopy
    Parameters: 
       From: f:\SomeDirectory
       To: f:\SomeOtherDirectory
Notifications:
- Name: ConsoleNotification
- Name: GrowlNotification
```

ScriptCS Support
====================
Jubilee has the ability to run scripts via ScriptCS. The contextual information, such as which directory is being watched as well as the file that changed will be passed to the script. (This is assuming the FileSystemWatchingRunner is being used).

Example Jubilee ScriptCS Configuration
```
Runner:
  Name: FileSystemWatchingRunner
  Parameters:
    FolderToWatch: f:\SomeFolderToMonitorForChanges
Plugins:
- Name: ScriptCS
  Parameters:
     ScriptName: ConsoleWriter.csx
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
