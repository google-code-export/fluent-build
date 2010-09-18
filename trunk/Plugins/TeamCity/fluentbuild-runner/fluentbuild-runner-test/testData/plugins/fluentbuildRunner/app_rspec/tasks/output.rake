require "fluentbuild"
require "spec/fluentbuild/spectask"

########################################
namespace :output do

  Spec::FluentBuild::SpecTask.new(:spec_output) do |t|
    t.spec_files = FileList['spec/output/**/*_spec.rb']
  end
end