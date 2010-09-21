package jetbrains.buildServer.agent.fluentbuildrunner;

import jetbrains.buildServer.agent.AgentBuildRunnerInfo;
import jetbrains.buildServer.agent.BuildAgent;
import jetbrains.buildServer.agent.BuildAgentConfiguration;
import jetbrains.buildServer.agent.runner.CommandLineBuildService;
import jetbrains.buildServer.agent.runner.CommandLineBuildServiceFactory;
import jetbrains.buildServer.fluentbuildrunner.FluentBuildRunnerConstants;
import org.jetbrains.annotations.NotNull;

/**
 * @author Pavel.Sher
 */

public class FluentBuildRunnerCommandLineServiceFactory implements CommandLineBuildServiceFactory {

    public FluentBuildRunnerCommandLineServiceFactory() {
    }

    @NotNull
  public CommandLineBuildService createService() {
    return new FluentBuildTaskBuildService();
  }

  @NotNull
  public AgentBuildRunnerInfo getBuildRunnerInfo() {
    return new AgentBuildRunnerInfo() {
      @NotNull
      public String getType() {
        return FluentBuildRunnerConstants.RUNNER_TYPE;
      }

      public boolean canRun(@NotNull final BuildAgentConfiguration buildAgentConfiguration) {
        return true;
      }
    };
  }
}
