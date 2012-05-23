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

package jetbrains.buildServer.fluentbuildrunner;

import org.jetbrains.annotations.NonNls;

/**
 * @author Roman Chernyatchik
 */
public interface FluentBuildRunnerConstants {
  @NonNls String CURRENT_CONFIG_VERSION = "1";
  @NonNls String RUNNER_TYPE = "fluentbuildrunner-runner";
  @NonNls String AGENT_BUNDLE_JAR = "fluentbuildrunner-runner.jar";

// Server properties

  // task name
  @NonNls String SERVER_UI_FLUENTBUILD_TASKS_PROPERTY = "ui.fluentbuildRunner.fluentbuild.tasks.names";


  // Additional CMD params
  @NonNls String SERVER_UI_FLUENTBUILD_ADDITIONAL_CMD_PARAMS_PROPERTY = "ui.fluentbuildRunner.additional.fluentbuild.cmd.params";
  @NonNls String SERVER_UI_FLUENTBUILD_CMD_PATH = "ui.fluentbuildRunner.fluentbuild.cmd.path";
  @NonNls String SERVER_CONFIGURATION_VERSION_PROPERTY = "ui.fluentbuildRunner.config.version";

  // Agent properties:
  // Custom fluentbuildrunner tasks runner script
  @NonNls String CUSTOM_RAKERUNNER_SCRIPT = "system.teamcity.fluentbuildrunner.runner.custom.runner";

}
