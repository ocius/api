#!/bin/bash

# Simple shell script to install dev dependencies on ubuntu 20.4
# Kevin Chan - 12/08/2020

# https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu
echo 'Installing 2.1 .NET dependencies'
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-2.1 && \
  sudo apt-get install -y dotnet-runtime-2.1 && \
  sudo apt-get install -y aspnetcore-runtime-2.1

# https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools
echo 'Installing NuGet .NET package manager'
sudo apt-get install -y nuget

# https://docs.aws.amazon.com/cli/latest/userguide/install-cliv2-linux.html
echo 'Installing AWS cli v2'
curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
unzip awscliv2.zip
sudo ./aws/install

echo 'Installing AWS Lambda .NET templates'
dotnet new --install Amazon.Lambda.Templates::*

echo 'Installing .NET extension: AWS Lambda '
dotnet tool install Amazon.Lambda.Tools::*
