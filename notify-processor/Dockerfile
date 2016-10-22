FROM microsoft/dotnet:latest
COPY . /root/app
RUN cd /root/app && dotnet restore && dotnet publish
ENTRYPOINT dotnet /root/app/bin/Debug/netcoreapp1.0/publish/app.dll
#ENTRYPOINT sh /root/app/entry-point.sh
