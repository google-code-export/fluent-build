require "fluentbuild"
require "fluentbuild/testtask"

########################################
namespace :tests do

  FluentBuild::TestTask.new(:test_output) do |t|
    t.test_files = FileList['tests/output/**/*_test.rb']
  end
end