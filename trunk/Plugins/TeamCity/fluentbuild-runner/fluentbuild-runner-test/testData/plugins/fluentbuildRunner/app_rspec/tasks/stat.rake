require "fluentbuild"
require "spec/fluentbuild/spectask"

########################################
namespace :stat do

  Spec::FluentBuild::SpecTask.new(:general) do |t|
    t.spec_files = FileList['spec/statistics/general/**/*_spec.rb']
  end

  Spec::FluentBuild::SpecTask.new(:passed) do |t|
    t.spec_files = FileList['spec/statistics/passed/**/*_spec.rb']
  end

  Spec::FluentBuild::SpecTask.new(:failed) do |t|
    t.spec_files = FileList['spec/statistics/failed/**/*_spec.rb']
  end

  Spec::FluentBuild::SpecTask.new(:error) do |t|
    t.spec_files = FileList['spec/statistics/error/**/*_spec.rb']
  end

  Spec::FluentBuild::SpecTask.new(:ignored) do |t|
    t.spec_files = FileList['spec/statistics/ignored/**/*_spec.rb']
  end

  Spec::FluentBuild::SpecTask.new(:compile_error) do |t|
    t.spec_files = FileList['spec/statistics/compile_error/**/*_spec.rb']
  end
end