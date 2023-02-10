# DDD-group-project
> DDD group project files

- [DDD-group-project](#ddd-group-project)
  - [System requirements](#system-requirements)
  - [Development](#development)
  - [Resources](#resources)
  - [Contributors](#contributors)
  - [Changelog](#changelog)

## System requirements

- .NET 6
- Azure Web Apps
- MSSQL server

## Development

- To build the site locally, navigate to Source_Code/RecipeThesaurus/ and run `dotnet run`

- a push to git will trigger a github action to deploy the site - current url is https://recipethesaurus.azurewebsites.net/

## Resources

- [Trello](https://trello.com/b/aTTXc03p/ddd-project)
- [Getting started with ASP](https://dotnet.microsoft.com/en-us/learn/aspnet)

## Contributors

- [Rowan Clark](https://github.com/crimsontome) 
- [Chris Boczko](https://github.com/admgecko)
- [David Cain](https://github.com/deev123)
- [Nikolai Valkamo](https://github.com/firefly599)

## Changelog

Run `./changelog.sh` to generate the changelog after your commit, then `git add . && git commit --amend --no-edit ` to generate the changelog  
The changelog is available [here](CHANGELOG) (This will only work on systems where a Unix shell is present (such as Bash or zsh), will work on Windows under WSL)
