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

<style type="text/css">
  .fluentbuild_reporter {
  padding-top: 3px;
  }
  .fluentbuild_reporter_options {
    padding-top: 2px;
    padding-left: 17px;
  }
</style>

<%--Default initial settings format version--%>
<props:hiddenProperty name="ui.fluentbuildRunner.config.version"/>

<l:settingsGroup title="FluentBuild Parameters">
  <tr>
    <th>
      <c:set var="onclick">
        if (this.checked) {
        $('build-file-path').focus();
        }
      </c:set>
      <props:radioButtonProperty name="use-custom-build-file" value="" id="custom1"
                                 checked="${empty propertiesBean.properties['use-custom-build-file']}" onclick="${onclick}"/>
      <label for="custom1">Path to a FluentBuildfile:</label>
    </th>
    <td>
      <props:textProperty name="build-file-path" style="width:30em;" maxlength="256"/>
      <span class="error" id="error_build-file-path"></span>
      <span class="smallNote">Enter FluentBuildfile path if you don't want to use a default one. Specified path should be relative to the checkout directory.</span>
    </td>
  </tr>
  <tr>
    <th>
      <c:set var="onclick">
        if (this.checked) {
        try {
        BS.MultilineProperties.show('build-file', true);
        $('build-file').focus();
        } catch(e) {}
        }
      </c:set>
      <props:radioButtonProperty name="use-custom-build-file" value="true" id="custom2" onclick="${onclick}"/>
      <label for="custom2">FluentBuildfile content:</label>
    </th>
    <td>
      <props:multilineProperty expanded="${propertiesBean.properties['use-custom-build-file'] == true}" name="build-file" rows="10" cols="58" linkTitle="Type the FluentBuildfile content" onkeydown="$('custom2').checked = true;"/>
      <span class="error" id="error_build-file"></span>
    </td>
  </tr>
  <forms:workingDirectory />
  <tr>
    <th><label for="ui.fluentbuildRunner.fluentbuild.tasks.names">FluentBuild tasks: </label></th>
    <td><props:textProperty name="ui.fluentbuildRunner.fluentbuild.tasks.names" style="width:30em;" maxlength="256"/>
      <span class="smallNote">Enter tasks names separated by space character if you don't want to use 'default' task.<br/>E.g. 'test:functionals' or 'mytask:test mytask:test2'.</span>
    </td>
  </tr>
  <tr>
    <th><label for="ui.fluentbuildRunner.additional.fluentbuild.cmd.params">Additional FluentBuild command line parameters: </label></th>
    <td><props:textProperty name="ui.fluentbuildRunner.additional.fluentbuild.cmd.params" style="width:30em;" maxlength="256"/>
      <span class="smallNote">If isn't empty these parameters will be added to 'fluentbuild' command line.</span>
    </td>
  </tr>
</l:settingsGroup>

<l:settingsGroup title="Launching Parameters">
  <tr>
    <th><label for="ui.fluentbuildRunner.ruby.interpreter">Ruby interpreter path: </label></th>
    <td><props:textProperty name="ui.fluentbuildRunner.ruby.interpreter" style="width:30em;" maxlength="256"/>
      <span class="smallNote">If not specified the interpreter will be searched in the PATH.</span>
    </td>
  </tr>
  <tr>
    <th>
      <label>Debug: </label>
    </th>
    <td>
      <props:checkboxProperty name="ui.fluentbuildRunner.fluentbuild.trace.invoke.exec.stages.enabled"/>
      <label for="ui.fluentbuildRunner.fluentbuild.trace.invoke.exec.stages.enabled">Track invoke/execute stages</label>
      <br/>
    </td>
  </tr>
</l:settingsGroup>

<l:settingsGroup title="Tests Reporting">
  <tr>
    <th>
      <label>Attached reporters:</label>
    </th>

    <td>
      <%-- Test Unit --%>
      <div class="fluentbuild_reporter">
        <props:checkboxProperty name="ui.fluentbuildRunner.frameworks.testunit.enabled"/>
        <label for="ui.fluentbuildRunner.frameworks.testunit.enabled">Test::Unit</label>
      </div>

      <%-- Test-Spec --%>
      <div class="fluentbuild_reporter">
        <props:checkboxProperty name="ui.fluentbuildRunner.frameworks.testspec.enabled"/>
        <label for="ui.fluentbuildRunner.frameworks.testspec.enabled">Test-Spec</label>
      </div>

      <%-- Shoulda --%>
      <div class="fluentbuild_reporter">
        <props:checkboxProperty name="ui.fluentbuildRunner.frameworks.shoulda.enabled"/>
        <label for="ui.fluentbuildRunner.frameworks.shoulda.enabled">Shoulda</label>
      </div>

      <%-- RSpec --%>
      <div class="fluentbuild_reporter">
        <props:checkboxProperty name="ui.fluentbuildRunner.frameworks.rspec.enabled"/>
        <label for="ui.fluentbuildRunner.frameworks.rspec.enabled">RSpec</label>
        <div class="fluentbuild_reporter_options">
        <props:textProperty name="ui.fluentbuildRunner.rspec.specoptions" style="width:30em;" maxlength="256"/>
        <span class="smallNote">FluentBuild will be invoked with a "SPEC_OPTS={internal options}
          <span style="font-weight: bold;">{user options}</span>".
        </span>
        </div>
      </div>

      <%-- Cucumber --%>
      <div class="fluentbuild_reporter">
        <props:checkboxProperty name="ui.fluentbuildRunner.frameworks.cucumber.enabled"/>
        <label for="ui.fluentbuildRunner.frameworks.cucumber.enabled">Cucumber</label>
        <div class="fluentbuild_reporter_options">
          <props:textProperty name="ui.fluentbuildRunner.cucumber.options" style="width:30em;" maxlength="256"/>
          <span class="smallNote">FluentBuild will be invoked with a "CUCUMBER_OPTS={internal options}
            <span style="font-weight: bold;">{user options}</span>".
          </span>
        </div>
      </div>
    </td>
  </tr>
</l:settingsGroup>