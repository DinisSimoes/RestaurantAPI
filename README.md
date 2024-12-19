# RestaurantAPI

## Descrição
O RestaurantAPI é uma API desenvolvida para gerenciar operações de restaurantes, incluindo autenticação de usuários, cadastro de clientes, gerenciamento de itens de menu e controle de pedidos. Construída com .NET Core, a aplicação utiliza Entity Framework Core para interagir com o banco de dados e Docker para facilitar o desenvolvimento e a implantação.

## Estrutura do Projeto

O projeto é dividido em várias camadas, cada uma com uma responsabilidade bem definida. A estrutura do projeto é a seguinte:

```plaintext
RestaurantAPI/
├── RestaurantAPI.API/              # Contém os controladores e configurações da API
├── RestaurantAPI.Application/      # Contém a lógica de negócios e serviços
├── RestaurantAPI.Domain/           # Contém as entidades, interfaces e DTOs
├── RestaurantAPI.Infrastructure/   # Contém a implementação dos repositórios e acesso a dados
├── RestaurantAPI.Tests/            # Contém a implementação dos testes unitários
├── README.md                       # Documentação do projeto
```

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

### Rodando o Projeto
1. **Iniciar o projeto**
   
Pressione F5 no Visual Studio para iniciar a aplicação.
O projeto será executado no URL: https://localhost:7147/swagger/index.html.

2. **Testar os endpoints no Swagger**

Gere um token de autenticação usando o endpoint /login.
No Swagger, clique em "Authorize" e insira o token no formato:
```
Bearer <token_gerado>
```
Após autorizar, você poderá testar os endpoints autenticados.

### Rodar testes unitarios
Para rodar os testes unitários, siga os passos abaixo:

1. Abra o **Visual Studio** ou qualquer outra IDE que você esteja utilizando.
2. Abra a solução do projeto.
3. Navegue até a pasta **RestaurantAPI.Tests**.
4. Execute os testes unitários:

   - Se estiver utilizando o Visual Studio, você pode ir até o menu **Test** e selecionar **Run All Tests**.
   - Ou, se preferir rodar via **CLI** (linha de comando), execute o comando abaixo:

   ```bash
   dotnet test
   ```

## Sugestões

**Segurança**
- Implementar criptografia para dados sensíveis ao gravar informações, como número de celular.
- Armazenar as chaves de segurança, como `secretkeys` em um local seguro, como o **Azure Key Vault** ou **AWS Secrets Manager**.

**Melhorias na arquitetura**
- Se o volume de usuários crescer, implementar **CQRS** (Command Query Responsibility Segregation) para separar as operações de leitura e escrita no banco de dados.

## Tarefas para eu fazer no futuro
As próximas tarefas para o projeto estão listadas nas [Issues do GitHub](https://github.com/DinisSimoes/RestaurantAPI/issues). Algumas das tarefas incluem:
- Implementação de novos endpoints.
- Melhoria na validação de dados.
- Refaturação de código.
