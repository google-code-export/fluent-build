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
  @NonNls String CURRENT_CONFIG_VERSION = "2";
  @NonNls String RUNNER_TYPE = "fluentbuildrunner-runner";
  @NonNls String AGENT_BUNDLE_JAR = "fluentbuildrunner-runner.jar";

// Server properties

  // task name
  @NonNls String SERVER_UI_RAKE_TASKS_PROPERTY = "ui.fluentbuildRunner.fluentbuildrunner.tasks.names";

  @NonNls String RAKE_MODE_KEY = "TEAMCITY_RAKE_RUNNER_MODE";
  @NonNls String RAKE_MODE_BUILDSERVER = "buildserver";

  // trace/invoke
  @NonNls String SERVER_UI_RAKE_TRACE_INVOKE_EXEC_STAGES_ENABLED = "ui.fluentbuildRunner.fluentbuildrunner.trace.invoke.exec.stages.enabled";
  @NonNls String RAKE_TRACE_INVOKE_EXEC_STAGES_ENABLED_KEY = "TEAMCITY_RAKE_TRACE";

  // Additional CMD params
  @NonNls String SERVER_UI_RAKE_ADDITIONAL_CMD_PARAMS_PROPERTY = "ui.fluentbuildRunner.additional.fluentbuildrunner.cmd.params";

  // Explicit Ruby interpreter lpath
  @NonNls String SERVER_UI_RUBY_INTERPRETER = "ui.fluentbuildRunner.ruby.interpreter";

  // Enable fluentbuildrunner output capturer
  @NonNls String SERVER_UI_RAKE_OUTPUT_CAPTURER_ENABLED = "ui.fluentbuildRunner.fluentbuildrunner.output.capturer.enabled";

  // Test Frameworks

  // Test::Unit
  @NonNls String SERVER_UI_RAKE_TESTUNIT_ENABLED_PROPERTY = "ui.fluentbuildRunner.frameworks.testunit.enabled";

  // RSpec
  @NonNls String SERVER_UI_RAKE_RSPEC_ENABLED_PROPERTY = "ui.fluentbuildRunner.frameworks.rspec.enabled";
  @NonNls String SERVER_UI_RAKE_RSPEC_OPTS_PROPERTY = "ui.fluentbuildRunner.rspec.specoptions";
  @NonNls String RAKE_RSPEC_OPTS_PARAM_NAME = "SPEC_OPTS";

  // Test-Spec
  @NonNls String SERVER_UI_RAKE_TESTSPEC_ENABLED_PROPERTY = "ui.fluentbuildRunner.frameworks.testspec.enabled";

  // Shoulda
  @NonNls String SERVER_UI_RAKE_SHOULDA_ENABLED_PROPERTY = "ui.fluentbuildRunner.frameworks.shoulda.enabled";

  // Cucumber
  @NonNls String SERVER_UI_RAKE_CUCUMBER_ENABLED_PROPERTY = "ui.fluentbuildRunner.frameworks.cucumber.enabled";
  @NonNls String SERVER_UI_RAKE_CUCUMBER_OPTS_PROPERTY = "ui.fluentbuildRunner.cucumber.options";
  @NonNls String RAKE_CUCUMBER_OPTS_PARAM_NAME = "CUCUMBER_OPTS";

  @NonNls String SERVER_CONFIGURATION_VERSION_PROPERTY = "ui.fluentbuildRunner.config.version";

  // Agent properties:
  // Custom fluentbuildrunner tasks runner script
  @NonNls String CUSTOM_RAKERUNNER_SCRIPT = "system.teamcity.fluentbuildrunner.runner.custom.runner";


  // Teamcity FluentBuild Runner Debug and logs
  @NonNls String DEBUG_PROPERTY = "system.teamcity.fluentbuildrunner.runner.debug.mode";

  // SDK hack
  @NonNls String RUBYLIB_ENVIRONMENT_VARIABLE = "RUBYLIB";
  @NonNls String ORIGINAL_SDK_AUTORUNNER_PATH_KEY = "TEAMCIY_RAKE_TU_AUTORUNNER_PATH";
  @NonNls String ORIGINAL_SDK_TESTRUNNERMEDIATOR_PATH_KEY = "TEAMCITY_RAKE_TU_TESTRUNNERMADIATOR_PATH";

  // FluentBuild
  @NonNls String RAKE_CMDLINE_OPTIONS_RAKEFILE = "--fluentbuildfile";

  // Attached frameworks
  @NonNls String RAKERUNNER_USED_FRAMEWORKS_KEY = "TEAMCITY_RAKE_RUNNER_USED_FRAMEWORKS";
}
