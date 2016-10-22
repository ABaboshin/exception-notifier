1. Den Docker DNS Service starten

docker run -d -v /var/run/docker.sock:/var/run/docker.sock --name dnsdock -p 172.17.0.1:53:53/udp tonistiigi/dnsdock

2. https://github.com/camptocamp/docker_smtp starten, die IP-Adresse des SMTPs notieren
und in die notification-processor/appsettings.json eintragen.
3. Redis starten
docker run -d --name redis --hostname redis redis
4. Die Apps bauen (als root)

build-notification-processor.sh
build-rest-listener.sh

5. Die Apps starten (als root)

docker run -d -p 5000:5000 ab:rest-listener
docker run -d ab:notification-processor

6. 

Wenn man jetzt mit z.B. dem Postman 
POST http://127.0.0.1:5000/api/notification
{
"source" :"prod",
"ExceptionType" :"Version",
"Text" : "pong"
}

ausführt, wird keine Email gesendet.

Wenn man aber
{
"source" :"prod",
"ExceptionType" :"Test",
"Text" : "pong"
}
postet, wird eine Email gesendet.
Siehe notification-processor/appsettings.json.


------

docker ps --filter "status=exited" | awk '{print $1}' | xargs docker rm
docker images | grep "<none>" | awk "{print \$3}" | xargs docker rmi
find '/var/lib/docker/volumes/' -mindepth 1 -maxdepth 1 -type d | grep -vFf <(
  docker ps -aq | xargs docker inspect | jq -r '.[]|.Mounts|.[]|.Name|select(.)'
) | xargs rm -rf
