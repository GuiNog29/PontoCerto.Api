# PontoCerto.Api

# Visão Geral

**PontoCerto.Api** é uma aplicação para gerenciamento de registros de ponto, projetada para ser executada em um ambiente seguro com suporte a HTTPS. Este guia irá ajudá-lo a configurar e executar o sistema em seu ambiente local.

## Requisitos

- **.NET Core SDK** instalado
- **SQL Server** ou outro banco de dados compatível configurado
- Ferramenta de linha de comando `dotnet` disponível no `PATH`

## Passos para Configuração e Execução

### 1. Baixar e Extrair o Arquivo
- Faça o download do arquivo `.zip` contendo a solução.
- Extraia o conteúdo para um diretório de sua escolha.

### 2. Abrir a Solução no Visual Studio
- Abra o **Visual Studio**.
- Navegue até o diretório onde você extraiu os arquivos.
- Abra a solução (`.sln`) da aplicação.

### 3. Restaurar Dependências
No Visual Studio, restaure as dependências do projeto:

- No menu superior, vá em **Build**.
- Selecione ``Restore NuGet Packages``.

### 4. Executar as Migrations do Banco de Dados
Abra o ``Package Manager Console`` no Visual Studio:

- Vá em **Tools > NuGet Package Manager > Package Manager Console**.
- Execute o seguinte comando para aplicar as migrations e configurar o banco de dados:

```bash
dotnet ef database update --project PontoCerto.Infrastructure --startup-project PontoCerto.Api
````

### 5.Após aplicar as migrations, execute a aplicação com o perfil de HTTPS:

No Visual Studio, selecione o projeto PontoCerto.Api.
Certifique-se de que o perfil de execução está configurado para `https`.
Clique em Run para iniciar a aplicação.
