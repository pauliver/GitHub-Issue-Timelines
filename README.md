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

      - name: Build Image Tools
        run: dotnet build IRLIssue/ --configuration Release
      
      - name: Run the IRL Script
        run: dotnet  D:\a\Pauliver.com\Pauliver.com\IRLIssue\bin\Release\netcoreapp3.1\StatusReport.dll  

```
