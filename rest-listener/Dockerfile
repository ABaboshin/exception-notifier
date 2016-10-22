FROM microsoft/dotnet:latest
COPY . /root/app
RUN cd /root/app && dotnet restore && dotnet publish
EXPOSE 5000/tcp
ENTRYPOINT dotnet /root/app/bin/Debug/netcoreapp1.0/publish/app.dll