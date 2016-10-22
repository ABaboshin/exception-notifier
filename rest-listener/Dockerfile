FROM microsoft/dotnet:latest
COPY . /root/rest-listener
RUN cd /root/rest-listener && dotnet restore && dotnet publish
# COPY bin/Debug/netcoreapp1.0/publish/ /root/
# EXPOSE 5000/tcp
ENTRYPOINT dotnet /root/rest-listener/bin/Debug/netcoreapp1.0/publish/rest-listener.dll
