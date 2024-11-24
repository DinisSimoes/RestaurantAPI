# RestaurantAPI

## Configuração 

### Rodando a BD no componente Docker
#### Pré-requisitos
Certifique-se que tem o Docker e o Docker Compose instalados no seu sistema 

[Instalar Docker](https://docs.docker.com/engine/install/)

[Instalar Docker Compose](https://docs.docker.com/compose/install/)

#### Passo a Passo
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

### Rodando Migrations no PMC (Package Manager Console)

Agora que temos a BD a funcionar é necessário executar as migrations.

Para aplicar as migrations no banco de dados, siga os passos abaixo utilizando o **Package Manager Console** no Visual Studio.

#### Pré-requisitos

- Certifique-se de que o **.NET SDK** esteja instalado corretamente no seu ambiente. Para verificar, execute `dotnet --version` no terminal.

#### Passos para rodar Migrations

1. **Abra o Package Manager Console** no Visual Studio:
   - No Visual Studio, vá em **Tools** > **NuGet Package Manager** > **Package Manager Console**.

2. **Selecione o projeto correto** no **PMC**:
   - Selecione esse projeto de Infrastructure como o **Default Project** no PMC.

3. **Aplique as migrations**:
   Para aplicar todas as migrations pendentes ao banco de dados, execute o comando:

   ```powershell
   Update-Database

## Sugestões

Caso o volume de usuarios da aplicação cresca recomendo implementar a metodologia [cqrs](https://medium.com/@marcelomg21/cqrs-command-query-responsibility-segregation-em-uma-arquitetura-de-micro-servi%C3%A7os-71dcb687a8a9) na coneção à BD.

Para se colocar em produção é altamente recomendável realizar as seguintes mudanças:
1. Colocar encriptação em dados sensiveis ao gravar a informação (Ex: celular)
2. Colocar as keys e secretkeys em algum lugar seguro (Ex. Azure Key Vault ou AWS Secret Manager)
3. Implementar uma validação mais "real" do número de celular (com regex)

## Tarefas para eu fazer no futuro
- [ ] Refatorar a função AddAsync (Create Order), ela está me incomodando
- [ ] Colocar criptografia na gravação dos dados
- [ ] Implementar o Azure Key Vault para armazenamento de keys e secret keys
- [ ] Implementar sistema de logs com Serilog ou NLog - pesquisar depois qual vou aplicar
- [ ] Isolar numa função o tratamento de erros nos endpoints
- [ ] Uma outra para o tratamento de erros dos serviços
- [ ] Criar ficheiro com todos as mensagens de erros, para centralizar todas as mensagens
- [ ] Criar enum para status de pedido (talvez deixar isto no front)
- [ ] Implementar a edição do pedido pela quantidade (tirar ou aumentar um item)
- [ ] Implementar um fluxo do pedido(Ex: não deixar editar quando o pedido estiver em "cooking")
- [ ] Validar o nr de celular
