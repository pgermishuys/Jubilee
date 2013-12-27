properties{
	$solution_name = "Jubilee"
	$base_directory = Resolve-Path ..\
	$solution_file = "$base_directory\src\$solution_name.sln"
	$build_configuration = "Release"
	$build_output_directory = "$base_directory\build_output"
	$max_cpu_count = 1	
}
task default -depends Build

task Build {
   Write-Host "building..."
   exec { msbuild /m:$max_cpu_count /p:Configuration="$build_configuration" /p:Platform="Any CPU" /p:OutDir="$build_output_directory"\\ "$solution_file" }
}