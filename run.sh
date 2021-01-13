#!/usr/bin/env bash

DOTNET=$HOME/.dotnet/dotnet

if [ ! -x $DOTNET ] 
then
  echo "Dotnet not installed..."
  chmod +x .dotnet-install.sh
  ./.dotnet-install.sh    
fi

$DOTNET test