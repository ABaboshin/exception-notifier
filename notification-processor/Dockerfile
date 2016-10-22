FROM microsoft/dotnet:latest
RUN mkdir /root/app
COPY notification-processor /root/app/notification-processor
COPY shared /root/app/shared
RUN cd /root/app/notification-processor && dotnet restore && dotnet publish
EXPOSE 5000/tcp
ENTRYPOINT dotnet /root/app/notification-processor/bin/Debug/netcoreapp1.0/publish/notification-processor.dll
