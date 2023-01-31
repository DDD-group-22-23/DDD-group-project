#!/usr/bin/env sh

# This script will only work under a shell such as Bash, zsh etc

git log --date=local --format="Commit: "%h%n"Author: "%cn%n%ad%n"Commit subject: "%n%s%n > CHANGELOG