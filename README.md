## Netzwerk

Eine bridged Netzwek namens el_nw einrichten.

## SMTP

cd smtp
docker-compose build
docker-compose up -d

Die Web-UI für den Mailserver ist unter http://127.0.0.1:8080 zu erreichen.

## Apps

docker-compose build
docker-compose up -d

## Benutzen 

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
