# [Choice] .NET version: 5.0, 3.1, 2.1
ARG VARIANT=5.0
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:${VARIANT}

# We need to install .NET Core 3.1 for fornax
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends dotnet-runtime-3.1
