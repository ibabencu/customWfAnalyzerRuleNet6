# customWfAnalyzerRuleNet6

This library is a custom workflow analyzer rule for .NET 6
It looks for the string set in it (Studio ->BackStage->Settings-> Locations -> Custom Workflow Analyzer rules location)
![image](https://user-images.githubusercontent.com/93514663/215098556-c2efe3bc-1f46-4e51-b406-cef713edc261.png)
In that location, to separate rules, set them under these folders:
foldername net461 -> Legacy,
 foldername  net6.0-windows -> Windows,
 foldername net6.0  ->Portable

https://uipath.atlassian.net/browse/STUD-24436
