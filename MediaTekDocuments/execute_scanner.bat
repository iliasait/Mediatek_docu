SonarScanner.MSBuild.exe begin /k:"MediatekFormation" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="sqp_73b56bdc2191f91696cd0eccb2b06ad5d25317f1"
MsBuild.exe /t:Rebuild
SonarScanner.MSBuild.exe end /d:sonar.token="sqp_73b56bdc2191f91696cd0eccb2b06ad5d25317f1"