require "fluentbuild"
require "fluentbuild/testtask"

########################################
namespace :stat do

  FluentBuild::TestTask.new(:general) do |t|
    t.test_files = FileList['tests/statistics/general/**/*_test.rb']
  end

  FluentBuild::TestTask.new(:passed) do |t|
    t.test_files = FileList['tests/statistics/passed/**/*_test.rb']
  end

  FluentBuild::TestTask.new(:failed) do |t|
    t.test_files = FileList['tests/statistics/failed/**/*_test.rb']
  end

  FluentBuild::TestTask.new(:error) do |t|
    t.test_files = FileList['tests/statistics/error/**/*_test.rb']
  end

  FluentBuild::TestTask.new(:compile_error) do |t|
    t.test_files = FileList['tests/statistics/compile_error/**/*_test.rb']
  end
end