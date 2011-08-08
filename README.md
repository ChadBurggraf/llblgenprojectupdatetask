# LLBLGen v3.x Project Update Task
#### MSBuild task for dynamically updating LLBLGen v3.x project files.

This task will let you perform dynamic updates to your LLBLGen projects at build time. The task is built to enable invocation of a number of discreet update actions. As of this writing, only the `TypeShortcuts` action has been implemented.

## Actions

Each action enumerated below can be invoked by setting the `Actions` property to the appropriate value. Actions may require certain inputs and/or outputs to be set in order to function correctly.

### TypeShortcuts

Allows you to modify the type name used in the list of known types for custom Type Converters. A common use case for this update would be incrementing the version number to the current build's version number, so that an automatic code-generation task (for example) can still load the type converters assembly.

The action allows you to pass a collection of items to the `TypeShortcuts` parameter. Each item's `Include` should be a regular expression that matches the type name(s) you'd like to update. The item is also required to have at least one piece of metadata defined that identifies the new value(s) to set.

The following example illustrates updating all types whose names begin with *MyNamespace.* to version *2.3.6*.

    <Project ToolsVersion="3.5" DefaultTargets="LLBLGen" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
	  <UsingTask TaskName="LLBLGenProjectUpdate" AssemblyFile="LLBLGenProjectUpdateTask.dll"/>

	  <ItemGroup>
	    <MyNamespace Include="^MyNamespace\..+">
	      <Version>2.3.5.3</Version>
	    </MyNamespace>
	  </ItemGroup>

	  <Target Name="LLBLGen">
	    <LLBLGenProjectUpdate Projects="Example.llblgenproj" Action="TypeShortcuts" TypeShortcuts="@(MyNamespace)"/>
	  </Target>
	</Project>

In an actual build workflow, the version number above would be reflected dynamically from e.g. an AssemblyInfo.cs file.

## License

Licensed under the [MIT](http://www.opensource.org/licenses/mit-license.html) license. See LICENSE.txt.

Copyright (c) 2011 Chad Burggraf.