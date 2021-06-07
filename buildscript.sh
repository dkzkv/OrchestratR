#docker build -t orchestrated-server-manager -f ./DockerFile-ServerManager .
#docker tag orchestrated-server-manager localhost:5050/orchestrated-server-manager
#docker push localhost:5050/orchestrated-server-manager

docker build -t orchestrated-server -f ./DockerFile-Server .
docker tag orchestrated-server localhost:5050/orchestrated-server
docker push localhost:5050/orchestrated-server