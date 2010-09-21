/*
 * Copyright 2000-2010 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package jetbrains.buildServer.runner.fluentbuildrunner;

import java.util.HashMap;
import java.util.Map;
import jetbrains.buildServer.fluentbuildrunner.FluentBuildRunnerBundle;
import jetbrains.buildServer.fluentbuildrunner.FluentBuildRunnerConstants;
import jetbrains.buildServer.serverSide.PropertiesProcessor;
import jetbrains.buildServer.serverSide.RunType;
import jetbrains.buildServer.serverSide.RunTypeRegistry;
import jetbrains.buildServer.util.StringUtil;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

/**
 * @author Roman Chernyatchik
 */
public class fluentbuildRunnerRunType extends RunType {

  public fluentbuildRunnerRunType(final RunTypeRegistry runTypeRegistry) {
    runTypeRegistry.registerRunType(this);
  }

  @Override
  @Nullable
  public PropertiesProcessor getRunnerPropertiesProcessor() {
    // Do nothing
    return null;
  }

  @Override
  public String getEditRunnerParamsJspFilePath() {
    return "taskRunnerRunParams.jsp";
  }

  @Override
  public String getViewRunnerParamsJspFilePath() {
    return "viewTaskRunnerRunParams.jsp";
  }

  @Override
  public Map<String, String> getDefaultRunnerProperties() {
    final Map<String, String> map = new HashMap<String, String>();

    final String trueStr = Boolean.TRUE.toString();

    // configuration version
    map.put(FluentBuildRunnerConstants.SERVER_CONFIGURATION_VERSION_PROPERTY,
            FluentBuildRunnerConstants.CURRENT_CONFIG_VERSION);
    return map;
  }

  @Override
  public String getDescription() {
    return FluentBuildRunnerBundle.RUNNER_DESCRIPTION;
  }

  @Override
  public String getDisplayName() {
    return FluentBuildRunnerBundle.RUNNER_DISPLAY_NAME;
  }

  @NotNull
  @Override
  public String getType() {
    return FluentBuildRunnerConstants.RUNNER_TYPE;
  }

  @NotNull
  //@Override
  public String getShortDescription(@NotNull final Map<String, String> runnerParams) {
    StringBuilder result = new StringBuilder();
    if (runnerParams.get("use-custom-build-file") != null) {
      result.append("FluentBuild file: custom");
    } else {
      result.append("FluentBuild file path: ").append(StringUtil.emptyIfNull(runnerParams.get("build-file-path")));
    }
    result.append("\n");
    final String tasks = runnerParams.get(FluentBuildRunnerConstants.SERVER_UI_RAKE_TASKS_PROPERTY);
    result.append("FluentBuild tasks: ").append(StringUtil.isEmpty(tasks) ? "default" : tasks);
    return result.toString();
  }
}