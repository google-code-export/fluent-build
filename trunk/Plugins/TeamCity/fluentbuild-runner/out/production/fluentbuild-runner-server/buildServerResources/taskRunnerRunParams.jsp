<%--
  ~ Copyright 2000-2010 JetBrains s.r.o.
  ~
  ~ Licensed under the Apache License, Version 2.0 (the "License");
  ~ you may not use this file except in compliance with the License.
  ~ You may obtain a copy of the License at
  ~
  ~ http://www.apache.org/licenses/LICENSE-2.0
  ~
  ~ Unless required by applicable law or agreed to in writing, software
  ~ distributed under the License is distributed on an "AS IS" BASIS,
  ~ WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  ~ See the License for the specific language governing permissions and
  ~ limitations under the License.
  --%>

<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<%@ taglib prefix="l" tagdir="/WEB-INF/tags/layout" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="forms" tagdir="/WEB-INF/tags/forms" %>

<%--Default initial settings format version--%>
<props:hiddenProperty name="ui.fluentbuildRunner.config.version"/>

<l:settingsGroup title="FluentBuild Parameters">
  <tr>
    <th>
      <label for="custom1">Path to Fluent Build file:</label>
    </th>
    <td>
      <props:textProperty name="build-file-path" style="width:30em;" maxlength="256"/>
      <span class="error" id="error_build-file-path"></span>
      <span class="smallNote">Enter Fluent Build dll or directory path. Specified path should be relative to the checkout directory.</span>
    </td>
  </tr>
  <forms:workingDirectory />
  <tr>
    <th><label for="ui.fluentbuildRunner.fluentbuild.tasks.names">FluentBuild tasks: </label></th>
    <td><props:textProperty name="ui.fluentbuildRunner.fluentbuild.tasks.names" style="width:30em;" maxlength="256"/>
      <span class="smallNote">Enter task name if you don't want to use 'default' task.</span>
    </td>
  </tr>
  <tr>
    <th><label for="ui.fluentbuildRunner.fluentbuild.cmd.path">FluentBuild home path: </label></th>
    <td><props:textProperty name="ui.fluentbuildRunner.fluentbuild.cmd.path" style="width:30em;" maxlength="256"/>
      <span class="smallNote">Relative path to fb.exe from checkout directory</span>
    </td>
  </tr>
  <tr>
    <th><label for="ui.fluentbuildRunner.additional.fluentbuild.cmd.params">Additional FluentBuild command line parameters: </label></th>
    <td><props:textProperty name="ui.fluentbuildRunner.additional.fluentbuild.cmd.params" style="width:30em;" maxlength="256"/>
      <span class="smallNote">If isn't empty these parameters will be added to 'fluentbuild' command line.</span>
    </td>
  </tr>
</l:settingsGroup>