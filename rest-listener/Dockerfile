FROM microsoft/dotnet:latest
CMD "sh" "-c" "echo nameserver 8.8.8.8 > /etc/resolv.conf"
RUN mkdir /root/app
COPY shared /root/app/shared
COPY rest-listener /root/app/rest-listener
RUN cd /root/app/rest-listener && dotnet restore && dotnet publish
EXPOSE 5000/tcp
ENTRYPOINT dotnet /root/app/rest-listener/bin/Debug/netcoreapp1.0/publish/rest-listener.dll
