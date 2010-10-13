using System;
using System.Diagnostics;
using System.Text;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Tasks;
using ThoughtWorks.CruiseControl.Core.Util;

namespace ccnet.fluentbuild.plugin
{
    [ReflectorType("fluentbuild")]
    public class FluentBuild :ITask
    {
        [ReflectorProperty("executable", Required = true)] public string Executable;
        [ReflectorProperty("buildFile", Required = true)] public string BuildFile;
        [ReflectorProperty("baseDirectory", Required = false)] public string BaseDirectory;
        [ReflectorProperty("buildArgs", Required = false)] public string BuildArgs;
        [ReflectorProperty("target", Required = false)] public string Target;
        private ProcessExecutor _executor;

        public FluentBuild()
        {
            _executor = new ProcessExecutor();
            
        }

        private ProcessInfo CreateProcessInfo(IIntegrationResult result)
        {
            //string str = new MbUnitArgument(this, result).ToString();
            Log.Info(string.Format("Running FluentBuild"));
            var info = new ProcessInfo(this.Executable, CreateArgumentString(), BaseDirectory);
            //info.TimeOut = this.Timeout * 1000;
            return info;
        }



        //public virtual void Run(IIntegrationResult result)
        //{
        //    // Initialise the task
        //    this.WasSuccessful = false;
        //    if ((this.currentStatus == null) ||
        //        (this.currentStatus.Status != ItemBuildStatus.Running))
        //    {
        //        InitialiseStatus(ItemBuildStatus.Pending);
        //        currentStatus.Status = ItemBuildStatus.Running;
        //    }

        //    // Perform the actual run
        //    currentStatus.TimeOfEstimatedCompletion = CalculateEstimatedTime();
        //    currentStatus.TimeStarted = DateTime.Now;
        //    try
        //    {
        //        this.WasSuccessful = Execute(result);
        //    }
        //    catch (Exception error)
        //    {
        //        // Store the error message
        //        currentStatus.Error = error.Message;
        //        result.Status = IntegrationStatus.Exception;
        //        throw;
        //    }
        //    finally
        //    {
        //        // Clean up
        //        currentStatus.Status = (this.WasSuccessful) ? ItemBuildStatus.CompletedSuccess : ItemBuildStatus.CompletedFailed;
        //        currentStatus.TimeCompleted = DateTime.Now;

        //        switch (result.Status)
        //        {
        //            case IntegrationStatus.Unknown:
        //            case IntegrationStatus.Success:
        //                result.Status = this.WasSuccessful ? IntegrationStatus.Success : IntegrationStatus.Failure;
        //                break;
        //        }
        //    }
        //}

        public void Run(IIntegrationResult result)
        {
            //return Path.Combine(result.ArtifactDirectory, string.Format(logFilename, LogFileId));
            //delete log file from last run
            //result.BuildProgressInformation.SignalStartRunTask(!string.IsNullOrEmpty(Description) ? Description : 
            //    string.Format("Executing Nant :BuildFile: {0} Targets: {1} ", BuildFile, string.Join(", ", Targets)));

            var processResult = _executor.Execute(CreateProcessInfo(result));
            result.AddTaskResult(new ProcessTaskResult(processResult)); //for this run add the results of execution
            
            //check if an output XML file was created (i.e. the build runner actually ran)
            //if (File.Exists(outputFile))
            //{
            //    result.AddTaskResult(new FileTaskResult(outputFile));
            //}
            //else
            //{
            //    Log.Warning(string.Format("MbUnit test output file {0} was not created", outputFile));
            //}


            //if (processResult.TimedOut)
            //    throw new BuilderException(this, "NAnt process timed out (after " + BuildTimeoutSeconds + " seconds)");
            //return !processResult.Failed;



            //var process = new Process();
            //process.StartInfo.FileName = Executable;
            //process.StartInfo.Arguments = CreateArgumentString();

            ////redirect to a stream so we can parse it and display it
            //process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.UseShellExecute = false;
            //process.StartInfo.ErrorDialog = false;

            //if (!String.IsNullOrEmpty(BaseDirectory))
            //    process.StartInfo.WorkingDirectory = BaseDirectory;

            //process.Start();
        }

        private string CreateArgumentString()
        {
            var sb = new StringBuilder();
            sb.Append(BuildFile);
            sb.AppendFormat(" /c:{0} ", Target);
            sb.AppendFormat(" {0} ", BuildArgs);
            return sb.ToString();
        }
    }
}
