using Jubilee.Core.Configuration;
using Jubilee.Core.Plugins;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process
{
    public class TaskProvider : ITaskProvider
    {
        private IKernel kernel;
        private ConfigurationSettings configurationSettings;
        public TaskProvider(IKernel kernel, ConfigurationSettings configurationSettings)
        {
            this.kernel = kernel;
            this.configurationSettings = configurationSettings;
        }
        public IEnumerable<ITask> GetNonDependentTasks()
        {
            return kernel.GetAll<ITask>().Where(plugin =>
            {
                return configurationSettings.NonDependentTasks.Contains(plugin.GetType().Name);
            });
        }

        public IEnumerable<ITask> GetTasksThatDependOn(ITask task)
        {
            var tasksThatDependOn = configurationSettings.Tasks.Where(x => x.DependsOn == task.Name);
            var dependentTasks = new List<ITask>();
            foreach (var taskThatDependOn in tasksThatDependOn)
            {
                dependentTasks.Add(kernel.Get<ITask>(taskThatDependOn.Name));
            }
            return dependentTasks;
        }
    }
}
