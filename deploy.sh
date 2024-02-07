#! /bin/bash

(cd src && dotnet build)
(cd src/Effuse.AWS.Handlers && dotnet lambda package)

cdk deploy --profile deployer