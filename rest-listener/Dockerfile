FROM microsoft/dotnet:latest
RUN mkdir /root/app
COPY rest-listener /root/app/rest-listener
COPY shared /root/app/shared
RUN cd /root/app/rest-listener && dotnet restore && dotnet restore
EXPOSE 5000/tcp
ENTRYPOINT dotnet /root/app/rest-listener/bin/Debug/netcoreapp1.0/publish/rest-listener.dll
