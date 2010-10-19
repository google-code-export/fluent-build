package jetbrains.buildServer.agent.fluentbuildrunner;

import com.intellij.openapi.diagnostic.Logger;
import com.intellij.util.containers.HashMap;
import java.io.File;
import java.util.*;
import jetbrains.buildServer.RunBuildException;
import jetbrains.buildServer.agent.AgentRuntimeProperties;
import jetbrains.buildServer.agent.fluentbuildrunner.utils.*;
import jetbrains.buildServer.agent.runner.CommandLineBuildService;
import jetbrains.buildServer.agent.runner.ProgramCommandLine;
import jetbrains.buildServer.agent.runner.SimpleProgramCommandLine;
import jetbrains.buildServer.fluentbuildrunner.FluentBuildRunnerConstants;
import jetbrains.buildServer.runner.BuildFileRunnerUtil;
import jetbrains.buildServer.util.*;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import static jetbrains.buildServer.runner.BuildFileRunnerConstants.BUILD_FILE_PATH_KEY;

public class FluentBuildTaskBuildService extends CommandLineBuildService implements FluentBuildRunnerConstants {
  private final Set<File> myFilesToDelete = new HashSet<File>();

  @NotNull
  @Override
  public ProgramCommandLine makeProgramCommandLine() throws RunBuildException {
    List<String> arguments = new ArrayList<String>();

    // runParams - all server-ui options
    // buildParams - system properties (system.*), environment vars (env.*)
    Map<String, String> runParams = new HashMap<String, String>(getBuild().getResolvedParameters().getRunnerParameters());
    Map<String, String> buildParams = new HashMap<String, String>(getBuild().getResolvedParameters().getBuildParameters().getAllParameters());
    final HashMap<String, String> envMap = new HashMap<String, String>(getBuild().getResolvedParameters().getBuildParameters().getEnvironmentVariables());

    String exePath = runParams.get(SERVER_UI_FLUENTBUILD_CMD_PATH);
    final File buildFile = getBuildFile(runParams);
    // FluentBuild options
    // Custom FluentBuildfile if specified

    if (buildFile != null) {
      arguments.add(buildFile.getAbsolutePath());
    }

      // Tasks names
      final String tasks_names = runParams.get(SERVER_UI_FLUENTBUILD_TASKS_PROPERTY);
      if (!PropertiesUtil.isEmptyOrNull(tasks_names)) {
        addCmdlineArguments(arguments, "/c:" +tasks_names);
      }

      // Set Logger to use the TeamCityLogger
      addCmdlineArguments(arguments, "/L:TeamCity");

     // Other arguments
     final String otherArgsString = runParams.get(SERVER_UI_FLUENTBUILD_ADDITIONAL_CMD_PARAMS_PROPERTY);
     if (!TextUtil.isEmptyOrWhitespaced(otherArgsString)) {
       addCmdlineArguments(arguments, otherArgsString);
     }

      return new SimpleProgramCommandLine(envMap, getBuild().getWorkingDirectory().getAbsolutePath(), exePath, arguments);
  }

  @Override
  public void afterProcessFinished() {
    // Remove tmp files
    for (File file : myFilesToDelete) {
      jetbrains.buildServer.util.FileUtil.delete(file);
    }
    myFilesToDelete.clear();
  }

  private void addCmdlineArguments(@NotNull final List<String> argsList, @NotNull final String argsString) {
    final List<String> stringList = StringUtil.splitHonorQuotes(argsString, ' ');
    for (String arg : stringList) {
      argsList.add(TextUtil.stripDoubleQuoteAroundValue(arg));
    }
  }

    private static final Logger LOG = Logger.getInstance(BuildFileRunnerUtil.class.getName());
  @Nullable
  private File getBuildFile(Map<String, String> runParameters) throws RunBuildException {
        final File buildFile;
        final String buildFilePath = runParameters.get(BUILD_FILE_PATH_KEY);
        final File checkoutDir = new File(AgentRuntimeProperties.getCheckoutDir(runParameters));
        buildFile = jetbrains.buildServer.util.FileUtil.resolvePath(checkoutDir, buildFilePath);
        return buildFile;
  }
}
