![Jubilee Logo](http://www.pieterg.com/assets/jubilee/jubilee.png)

*Grunt for the .NET developer*

Imagine being able to easily define a workflow that will build your solution and run your tests. We have some of those tools available right now, but why stop there? Imagine being able to compile coffeescript and/or less or restart IIS.

Jubilee aims to assist the developer in minimizing the effort to automate their workflows.

##Getting Started

Jubilee works best with [Growl for Windows](http://www.growlforwindows.com/gfw/ "Growl for Windows"), but is not limited to Growl, it includes a console notifier.

![Jubilee Watching](http://www.pieterg.com/assets/jubilee/watching.png)

Ask Jubilee for some help

```
Jubilee.exe -?
```

With Jubilee, you can define dependent plugins. Let's suppose you want to copy files after a successful build. The configuration below shows how to wire up dependent plugins.

##Sample Configuration
```
Runner:
  Name: FileSystemWatchingRunner
  Parameters:
    FolderToWatch: c:\SomeFolderToMonitorForChanges
Plugins:
- Name: MSBuild
  DependentPlugins:
- Name: FileCopy
  DependsOn: MSBuild
  Parameters: 
    From: c:\SomeDirectory
    To: c:\SomeOtherDirectory
Notifications:
- Name: ConsoleNotification
- Name: GrowlNotification
```

##ScriptCS Support
Jubilee has the ability to run scripts via ScriptCS. The contextual information, such as which directory is being watched as well as the file that changed will be passed to the script. (This is assuming the FileSystemWatchingRunner is being used).

![Jubilee ScriptCS](http://www.pieterg.com/assets/jubilee/hello_from_scriptcs_plugin.PNG)

Scaffold a new ScriptCS Plugin

```
Jubilee.exe -s YourPluginName
```

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
