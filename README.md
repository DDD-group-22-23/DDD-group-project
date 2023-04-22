# DDD-group-project
> DDD group project files

Selenium test branch status
[![Selenium Testing](https://github.com/DDD-group-22-23/DDD-group-project/actions/workflows/testing.yml/badge.svg?branch=feature%2Fselenium)](https://github.com/DDD-group-22-23/DDD-group-project/actions/workflows/testing.yml)

- [DDD-group-project](#ddd-group-project)
  - [System requirements](#system-requirements)
  - [Development](#development)
  - [Resources](#resources)
  - [Contributors](#contributors)
  - [Changelog](#changelog)

## System requirements

For the main webservice
- .NET 6
- MSSQL server
for the backend oauth2 
- Fusionauth
- Mysql
- elk
finally for testing
- selinium
All of the containers are orcastrated using k8s

## Development

  

## Resources

- [Trello](https://trello.com/b/aTTXc03p/ddd-project)
- [Getting started with ASP](https://dotnet.microsoft.com/en-us/learn/aspnet)

## Contributors

- [Rowan Clark](https://github.com/crimsontome) 
- [Chris Boczko](https://github.com/admgecko)
- [David Cain](https://github.com/deev123)
- [Nikolai Valkamo](https://github.com/firefly599)
- [Lawrence Gibson](https://github.com/lgibson02)

## Changelog

Run `./changelog.sh` to generate the changelog after your commit, then `git add . && git commit --amend --no-edit ` to generate the changelog  
The changelog is available [here](CHANGELOG) (This will only work on systems where a Unix shell is present (such as Bash or zsh), will work on Windows under WSL)
