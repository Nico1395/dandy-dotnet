echo "Removing and running compose script... "
docker compose down -v
docker compose build
docker compose up -d
echo "...running!"

echo "Clustering RabbitMQ nodes..."
sleep 4s
docker exec os-rabbitmq-2 rabbitmqctl stop_app
docker exec os-rabbitmq-2 rabbitmqctl reset
docker exec os-rabbitmq-2 rabbitmqctl join_cluster online-shop@os-rabbitmq-1
docker exec os-rabbitmq-2 rabbitmqctl start_app
docker exec os-rabbitmq-2 rabbitmqctl cluster_status
echo "...clustered!"