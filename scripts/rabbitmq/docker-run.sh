 #!/bin/bash


echo "Running RabbitMq Container using codechallenge/rabbitmq-global-error-handler"
docker run -d -p 4369:4369 -p 5671:5671 -p 15671:15671 -p 5672:5672 -p 15672:15672 codechallenge/rabbitmq-global-error-handler:v0.1