FROM ubuntu:20.04

RUN echo "Installing .NET CLI dependencies" \
    && apt-get update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu66 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
        curl \
        ca-certificates

RUN echo "Installing .NET Core" \
    && dotnet_sdk_version=3.1.401 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

RUN echo "Installing .NET tools" \
    && dotnet tool install -g Amazon.Lambda.Tools \ 
    && export PATH="$PATH:$HOME/.dotnet/tools/"

RUN echo "Installing AWS CLI" \
   && apt-get install -y zip \
   && curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" \
   && unzip awscliv2.zip \
   && ./aws/install \
   && rm -rf ./aws awscliv2.zip 
   
RUN rm -rf /var/lib/apt/lists/*

RUN useradd -m ocius
VOLUME /home/ocius/api
USER ocius

WORKDIR /home/ocius

