Jubilee
=======

.NET Sidekick

We all have our workflows, some include building and then running tests, some might build, compile coffee script and then deploy our website to a local IIS.

Jubilee aims to assist the developer in minimizing the effort to automate their workflows.

###Runners###
Jubilee relies on a runner to kick off a workflow. The built in runner is a file system watching runner. The file system watching runner will monitor all the files in a particular folder and by default will start a build of the solution found in that folder.

You may provide your own runners by implementing the following IRunner interface
```public interface IRunner
{
	void Run(string workingPath, string filePatternToWatch = "*.*");
}```
You can then register the runner using the configuration builder.

###Plugins###
a Jubilee workflow is built using plugins.
```public interface IPlugin
{
	bool Process(string workingPath);
}```

Plugins can rely on the succesful processing of other plugins.
For example, we only want our tests to run on a successful build. 

```public interface IDependsOnPlugin<Plugin> where Plugin : IPlugin
{
}```
