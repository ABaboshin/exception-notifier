FROM microsoft/dotnet:latest
RUN mkdir /root/app
COPY notify-processor /root/app/notify-processor
COPY shared /root/app/shared
RUN cd /root/app/notify-processor && dotnet restore && dotnet publish
EXPOSE 5000/tcp
ENTRYPOINT dotnet /root/app/notify-processor/bin/Debug/netcoreapp1.0/publish/notify-processor.dll
