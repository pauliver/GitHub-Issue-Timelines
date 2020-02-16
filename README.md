# GitHub-Issue-Timelines

[![Build .NET Core App](https://github.com/pauliver/GitHub-Issue-Timelines/workflows/Build%20.NET%20Core%20App/badge.svg)](https://github.com/pauliver/GitHub-Issue-Timelines/actions?query=workflow%3A%22Build+.NET+Core+App%22)

```yml
name: IRL Schedule Builder
on:
  issues:
     types: [opened,edited]
  schedule:
  - cron: 01 00 * * 1
  
jobs:
  build:
    runs-on: windows-latest
    strategy:
      matrix:
        dotnet: [ '2.1.802' ]
    name: Dotnet ${{ matrix.dotnet }} IRL-Status
    steps:
      - name: Checkout Image Tools
        uses: actions/checkout@v2
        with:
          repository: pauliver/GitHub-Issue-Timelines
          path: IRLIssue
        
      - name: Setup dotnet
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: ${{ matrix.dotnet }}
     
      - name: Restore Dependancies
        run: dotnet restore IRLIssue/

      - name: Build Status Parsing Tools
        run: dotnet build IRLIssue/ --configuration Release
      
      - name: Run the Status Parsing Tools
        run: dotnet D:\a\REPO\REPO\IRLIssue\StatusReport\bin\Release\netcoreapp2.1\StatusReport.dll "${{ secrets.GITHUB_TOKEN }}" OWNER REPO REPO
```
*Originially a private project here: https://github.com/pauliver/IRL-StatusReport*
