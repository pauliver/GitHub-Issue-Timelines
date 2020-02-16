# GitHub-Issue-Timelines


```yml
name: IRL Schedule Builder
on:
  issues:
     types: [opened,edited]

jobs:
  build:
    runs-on: windows-latest
    strategy:
      matrix:
        dotnet: [ '2.1.802' ]
    name: Dotnet ${{ matrix.dotnet }} ImageCompression
    steps:
      - name: Checkout Status Report Tool
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

      - name: Build Status Report Tool
        run: dotnet build IRLIssue/ --configuration Release
      
      - name: Run Status Report Tool
        run: dotnet  .\bin\Release\netcoreapp2.1\StatusReport.dll   ${{ secrets.NEW_TOKEN }} ORG Repo1 Repo2
          path: IRLIssue
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```
