# RestaurantAPI

## Configuração 

### Pré-requisitos
Certifique-se que tem o Docker e o Docker Compose instalados no seu sistema 

[Instalar Docker](https://docs.docker.com/engine/install/)

[Instalar Docker Compose](https://docs.docker.com/compose/install/)

### Passo a Passo
No temrinal entre na pasta ...\RestaurantAPI.API\Docker existente no projeto e rode o comando para instalar o banco via docker
```
docker-compose up -d
```
Para verificar o status do container use o comando
```
docker ps
```
Para encerrar o container docker use o comando
```
docker-compose down
```